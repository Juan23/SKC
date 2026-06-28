using System;
using System.Collections.Generic;
using System.Linq;
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

                // Inventory
                connection.Execute(@"CREATE TABLE IF NOT EXISTS Inventory (
                            SKU TEXT PRIMARY KEY, Brand TEXT, BaseName TEXT, UOM TEXT, PackMultiplier REAL, Price REAL)");

                // Purchases
                connection.Execute(@"CREATE TABLE IF NOT EXISTS PurchaseLogs (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT, TransactionId TEXT, Date TEXT, SKU TEXT, Qty INTEGER, UnitCost REAL, Supplier TEXT)");

                // Delivery
                connection.Execute(@"CREATE TABLE IF NOT EXISTS DeliveryLogs (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT, TransactionId TEXT, Date TEXT, SKU TEXT, Qty INTEGER, ToBranch TEXT, TotalLineCost REAL DEFAULT 0)");

                // NEW: Inventory Lots for FIFO Costing
                connection.Execute(@"CREATE TABLE IF NOT EXISTS InventoryLots (
                            LotId INTEGER PRIMARY KEY AUTOINCREMENT, SKU TEXT NOT NULL, DateReceived TEXT NOT NULL, 
                            OriginalQty INTEGER NOT NULL, RemainingQty INTEGER NOT NULL, UnitCost REAL NOT NULL)");

                // Add new columns safely
                try { connection.Execute("ALTER TABLE PurchaseLogs ADD COLUMN TransactionId TEXT"); } catch { }
                try { connection.Execute("ALTER TABLE DeliveryLogs ADD COLUMN TransactionId TEXT"); } catch { }
                try { connection.Execute("ALTER TABLE DeliveryLogs ADD COLUMN TotalLineCost REAL DEFAULT 0"); } catch { }
                try { connection.Execute("ALTER TABLE Inventory ADD COLUMN IsActive INTEGER DEFAULT 1"); } catch { }
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
                return connection.Query<BakeryProduct>("SELECT * FROM Inventory WHERE IsActive = 1").ToList();
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
                    // 1. Log the Purchase
                    string sqlPurchase = @"
                        INSERT INTO PurchaseLogs (TransactionId, Date, SKU, Qty, UnitCost, Supplier) 
                        VALUES (@TransactionId, @Date, @SKU, @Qty, @UnitCost, @Supplier)";
                    connection.Execute(sqlPurchase, purchases, transaction: transaction);

                    // 2. Create the Inventory Lot for FIFO tracking
                    string sqlLot = @"
                        INSERT INTO InventoryLots (SKU, DateReceived, OriginalQty, RemainingQty, UnitCost) 
                        VALUES (@SKU, @Date, @Qty, @Qty, @UnitCost)";
                    connection.Execute(sqlLot, purchases, transaction: transaction);

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
                connection.Execute("DELETE FROM PurchaseLogs WHERE TransactionId = @Id", new { Id = transactionId });
                // Note: If you delete a purchase, you would technically need complex logic to reverse FIFO lots. 
                // For now, this just removes the log. Let me know if you need full lot reversal logic.
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

        public static void AddDeliveryBulk(List<DeliveryLog> deliveries)
        {
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
                            double totalCostForThisLine = 0;

                            // Fetch lots oldest first
                            string lotSql = "SELECT * FROM InventoryLots WHERE SKU = @SKU AND RemainingQty > 0 ORDER BY DateReceived ASC";
                            var availableLots = connection.Query<InventoryLot>(lotSql, new { SKU = item.SKU }, transaction).AsList();

                            foreach (var lot in availableLots)
                            {
                                if (qtyNeeded == 0) break;

                                int qtyToTake = Math.Min(qtyNeeded, lot.RemainingQty);
                                totalCostForThisLine += (qtyToTake * lot.UnitCost);

                                lot.RemainingQty -= qtyToTake;
                                qtyNeeded -= qtyToTake;

                                connection.Execute("UPDATE InventoryLots SET RemainingQty = @RemainingQty WHERE LotId = @LotId",
                                    new { RemainingQty = lot.RemainingQty, LotId = lot.LotId }, transaction);
                            }

                            if (qtyNeeded > 0)
                            {
                                throw new Exception($"Insufficient inventory for SKU: {item.SKU}. Short by {qtyNeeded} units.");
                            }

                            item.TotalLineCost = totalCostForThisLine;

                            string insertSql = @"
                                INSERT INTO DeliveryLogs (TransactionId, Date, SKU, Qty, ToBranch, TotalLineCost) 
                                VALUES (@TransactionId, @Date, @SKU, @Qty, @ToBranch, @TotalLineCost)";
                            connection.Execute(insertSql, item, transaction);
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
        }

        public static List<DeliveryTicketSummary> GetDeliveryTickets(DateTime startDate, DateTime endDate)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                string sql = @"
                    SELECT TransactionId, Date, ToBranch, SUM(Qty) AS TotalItems 
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
                connection.Execute("DELETE FROM DeliveryLogs WHERE TransactionId = @Id", new { Id = transactionId });
            }
        }
    }
}