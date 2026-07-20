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
        // Suppresses the detail fetch while a programmatic reload assigns DataSource / clears the
        // selection (both fire SelectionChanged), so the line grid doesn't populate for an unselected
        // row or race a real click.
        private bool suppressSelectionLoad;

        public frmBranchSalesReport()
        {
            InitializeComponent();
        }

        private void frmBranchSalesReport_Load(object sender, EventArgs e)
        {
            cmbBranch.Items.AddRange(new string[] { "Yoho", "Gaisano", "Liloy", "Labason", "Office" });
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

                suppressSelectionLoad = true;
                dgvSales.DataSource = currentSales.Select(s => new SaleSummaryDisplay
                {
                    ClientSaleId = s.ClientSaleId,
                    No = s.LocalId,
                    Date = s.SoldAt.ToString("yyyy-MM-dd HH:mm"),
                    Cashier = s.StaffName,
                    Total = s.TotalAmount,
                    // Voided takes precedence over the shortfall flag - a voided sale was reversed.
                    Flag = s.Voided ? "VOIDED" : (s.HasShortfall ? "SHORTFALL" : "")
                }).ToList();

                if (dgvSales.Columns["ClientSaleId"] != null) dgvSales.Columns["ClientSaleId"].Visible = false;
                if (dgvSales.Columns["Total"] is { } totalColumn) totalColumn.DefaultCellStyle.Format = "N2";
                HighlightRows();
                dgvSales.ClearSelection();
                dgvLines.DataSource = null;

                // Voided sales were reversed, so they don't count toward the day's takings.
                decimal total = currentSales.Where(s => !s.Voided).Sum(s => s.TotalAmount);
                int shortfalls = currentSales.Count(s => s.HasShortfall && !s.Voided);
                int voided = currentSales.Count(s => s.Voided);
                lblTotals.Text = $"{currentSales.Count} sales, total {total:N2}" +
                                 (shortfalls > 0 ? $"   -   {shortfalls} with stock shortfall" : "") +
                                 (voided > 0 ? $"   -   {voided} voided (excluded)" : "");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not load sales for {branch}.\n\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                suppressSelectionLoad = false;
            }
        }

        private void HighlightRows()
        {
            foreach (DataGridViewRow row in dgvSales.Rows)
            {
                if (row.DataBoundItem is not SaleSummaryDisplay d) continue;
                if (d.Flag == "VOIDED")
                {
                    row.DefaultCellStyle.BackColor = Color.Gainsboro;
                    row.DefaultCellStyle.ForeColor = Color.Gray;
                }
                else if (d.Flag == "SHORTFALL")
                {
                    row.DefaultCellStyle.BackColor = Color.MistyRose;
                }
            }
        }

        private async void dgvSales_SelectionChanged(object sender, EventArgs e)
        {
            if (suppressSelectionLoad) return;
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

                if (dgvLines.Columns["Price"] is { } priceColumn) priceColumn.DefaultCellStyle.Format = "N2";
                if (dgvLines.Columns["Amount"] is { } amountColumn) amountColumn.DefaultCellStyle.Format = "N2";

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
        public bool Voided { get; set; }
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
