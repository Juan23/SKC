using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Dapper;
using Microsoft.Data.Sqlite;

namespace SKC_Bakery_Supplies
{
    // A product row in the office POS catalog cache. Sellable = Price > 0 AND Category is
    // RawMaterial or Miscellaneous - the office counter sells raw materials and misc goods,
    // never BakedGood/DecoratedGood (those only ever sell from a branch POS).
    public class CachedProduct
    {
        public string SKU { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string BaseName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Category { get; set; } = string.Empty;
        public int Stock { get; set; }

        public string Display => $"{Brand} {BaseName}".Trim();

        public string SearchDisplay => $"{Display}  -  {Price:N2}  (stock: {Stock})";
    }

    // One row in the day log (pending queue + synced log combined).
    public class DayLogRow
    {
        public string ClientSaleId { get; set; } = string.Empty;
        public string StaffName { get; set; } = string.Empty;
        public DateTime SoldAt { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty; // Pending | Synced | SyncedWithShortfall | Rejected
        public string Detail { get; set; } = string.Empty;
    }

    // The office POS's offline-first local store: a catalog/price/stock cache refreshed on
    // every sync, a durable queue of unsynced sales, and a log of what already synced. Lives
    // in %APPDATA%\SKC Bakery Supplies\pos.db. Same SQLite+Dapper combo as SKC Branch's
    // PosLocalStore, written fresh here per the no-shared-code convention.
    public static class PosLocalStore
    {
        private static readonly string DbDir =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SKC Bakery Supplies");
        private static readonly string connectionString =
            $"Data Source={Path.Combine(DbDir, "pos.db")}";

        public static void Initialize()
        {
            Directory.CreateDirectory(DbDir);
            using var connection = new SqliteConnection(connectionString);

            // Money columns are TEXT, not REAL: Microsoft.Data.Sqlite binds a C# decimal
            // parameter as text, but a REAL column's affinity would then coerce that text
            // into a double on storage (e.g. 145.70 -> 145.6999...), and casting back with
            // (decimal) locks the drift in. TEXT affinity stores the bound text verbatim, so
            // Dapper reads it back into decimal exact. Qty/Stock stay INTEGER - no fraction to lose.
            connection.Execute(@"CREATE TABLE IF NOT EXISTS catalog_cache (
                SKU TEXT PRIMARY KEY,
                Brand TEXT,
                BaseName TEXT,
                Price TEXT NOT NULL DEFAULT '0',
                Category TEXT,
                Stock INTEGER NOT NULL DEFAULT 0)");

            connection.Execute(@"CREATE TABLE IF NOT EXISTS pending_sales (
                ClientSaleId TEXT PRIMARY KEY,
                StaffName TEXT NOT NULL,
                SoldAt TEXT NOT NULL,
                TotalAmount TEXT NOT NULL)");

            connection.Execute(@"CREATE TABLE IF NOT EXISTS pending_sale_lines (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                ClientSaleId TEXT NOT NULL,
                SKU TEXT,
                Description TEXT NOT NULL,
                Qty INTEGER NOT NULL,
                UnitPrice TEXT NOT NULL,
                LineTotal TEXT NOT NULL)");

            connection.Execute(@"CREATE TABLE IF NOT EXISTS sale_log (
                ClientSaleId TEXT PRIMARY KEY,
                StaffName TEXT NOT NULL,
                SoldAt TEXT NOT NULL,
                TotalAmount TEXT NOT NULL,
                Status TEXT NOT NULL,
                Detail TEXT NOT NULL DEFAULT '')");
        }

        // ---- catalog cache ----

        // Replaces cached price/stock with the server's authoritative snapshot.
        public static void RefreshCatalog(List<BakeryProduct> items)
        {
            // An empty snapshot is never a real operating state (the server returns every active
            // product, 0-stock included). Treat it as a bad pull and keep the last good cache
            // rather than DELETE-ing it and blanking the counter until the next successful sync.
            if (items == null || items.Count == 0) return;

            using var connection = new SqliteConnection(connectionString);
            connection.Open();
            using var transaction = connection.BeginTransaction();

            connection.Execute("DELETE FROM catalog_cache", transaction: transaction);
            connection.Execute(@"
                INSERT INTO catalog_cache (SKU, Brand, BaseName, Price, Category, Stock)
                VALUES (@SKU, @Brand, @BaseName, @Price, @Category, @CurrentStock)",
                items, transaction: transaction);

            transaction.Commit();
        }

        public static List<CachedProduct> GetSellableCatalog()
        {
            // Price is a TEXT column (see Initialize); CAST to REAL so the > 0 test is numeric,
            // not a string comparison that would mis-sort values like ".50". The IN filter already
            // excludes a NULL category, so no COALESCE guard is needed here (unlike the branch POS).
            using var connection = new SqliteConnection(connectionString);
            return connection.Query<CachedProduct>(
                "SELECT SKU, Brand, BaseName, Price, Category, Stock FROM catalog_cache WHERE CAST(Price AS REAL) > 0 AND Category IN ('RawMaterial', 'Miscellaneous') ORDER BY BaseName").ToList();
        }

        public static bool HasCatalog()
        {
            using var connection = new SqliteConnection(connectionString);
            return connection.ExecuteScalar<int>("SELECT COUNT(*) FROM catalog_cache") > 0;
        }

        // Keeps the cached stock honest between syncs so the oversell warning sees
        // sales made minutes ago while offline. The next pull overwrites this with
        // the server's authoritative figure.
        public static void DecrementCachedStock(string sku, int qty)
        {
            // Floor at 0 - an offline oversell would otherwise drive cached Stock negative and
            // show "(stock: -N)" in search results until the next authoritative pull. The
            // oversell warning still fires at 0, so nothing is hidden.
            using var connection = new SqliteConnection(connectionString);
            connection.Execute("UPDATE catalog_cache SET Stock = MAX(Stock - @qty, 0) WHERE SKU = @sku", new { sku, qty });
        }

        // ---- pending queue ----

        public static void QueueSale(PosSaleDto sale)
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();
            using var transaction = connection.BeginTransaction();

            connection.Execute(@"
                INSERT INTO pending_sales (ClientSaleId, StaffName, SoldAt, TotalAmount)
                VALUES (@ClientSaleId, @StaffName, @SoldAt, @TotalAmount)",
                new { sale.ClientSaleId, sale.StaffName, SoldAt = sale.SoldAt.ToString("o"), sale.TotalAmount },
                transaction: transaction);

            connection.Execute(@"
                INSERT INTO pending_sale_lines (ClientSaleId, SKU, Description, Qty, UnitPrice, LineTotal)
                VALUES (@ClientSaleId, @SKU, @Description, @Qty, @UnitPrice, @LineTotal)",
                sale.Lines.Select(l => new { sale.ClientSaleId, l.SKU, l.Description, l.Qty, l.UnitPrice, l.LineTotal }),
                transaction: transaction);

            transaction.Commit();
        }

        public static List<PosSaleDto> GetPendingSales(string branch)
        {
            using var connection = new SqliteConnection(connectionString);

            var headers = connection.Query<(string ClientSaleId, string StaffName, string SoldAt, decimal TotalAmount)>(
                "SELECT ClientSaleId, StaffName, SoldAt, TotalAmount FROM pending_sales ORDER BY SoldAt ASC").ToList();

            var sales = new List<PosSaleDto>();
            foreach (var h in headers)
            {
                var lines = connection.Query<PosSaleLineDto>(@"
                    SELECT SKU, Description, Qty, UnitPrice, LineTotal
                    FROM pending_sale_lines WHERE ClientSaleId = @id ORDER BY Id ASC",
                    new { id = h.ClientSaleId }).ToList();

                sales.Add(new PosSaleDto
                {
                    ClientSaleId = h.ClientSaleId,
                    Branch = branch,
                    StaffName = h.StaffName,
                    // Stored as the counter's own wall-clock (see QueueSale); strip the Kind
                    // here so System.Text.Json serializes it with no UTC offset. Otherwise a
                    // Local-kind DateTime round-trips through the wire with "+08:00", the
                    // UTC droplet converts it on deserialize, and the sale lands in
                    // pos_sales.sold_at (TIMESTAMP WITHOUT TIME ZONE) shifted ~8h earlier -
                    // confirmed live on the branch POS: a sale pushed with an offset came
                    // back 8h off, one pushed without an offset came back exact.
                    SoldAt = DateTime.SpecifyKind(
                        DateTime.Parse(h.SoldAt, null, System.Globalization.DateTimeStyles.RoundtripKind),
                        DateTimeKind.Unspecified),
                    TotalAmount = h.TotalAmount,
                    Lines = lines
                });
            }
            return sales;
        }

        public static int PendingCount()
        {
            using var connection = new SqliteConnection(connectionString);
            return connection.ExecuteScalar<int>("SELECT COUNT(*) FROM pending_sales");
        }

        // Moves a pushed sale from the queue into the local log with the server's verdict.
        // Rejected sales move too (they'd fail identically on every retry) but stay
        // visible in the day log with the rejection reason for manual review.
        public static void ResolvePendingSale(string clientSaleId, string status, string detail)
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();
            using var transaction = connection.BeginTransaction();

            connection.Execute(@"
                INSERT OR REPLACE INTO sale_log (ClientSaleId, StaffName, SoldAt, TotalAmount, Status, Detail)
                SELECT ClientSaleId, StaffName, SoldAt, TotalAmount, @status, @detail
                FROM pending_sales WHERE ClientSaleId = @clientSaleId",
                new { clientSaleId, status, detail }, transaction: transaction);

            connection.Execute("DELETE FROM pending_sale_lines WHERE ClientSaleId = @clientSaleId",
                new { clientSaleId }, transaction: transaction);
            connection.Execute("DELETE FROM pending_sales WHERE ClientSaleId = @clientSaleId",
                new { clientSaleId }, transaction: transaction);

            transaction.Commit();
        }

        // Marks an already-synced sale as voided in the local log, after the server has
        // accepted the void and restocked. Purely cosmetic for the day log - the server is
        // the source of truth; the next office report reads the authoritative voided flag.
        public static void MarkSaleVoided(string clientSaleId, string voidedBy)
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Execute(
                "UPDATE sale_log SET Status = 'Voided', Detail = @detail WHERE ClientSaleId = @clientSaleId",
                new { clientSaleId, detail = $"Voided by {voidedBy}" });
        }

        // ---- day log ----

        public static List<DayLogRow> GetDayLog(DateTime day)
        {
            using var connection = new SqliteConnection(connectionString);
            string dayPrefix = day.ToString("yyyy-MM-dd");

            var rows = connection.Query<(string ClientSaleId, string StaffName, string SoldAt, decimal TotalAmount, string Status, string Detail)>(@"
                SELECT ClientSaleId, StaffName, SoldAt, TotalAmount, 'Pending' AS Status, '' AS Detail
                FROM pending_sales WHERE SoldAt LIKE @like
                UNION ALL
                SELECT ClientSaleId, StaffName, SoldAt, TotalAmount, Status, Detail
                FROM sale_log WHERE SoldAt LIKE @like",
                new { like = dayPrefix + "%" }).ToList();

            return rows
                .Select(r => new DayLogRow
                {
                    ClientSaleId = r.ClientSaleId,
                    StaffName = r.StaffName,
                    SoldAt = DateTime.Parse(r.SoldAt, null, System.Globalization.DateTimeStyles.RoundtripKind),
                    TotalAmount = r.TotalAmount,
                    Status = r.Status,
                    Detail = r.Detail
                })
                .OrderByDescending(r => r.SoldAt)
                .ToList();
        }
    }
}
