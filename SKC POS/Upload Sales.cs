using SKC.DataEngine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace SKC_POS
{
    public partial class frmUploadSales : Form
    {
        public frmUploadSales()
        {
            InitializeComponent();
        }

        private void btnUploadSales_Click(object sender, EventArgs e)
        {
            string selectedBranch = "";
            if (rdoIpil.Checked) selectedBranch = "Ipil";
            else if (rdoLiloy.Checked) selectedBranch = "Liloy";
            else if (rdoLabason.Checked) selectedBranch = "Labason";
            else if (rdoGaisano.Checked) selectedBranch = "Gaisano";

            if (string.IsNullOrEmpty(selectedBranch))
            {
                MessageBox.Show("Please select a branch.", "Validation Error");
                return;
            }

            DateTime selectedDate = dtpSaleDate.Value.Date;
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
                Title = $"Select Sales report for {selectedBranch}"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                List<SaleRecord> importedSales = new List<SaleRecord>();

                try
                {
                    string[] lines = File.ReadAllLines(filePath);

                    for (int i = 1; i < lines.Length; i++)
                    {
                        if (string.IsNullOrWhiteSpace(lines[i])) continue;
                        string[] columns = lines[i].Split(',');

                        if (columns.Length < 6) continue;

                        string sku = columns[0];
                        string productName = columns[1];

                        int.TryParse(columns[2], out int quantity);
                        decimal.TryParse(columns[5], out decimal totalAmount);

                        SaleRecord newSale = new SaleRecord
                        {
                            SaleDate = selectedDate,
                            Branch = selectedBranch,
                            SKU = sku,
                            ProductName = productName,
                            Quantity = quantity,
                            TotalAmount = totalAmount
                        };

                        importedSales.Add(newSale);
                    }

                    DatabaseManager.AddSalesBulk(importedSales);

                    MessageBox.Show($"Successfully logged {importedSales.Count} sales items for {selectedBranch} on {selectedDate.ToShortDateString()}", "Import Complete");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Critical failure reading file: {ex.Message}", "Import error.");
                }
            }
        }
    }
}
