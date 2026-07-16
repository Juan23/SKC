using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SKC_Bakery_Supplies
{
    // POS sales synced up from the branches (pos_sales/pos_sale_lines). Shortfall-flagged
    // sales - where FIFO couldn't fully cover a line at sync time, usually unrecorded
    // baking/decorating - are highlighted so the office can chase them up.
    public partial class frmBranchSalesReport : Form
    {
        private List<BranchSaleSummary> currentSales = new List<BranchSaleSummary>();
        private string loadedBranch = null;

        public frmBranchSalesReport()
        {
            InitializeComponent();
        }

        private void frmBranchSalesReport_Load(object sender, EventArgs e)
        {
            cmbBranch.Items.AddRange(new string[] { "Yoho", "Gaisano", "Liloy", "Labason" });
            cmbBranch.SelectedIndex = 0;
            dtpStart.Value = DateTime.Today;
            dtpEnd.Value = DateTime.Today;
        }

        private async void btnLoad_Click(object sender, EventArgs e)
        {
            if (cmbBranch.SelectedItem == null) return;

            string branch = cmbBranch.SelectedItem.ToString();
            DateTime start = dtpStart.Value.Date;
            DateTime end = dtpEnd.Value.Date.AddDays(1).AddSeconds(-1); // inclusive end day

            try
            {
                currentSales = await CentralApiClient.GetBranchSalesAsync(branch, start, end);
                loadedBranch = branch;

                dgvSales.DataSource = currentSales.Select(s => new SaleSummaryDisplay
                {
                    ClientSaleId = s.ClientSaleId,
                    No = s.LocalId,
                    Date = s.SoldAt.ToString("yyyy-MM-dd HH:mm"),
                    Cashier = s.StaffName,
                    Total = s.TotalAmount,
                    Flag = s.HasShortfall ? "SHORTFALL" : ""
                }).ToList();

                if (dgvSales.Columns["ClientSaleId"] != null) dgvSales.Columns["ClientSaleId"].Visible = false;
                HighlightShortfalls();
                dgvSales.ClearSelection();
                dgvLines.DataSource = null;

                decimal total = currentSales.Sum(s => s.TotalAmount);
                int shortfalls = currentSales.Count(s => s.HasShortfall);
                lblTotals.Text = $"{currentSales.Count} sales, total {total:N2}" +
                                 (shortfalls > 0 ? $"   -   {shortfalls} with stock shortfall" : "");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not load sales for {branch}.\n\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void HighlightShortfalls()
        {
            foreach (DataGridViewRow row in dgvSales.Rows)
            {
                if (row.DataBoundItem is SaleSummaryDisplay d && d.Flag == "SHORTFALL")
                    row.DefaultCellStyle.BackColor = Color.MistyRose;
            }
        }

        private async void dgvSales_SelectionChanged(object sender, EventArgs e)
        {
            if (loadedBranch == null || dgvSales.CurrentRow?.DataBoundItem is not SaleSummaryDisplay selected)
                return;

            try
            {
                var lines = await CentralApiClient.GetBranchSaleLinesAsync(loadedBranch, selected.ClientSaleId);

                dgvLines.DataSource = lines.Select(l => new SaleLineDisplay
                {
                    Item = l.Description,
                    Qty = l.Qty,
                    Price = l.UnitPrice,
                    Amount = l.LineTotal,
                    Shortfall = l.ShortfallQty > 0 ? l.ShortfallQty.ToString() : ""
                }).ToList();
                dgvLines.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not load sale detail.\n\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private class SaleSummaryDisplay
        {
            [Browsable(false)]
            public string ClientSaleId { get; set; } = string.Empty;

            [DisplayName("No.")]
            public int No { get; set; }

            [DisplayName("Date")]
            public string Date { get; set; } = string.Empty;

            [DisplayName("Cashier")]
            public string Cashier { get; set; } = string.Empty;

            [DisplayName("Total")]
            public decimal Total { get; set; }

            [DisplayName("Flag")]
            public string Flag { get; set; } = string.Empty;
        }

        private class SaleLineDisplay
        {
            [DisplayName("Item")]
            public string Item { get; set; } = string.Empty;

            [DisplayName("Qty")]
            public int Qty { get; set; }

            [DisplayName("Price")]
            public decimal Price { get; set; }

            [DisplayName("Amount")]
            public decimal Amount { get; set; }

            [DisplayName("Short")]
            public string Shortfall { get; set; } = string.Empty;
        }
    }

    // ---- DTOs for /api/sales (deserialized case-insensitively) ----

    public class BranchSaleSummary
    {
        public int LocalId { get; set; }
        public string ClientSaleId { get; set; } = string.Empty;
        public string StaffName { get; set; } = string.Empty;
        public DateTime SoldAt { get; set; }
        public decimal TotalAmount { get; set; }
        public bool HasShortfall { get; set; }
    }

    public class BranchSaleLine
    {
        public string SKU { get; set; }
        public string Description { get; set; } = string.Empty;
        public int Qty { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal { get; set; }
        public int ShortfallQty { get; set; }
    }
}
