using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SKC_Bakery_Supplies
{
    // The office counter's own sales history over a date range, reading the same /api/sales
    // endpoints the Branch Sales Report uses (no new server endpoint). Voided sales are greyed
    // and excluded from the total; shortfall sales are flagged. Read-only, requires
    // connectivity to load.
    public partial class frmPosSalesHistory : Form
    {
        private readonly string branchName;
        private List<BranchSaleSummary> currentSales = new List<BranchSaleSummary>();
        // Suppresses the detail fetch while a programmatic reload assigns DataSource / clears the
        // selection (both fire SelectionChanged), so the detail pane doesn't populate for a row the
        // user never selected, and so a stray load can't race a real click.
        private bool suppressSelectionLoad;

        public frmPosSalesHistory(string branchName)
        {
            this.branchName = branchName;
            InitializeComponent();
            Text = $"Sales History - {branchName}";
        }

        private void frmPosSalesHistory_Load(object sender, EventArgs e)
        {
            dtpStart.Value = DateTime.Today;
            dtpEnd.Value = DateTime.Today;
            LoadSales();
        }

        private void btnLoad_Click(object sender, EventArgs e) => LoadSales();

        private async void LoadSales()
        {
            DateTime start = dtpStart.Value.Date;
            DateTime end = dtpEnd.Value.Date.AddDays(1).AddSeconds(-1); // inclusive end day

            btnLoad.Enabled = false;
            try
            {
                currentSales = await CentralApiClient.GetBranchSalesAsync(branchName, start, end);

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

                decimal total = currentSales.Where(s => !s.Voided).Sum(s => s.TotalAmount);
                int shortfalls = currentSales.Count(s => s.HasShortfall && !s.Voided);
                int voided = currentSales.Count(s => s.Voided);
                lblTotals.Text = $"{currentSales.Count} sales, total {total:N2}" +
                                 (shortfalls > 0 ? $"   -   {shortfalls} with shortfall" : "") +
                                 (voided > 0 ? $"   -   {voided} voided (excluded)" : "");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not load sales - are you online?\n\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                suppressSelectionLoad = false;
                btnLoad.Enabled = true;
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
            if (dgvSales.CurrentRow?.DataBoundItem is not SaleSummaryDisplay selected) return;

            try
            {
                var lines = await CentralApiClient.GetBranchSaleLinesAsync(branchName, selected.ClientSaleId);
                dgvLines.DataSource = lines.Select(l => new SaleLineDisplay
                {
                    Item = l.Description,
                    Qty = l.Qty,
                    Price = l.UnitPrice,
                    Amount = l.LineTotal,
                    Short = l.ShortfallQty > 0 ? l.ShortfallQty.ToString() : ""
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
            public string Short { get; set; } = string.Empty;
        }
    }
}
