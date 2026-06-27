//BakeryDatabaseManager.cs
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using Microsoft.Data.Sqlite;

namespace SKC_Bakery_Supplies{
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
                            Id INTEGER PRIMARY KEY AUTOINCREMENT, TransactionId TEXT, Date TEXT, SKU TEXT, Qty INTEGER, ToBranch TEXT)");

                // Add new columns safely
                try { connection.Execute("ALTER TABLE PurchaseLogs ADD COLUMN TransactionId TEXT"); } catch { /* Column exists */ }
                try { connection.Execute("ALTER TABLE DeliveryLogs ADD COLUMN TransactionId TEXT"); } catch { /* Column exists */ }
                try { connection.Execute("ALTER TABLE Inventory ADD COLUMN IsActive INTEGER DEFAULT 1"); } catch { }
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
                    string sql = @"
                INSERT INTO PurchaseLogs (TransactionId, Date, SKU, Qty, UnitCost, Supplier) 
                VALUES (@TransactionId, @Date, @SKU, @Qty, @UnitCost, @Supplier)";

                    connection.Execute(sql, purchases, transaction: transaction);
                    transaction.Commit();
                }
            }
        }

        public static List<PurchaseTicketSummary> GetPurchaseTickets(DateTime startDate, DateTime endDate)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                string sql = @"
            SELECT 
                TransactionId, 
                Date, 
                Supplier, 
                SUM(Qty * UnitCost) AS TotalAmount 
            FROM PurchaseLogs 
            WHERE Date >= @Start AND Date <= @End 
            GROUP BY TransactionId, Date, Supplier 
            ORDER BY Date DESC";

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
                string sql = "SELECT * FROM PurchaseLogs WHERE TransactionId = @Id";
                return connection.Query<PurchaseLog>(sql, new { Id = transactionId }).ToList();
            }
        }

        public static void DeletePurchaseTicket(string transactionId)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                // Wipes all rows sharing this specific TransactionId
                connection.Execute("DELETE FROM PurchaseLogs WHERE TransactionId = @Id", new { Id = transactionId });
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
                    string sql = @"
        INSERT INTO DeliveryLogs (TransactionId, Date, SKU, Qty, ToBranch) 
        VALUES (@TransactionId, @Date, @SKU, @Qty, @ToBranch)";

                    connection.Execute(sql, deliveries, transaction: transaction);
                    transaction.Commit();
                }
            }
        }


    
    }
}
