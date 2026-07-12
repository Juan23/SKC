using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Dapper;
using Microsoft.Data.Sqlite;

namespace SKC_Bakery_Supplies
{
    public static class BakeryDatabaseManager
    {
        private static string connectionString = "Data Source = bakery_inventory.db";

        public static void InitializeDatabase()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                connection.Execute(@"CREATE TABLE IF NOT EXISTS Inventory (SKU TEXT PRIMARY KEY, Brand TEXT, BaseName TEXT, Price REAL)");
                connection.Execute(@"CREATE TABLE IF NOT EXISTS PurchaseLogs (Id INTEGER PRIMARY KEY AUTOINCREMENT, TransactionId TEXT, Date TEXT, SKU TEXT, Qty INTEGER, UnitCost REAL, Supplier TEXT)");
                connection.Execute(@"CREATE TABLE IF NOT EXISTS DeliveryLogs (Id INTEGER PRIMARY KEY AUTOINCREMENT, TransactionId TEXT, Date TEXT, SKU TEXT, Qty INTEGER, ToBranch TEXT, TotalLineCost REAL DEFAULT 0)");

                // The FIFO Ledger
                connection.Execute(@"CREATE TABLE IF NOT EXISTS InventoryLots (LotId INTEGER PRIMARY KEY AUTOINCREMENT, SKU TEXT NOT NULL, DateReceived TEXT NOT NULL, OriginalQty INTEGER NOT NULL, RemainingQty INTEGER NOT NULL, UnitCost REAL NOT NULL)");

                try { connection.Execute("ALTER TABLE PurchaseLogs ADD COLUMN TransactionId TEXT"); } catch { }
                try { connection.Execute("ALTER TABLE DeliveryLogs ADD COLUMN TransactionId TEXT"); } catch { }
                try { connection.Execute("ALTER TABLE DeliveryLogs ADD COLUMN TotalLineCost REAL DEFAULT 0"); } catch { }
                try { connection.Execute("ALTER TABLE Inventory ADD COLUMN IsActive INTEGER DEFAULT 1"); } catch { }
                try { connection.Execute("ALTER TABLE DeliveryLogs ADD COLUMN Requester TEXT"); } catch { }
                try { connection.Execute("ALTER TABLE DeliveryLogs ADD COLUMN Reason TEXT"); } catch { }
                try { connection.Execute("ALTER TABLE DeliveryLogs ADD COLUMN IsSynced INTEGER DEFAULT 0"); } catch { }

                try { connection.Execute("ALTER TABLE PurchaseLogs ADD COLUMN IsSynced INTEGER DEFAULT 0"); } catch { }
                try { connection.Execute("ALTER TABLE DeliveryLogs ADD COLUMN IsSynced INTEGER DEFAULT 0"); } catch { }
                try { connection.Execute("ALTER TABLE InventoryLots ADD COLUMN IsSynced INTEGER DEFAULT 0"); } catch { }
            }
        }

        // --- NEW: Check Available Inventory ---
        public static int GetAvailableInventory(string sku)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                return connection.ExecuteScalar<int>("SELECT COALESCE(SUM(RemainingQty), 0) FROM InventoryLots WHERE SKU = @SKU", new { SKU = sku });
            }
        }

        public static List<BakeryProduct> GetAllProducts()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                string sql = @"
            SELECT i.*, 
                   COALESCE((SELECT SUM(RemainingQty) FROM InventoryLots WHERE SKU = i.SKU), 0) AS CurrentStock
            FROM Inventory i 
            WHERE i.IsActive = 1";
                return connection.Query<BakeryProduct>(sql).ToList();
            }
        }

        public static BakeryProduct GetProduct(string sku)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                return connection.QuerySingleOrDefault<BakeryProduct>("SELECT * FROM Inventory WHERE SKU = @SKU", new { SKU = sku });
            }
        }

        public static BakeryProduct GetProductBySkuOrName(string input)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                return connection.QuerySingleOrDefault<BakeryProduct>(
                    "SELECT * FROM Inventory WHERE (SKU = @Input OR BaseName = @Input) AND IsActive = 1",
                    new { Input = input });
            }
        }

        public static void SoftDeleteProduct(string sku)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Execute("UPDATE Inventory SET IsActive = 0 WHERE SKU = @SKU", new { SKU = sku });
            }
        }

        public static void AddPurchasesBulk(List<PurchaseLog> purchases)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    string sqlPurchase = @"INSERT INTO PurchaseLogs (TransactionId, Date, SKU, Qty, UnitCost, Supplier, IsSynced) 
                       VALUES (@TransactionId, @Date, @SKU, @Qty, @UnitCost, @Supplier, 0)";

                    string sqlLot = @"INSERT INTO InventoryLots (SKU, DateReceived, OriginalQty, RemainingQty, UnitCost, IsSynced) 
                  VALUES (@SKU, @Date, @Qty, @Qty, @UnitCost, 0)";

                    transaction.Commit();
                }
            }
        }

        public static List<PurchaseTicketSummary> GetPurchaseTickets(DateTime startDate, DateTime endDate)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                string sql = @"
                    SELECT TransactionId, Date, Supplier, SUM(Qty * UnitCost) AS TotalAmount 
                    FROM PurchaseLogs WHERE Date >= @Start AND Date <= @End 
                    GROUP BY TransactionId, Date, Supplier ORDER BY Date DESC";

                return connection.Query<PurchaseTicketSummary>(sql, new
                {
                    Start = startDate.ToString("yyyy-MM-dd 00:00:00"),
                    End = endDate.ToString("yyyy-MM-dd 23:59:59")
                }).ToList();
            }
        }

        public static List<PurchaseLog> GetPurchaseDetails(string transactionId)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                return connection.Query<PurchaseLog>("SELECT * FROM PurchaseLogs WHERE TransactionId = @Id", new { Id = transactionId }).ToList();
            }
        }

        public static void DeletePurchaseTicket(string transactionId)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // 1. Fetch the original purchase lines
                        var originalLines = connection.Query<PurchaseLog>("SELECT * FROM PurchaseLogs WHERE TransactionId = @Id", new { Id = transactionId }, transaction).AsList();

                        foreach (var line in originalLines)
                        {
                            // 2. Check if we have enough inventory in the specific lot to remove it.
                            // If we try to delete a purchase that has already been "consumed" by deliveries, we abort.
                            int consumedQty = connection.ExecuteScalar<int>(
                                @"SELECT (OriginalQty - RemainingQty) FROM InventoryLots 
                          WHERE SKU = @SKU AND OriginalQty = @Qty AND UnitCost = @Cost",
                                new { SKU = line.SKU, Qty = line.Qty, Cost = line.UnitCost }, transaction);

                            if (consumedQty > 0)
                                throw new Exception($"Cannot delete purchase: {line.SKU} has already been used in deliveries.");

                            // 3. Remove the lot
                            connection.Execute("DELETE FROM InventoryLots WHERE SKU = @SKU AND OriginalQty = @Qty AND UnitCost = @Cost",
                                new { SKU = line.SKU, Qty = line.Qty, Cost = line.UnitCost }, transaction);
                        }

                        // 4. Remove the log
                        connection.Execute("DELETE FROM PurchaseLogs WHERE TransactionId = @Id", new { Id = transactionId }, transaction);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception($"Deletion blocked: {ex.Message}");
                    }
                }
            }
        }

        public static void UpdateProductText(string sku, string newBrand, string newBaseName)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Execute("UPDATE Inventory SET Brand = @Brand, BaseName = @BaseName WHERE SKU = @SKU",
                    new { Brand = newBrand, BaseName = newBaseName, SKU = sku });
            }
        }

        public static List<DeliveryLog> AddDeliveryBulk(List<DeliveryLog> deliveries)
        {
            var executedLogs = new List<DeliveryLog>();

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        foreach (var item in deliveries)
                        {
                            int qtyNeeded = item.Qty;

                            string lotSql = "SELECT * FROM InventoryLots WHERE SKU = @SKU AND RemainingQty > 0 ORDER BY DateReceived ASC";
                            var availableLots = connection.Query<InventoryLot>(lotSql, new { SKU = item.SKU }, transaction).AsList();

                            foreach (var lot in availableLots)
                            {
                                if (qtyNeeded == 0) break;

                                int qtyToTake = Math.Min(qtyNeeded, lot.RemainingQty);
                                double costForThisChunk = (qtyToTake * lot.UnitCost);

                                lot.RemainingQty -= qtyToTake;
                                qtyNeeded -= qtyToTake;

                                connection.Execute("UPDATE InventoryLots SET RemainingQty = @RemainingQty, IsSynced = 0 WHERE LotId = @LotId",
                                                   new { RemainingQty = lot.RemainingQty, LotId = lot.LotId }, transaction);

                                // Create the exact line item for this specific lot
                                var chunkLog = new DeliveryLog
                                {
                                    TransactionId = item.TransactionId,
                                    Date = item.Date,
                                    SKU = item.SKU,
                                    Qty = qtyToTake,
                                    ToBranch = item.ToBranch,
                                    Requester = item.Requester,
                                    Reason = item.Reason,
                                    TotalLineCost = costForThisChunk,
                                    IsSynced = 0
                                };

                                string insertSql = @"INSERT INTO DeliveryLogs (TransactionId, Date, SKU, Qty, ToBranch, TotalLineCost, Requester, Reason, IsSynced) 
                     VALUES (@TransactionId, @Date, @SKU, @Qty, @ToBranch, @TotalLineCost, @Requester, @Reason, 0)";
                                connection.Execute(insertSql, chunkLog, transaction);

                                executedLogs.Add(chunkLog);
                            }

                            if (qtyNeeded > 0) throw new Exception($"Insufficient inventory for SKU: {item.SKU}. Short by {qtyNeeded} units.");
                        }
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            return executedLogs;
        }

        public static List<DeliveryTicketSummary> GetDeliveryTickets(DateTime startDate, DateTime endDate)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                string sql = @"
            SELECT TransactionId, Date, ToBranch, SUM(Qty) AS TotalItems, 
                   MAX(Requester) AS Requester, MAX(Reason) AS Reason, SUM(TotalLineCost) AS TotalCost
            FROM DeliveryLogs WHERE Date >= @Start AND Date <= @End 
            GROUP BY TransactionId, Date, ToBranch ORDER BY Date DESC";

                return connection.Query<DeliveryTicketSummary>(sql, new
                {
                    Start = startDate.ToString("yyyy-MM-dd 00:00:00"),
                    End = endDate.ToString("yyyy-MM-dd 23:59:59")
                }).ToList();
            }
        }

        public static List<DeliveryLog> GetDeliveryDetails(string transactionId)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                return connection.Query<DeliveryLog>("SELECT * FROM DeliveryLogs WHERE TransactionId = @Id", new { Id = transactionId }).ToList();
            }
        }

        public static void DeleteDeliveryTicket(string transactionId)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // 1. Fetch the items we are about to delete
                        var itemsToReturn = connection.Query<DeliveryLog>("SELECT * FROM DeliveryLogs WHERE TransactionId = @Id", new { Id = transactionId }, transaction).AsList();

                        // 2. Restock them as new inventory lots to preserve FIFO math
                        foreach (var item in itemsToReturn)
                        {
                            if (item.Qty > 0)
                            {
                                double returnUnitCost = item.TotalLineCost / item.Qty; // Retrieve the blended cost

                                string restockSql = @"INSERT INTO InventoryLots (SKU, DateReceived, OriginalQty, RemainingQty, UnitCost) 
                                              VALUES (@SKU, @Date, @Qty, @Qty, @UnitCost)";
                                connection.Execute(restockSql, new { SKU = item.SKU, Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Qty = item.Qty, UnitCost = returnUnitCost }, transaction);
                            }
                        }

                        // 3. Delete the ticket
                        connection.Execute("DELETE FROM DeliveryLogs WHERE TransactionId = @Id", new { Id = transactionId }, transaction);

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public static List<DailyDeliveryPrintItem> GetDailyDeliveryConsolidation(DateTime targetDate)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                string sql = @"
            SELECT d.TransactionId, d.ToBranch, d.Requester, d.Reason, d.SKU, i.BaseName, i.Brand, d.Qty, d.TotalLineCost
            FROM DeliveryLogs d
            LEFT JOIN Inventory i ON d.SKU = i.SKU
            WHERE date(d.Date) = date(@TargetDate)
            ORDER BY d.ToBranch, d.TransactionId, i.Brand, i.BaseName";

                return connection.Query<DailyDeliveryPrintItem>(sql, new { TargetDate = targetDate.ToString("yyyy-MM-dd") }).ToList();
            }
        }

        public static void MarkDeliveriesAsSynced(List<string> transactionIds)
        {
            if (transactionIds == null || transactionIds.Count == 0) return;

            using (var connection = new SqliteConnection(connectionString))
            {
                // Updates all matching IDs in one shot
                connection.Execute("UPDATE DeliveryLogs SET IsSynced = 1 WHERE TransactionId IN @Ids", new { Ids = transactionIds });
            }
        }


        public static List<PurchaseLog> GetUnsyncedPurchases()
        {
            using (var connection = new SqliteConnection(connectionString))
                return connection.Query<PurchaseLog>("SELECT * FROM PurchaseLogs WHERE IsSynced = 0 OR IsSynced IS NULL").ToList();
        }

        public static List<DeliveryLog> GetUnsyncedDeliveries()
        {
            using (var connection = new SqliteConnection(connectionString))
                return connection.Query<DeliveryLog>("SELECT * FROM DeliveryLogs WHERE IsSynced = 0 OR IsSynced IS NULL").ToList();
        }

        public static List<InventoryLot> GetUnsyncedInventoryLots()
        {
            using (var connection = new SqliteConnection(connectionString))
                return connection.Query<InventoryLot>("SELECT * FROM InventoryLots WHERE IsSynced = 0 OR IsSynced IS NULL").ToList();
        }

        public static void MarkPurchasesAsSynced(List<int> ids)
        {
            if (ids != null && ids.Any())
                using (var connection = new SqliteConnection(connectionString))
                    connection.Execute("UPDATE PurchaseLogs SET IsSynced = 1 WHERE Id IN @Ids", new { Ids = ids });
        }

        public static void MarkDeliveriesAsSyncedById(List<int> ids)
        {
            if (ids != null && ids.Any())
                using (var connection = new SqliteConnection(connectionString))
                    connection.Execute("UPDATE DeliveryLogs SET IsSynced = 1 WHERE Id IN @Ids", new { Ids = ids });
        }

        public static void MarkInventoryLotsAsSynced(List<int> ids)
        {
            if (ids != null && ids.Any())
                using (var connection = new SqliteConnection(connectionString))
                    connection.Execute("UPDATE InventoryLots SET IsSynced = 1 WHERE LotId IN @Ids", new { Ids = ids });
        }

    } //end
}