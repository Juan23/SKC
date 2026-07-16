using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Dapper;
using Microsoft.Data.Sqlite;

namespace SKC_Branch
{
    // A product row in the POS catalog cache. Only rows with Price > 0 are sellable -
    // the owner prices what branches may sell (bread, candles) and leaves raw
    // materials/intermediaries (flour, chiffon) at 0, so an unpriced item is simply
    // not findable at the counter.
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

    // The POS's offline-first local store: a catalog/price/stock cache refreshed on every
    // sync, a durable queue of unsynced sales, and a log of what already synced. Lives in
    // %APPDATA%\SKC Branch\pos.db next to config.json. Same SQLite+Dapper combo as the
    // legacy SKC.DataEngine, but written fresh here per the no-shared-code convention.
    public static class PosLocalStore
    {
        private static readonly string DbDir =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SKC Branch");
        private static readonly string connectionString =
            $"Data Source={Path.Combine(DbDir, "pos.db")}";

        public static void Initialize()
        {
            Directory.CreateDirectory(DbDir);
            using var connection = new SqliteConnection(connectionString);

            connection.Execute(@"CREATE TABLE IF NOT EXISTS catalog_cache (
                SKU TEXT PRIMARY KEY,
                Brand TEXT,
                BaseName TEXT,
                Price REAL NOT NULL DEFAULT 0,
                Category TEXT,
                Stock INTEGER NOT NULL DEFAULT 0)");

            connection.Execute(@"CREATE TABLE IF NOT EXISTS pending_sales (
                ClientSaleId TEXT PRIMARY KEY,
                StaffName TEXT NOT NULL,
                SoldAt TEXT NOT NULL,
                TotalAmount REAL NOT NULL)");

            connection.Execute(@"CREATE TABLE IF NOT EXISTS pending_sale_lines (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                ClientSaleId TEXT NOT NULL,
                SKU TEXT,
                Description TEXT NOT NULL,
                Qty INTEGER NOT NULL,
                UnitPrice REAL NOT NULL,
                LineTotal REAL NOT NULL)");

            connection.Execute(@"CREATE TABLE IF NOT EXISTS sale_log (
                ClientSaleId TEXT PRIMARY KEY,
                StaffName TEXT NOT NULL,
                SoldAt TEXT NOT NULL,
                TotalAmount REAL NOT NULL,
                Status TEXT NOT NULL,
                Detail TEXT NOT NULL DEFAULT '')");
        }

        // ---- catalog cache ----

        // Replaces cached price/stock with the server's authoritative snapshot.
        public static void RefreshCatalog(List<BranchStockItem> items)
        {
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
            using var connection = new SqliteConnection(connectionString);
            return connection.Query<CachedProduct>(
                "SELECT SKU, Brand, BaseName, Price, Category, Stock FROM catalog_cache WHERE Price > 0 ORDER BY BaseName").ToList();
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
            using var connection = new SqliteConnection(connectionString);
            connection.Execute("UPDATE catalog_cache SET Stock = Stock - @qty WHERE SKU = @sku", new { sku, qty });
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

            var headers = connection.Query<(string ClientSaleId, string StaffName, string SoldAt, double TotalAmount)>(
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
                    SoldAt = DateTime.Parse(h.SoldAt, null, System.Globalization.DateTimeStyles.RoundtripKind),
                    TotalAmount = (decimal)h.TotalAmount,
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

        // ---- day log ----

        public static List<DayLogRow> GetDayLog(DateTime day)
        {
            using var connection = new SqliteConnection(connectionString);
            string dayPrefix = day.ToString("yyyy-MM-dd");

            var rows = connection.Query<(string ClientSaleId, string StaffName, string SoldAt, double TotalAmount, string Status, string Detail)>(@"
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
                    TotalAmount = (decimal)r.TotalAmount,
                    Status = r.Status,
                    Detail = r.Detail
                })
                .OrderByDescending(r => r.SoldAt)
                .ToList();
        }
    }
}
