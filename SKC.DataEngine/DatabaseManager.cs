using Dapper;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Linq;

namespace SKC.DataEngine
{
    public static class DatabaseManager // Static because there is only one database connection rule for the whole app
    {
        // The hardcoded file path for your offline database
        private static string connectionString = "Data Source=pos_inventory.db";

        public static void InitializeDatabase()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                // SKU is now the absolute identifier
                connection.Execute("CREATE TABLE IF NOT EXISTS Inventory (SKU TEXT PRIMARY KEY, Name TEXT, Price REAL)");

                connection.Execute(@"CREATE TABLE IF NOT EXISTS Sales (
                                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                    SaleDate TEXT,
                                    Branch TEXT,
                                    SKU TEXT,
                                    ProductName TEXT,
                                    Quantity INTEGER,
                                    TotalAmount REAL)");

            }
        }

        public static void AddProductsBulk(List<Product> products)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    // The "Upsert" Command
                    // If the SKU is new, it inserts. If the SKU exists, it updates the Name and Price to the newest version.
                    string sql = @"
                INSERT INTO Inventory (SKU, Name, Price) 
                VALUES (@SKU, @Name, @Price)
                ON CONFLICT(SKU) DO UPDATE SET 
                    Name = excluded.Name, 
                    Price = excluded.Price";

                    connection.Execute(sql, products, transaction: transaction);

                    transaction.Commit();
                }
            }
        }

        public static void AddProduct(Product newProduct)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                // Dapper safely maps the C# object to the SQL query to prevent hacking (SQL Injection)
                string sql = "INSERT INTO Inventory (SKU, Name, Price) VALUES (@SKU, @Name, @Price)";
                connection.Execute(sql, newProduct);
            }
        }

        public static List<Product> GetAllProducts()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                // Pulls all rows and instantly converts them back into a List of C# Objects
                return connection.Query<Product>("SELECT * FROM Inventory").ToList();
            }
        }

        public static void AddSalesBulk(List<SaleRecord> sales)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    string sql = @"
                                INSERT INTO Sales (SaleDate, Branch, SKU, ProductName, Quantity, TotalAmount)
                                VALUES (@SaleDate, @Branch, @SKU, @ProductName, @Quantity, @TotalAmount)";

                    connection.Execute(sql, sales, transaction: transaction);
                    transaction.Commit();
                }
            }
        }

    }
}