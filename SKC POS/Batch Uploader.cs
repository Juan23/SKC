using SKC.DataEngine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SKC_POS
{
    public partial class frmUploadData : Form
    {
        public frmUploadData()
        {
            InitializeComponent();
        }

        private void frmUploadData_Load(object sender, EventArgs e)
        {

        }

        private void btnUploadProductList_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
                Title = "Select Clean Master Product List"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                List<Product> importedProducts = new List<Product>();

                try
                {
                    string[] lines = File.ReadAllLines(filePath);

                    // Start at index 1 to skip "SKU,Name,Price per Piece" header
                    for (int i = 1; i < lines.Length; i++)
                    {
                        if (string.IsNullOrWhiteSpace(lines[i])) continue;

                        string[] columns = lines[i].Split(',');

                        if (columns.Length < 3) continue;

                        // Extract all 3 properties based on your clean CSV
                        string sku = columns[0];
                        string name = columns[1];
                        decimal price = 0;

                        if (decimal.TryParse(columns[2], out decimal parsedPrice))
                        {
                            price = parsedPrice;
                        }

                        // Add to the list with the SKU included
                        importedProducts.Add(new Product { SKU = sku, Name = name, Price = price });
                    }

                    DatabaseManager.AddProductsBulk(importedProducts);

                    MessageBox.Show($"Successfully seeded database with {importedProducts.Count} products from clean file.", "Import Complete");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Critical failure reading file: {ex.Message}", "Import Error");
                }
            }
        }
    }
}
