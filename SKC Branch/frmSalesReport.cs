using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SKC_Branch
{
    // End-of-day sales report: a printable per-sale list for a date range (defaulting to today),
    // plus a line-level CSV export for analysis in Excel.
    //
    // This is a management/accountability report, NOT a customer receipt - the POS deliberately has
    // no receipt printing.
    //
    // The server is the source of truth (it alone knows about voids and sales rung up on another
    // device). But closing time is exactly when a branch might be offline, so a today-only range
    // falls back to the local POS store rather than refusing to print - clearly banner-flagged,
    // since local data can't see other devices or remote voids. The CSV export has no such fallback:
    // item lines for already-synced sales only exist server-side.
    public partial class frmSalesReport : Form
    {
        private readonly string branchName;
        private List<ReportRow> rows = new List<ReportRow>();
        // True when the currently-displayed rows came from the local store because the server was
        // unreachable. Drives the on-screen banner, the printed banner, and blocking CSV export.
        private bool offlineData;
        private int printIndex;
        private readonly PrintDocument printDoc = new PrintDocument();
        // The range the displayed rows were actually loaded for. Print and export both work off
        // THIS, never the live pickers: the user can move a picker without pressing Load, and a
        // printed header or an export that disagreed with the rows on screen would be worse than
        // useless on an accountability document. lblStale warns when the two have diverged.
        private DateTime loadedStart;
        private DateTime loadedEnd;
        private bool hasLoaded;

        public frmSalesReport(string branchName)
        {
            this.branchName = branchName;
            InitializeComponent();
            Text = $"Sales Report - {branchName}";
            lblHeader.Text = $"Sales Report - {branchName}";
        }

        private void frmSalesReport_Load(object sender, EventArgs e)
        {
            dtpStart.Value = DateTime.Today;
            dtpEnd.Value = DateTime.Today;
            LoadReport();
        }

        private void btnToday_Click(object sender, EventArgs e)
        {
            dtpStart.Value = DateTime.Today;
            dtpEnd.Value = DateTime.Today;
            LoadReport();
        }

        private void btnLoad_Click(object sender, EventArgs e) => LoadReport();

        // ---- loading ----

        private async void LoadReport()
        {
            DateTime startDay = dtpStart.Value.Date;
            DateTime endDay = dtpEnd.Value.Date;

            if (startDay > endDay)
            {
                MessageBox.Show("The \"From\" date is after the \"To\" date.", "Check Dates",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DateTime end = endDay.AddDays(1).AddSeconds(-1); // inclusive end day

            btnLoad.Enabled = false;
            btnToday.Enabled = false;
            try
            {
                var sales = await BranchApiClient.GetBranchSalesAsync(branchName, startDay, end);
                rows = sales
                    .Select(s => new ReportRow
                    {
                        No = s.LocalId.ToString(),
                        SoldAt = s.SoldAt,
                        Cashier = s.StaffName,
                        Total = s.TotalAmount,
                        // Voided wins over shortfall: a voided sale was reversed outright.
                        Flag = s.Voided ? "VOIDED" : (s.HasShortfall ? "SHORTFALL" : ""),
                        Counted = !s.Voided
                    })
                    .OrderBy(r => r.SoldAt)
                    .ToList();
                offlineData = false;
            }
            catch (Exception ex)
            {
                // Only today can be reconstructed locally - the local store keeps a day log, not
                // history. For any other range there's nothing to fall back to, so report and stop.
                if (startDay != DateTime.Today || endDay != DateTime.Today)
                {
                    MessageBox.Show(
                        $"Could not load sales for this date range - are you online?\n\n{ex.Message}\n\n" +
                        "Past days can only be read from the server. Today's report still works offline.",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btnLoad.Enabled = true;
                    btnToday.Enabled = true;
                    return;
                }

                rows = PosLocalStore.GetDayLog(DateTime.Today)
                    .Select(r => new ReportRow
                    {
                        No = "",              // the server assigns the sale no.; unsynced sales have none yet
                        SoldAt = r.SoldAt,
                        Cashier = r.StaffName,
                        Total = r.TotalAmount,
                        Flag = r.Status switch
                        {
                            "Voided" => "VOIDED",
                            "Rejected" => "REJECTED",
                            "Pending" => "UNSYNCED",
                            "SyncedWithShortfall" => "SHORTFALL",
                            _ => ""
                        },
                        // Rejected sales never counted server-side; voided ones were reversed.
                        Counted = r.Status != "Rejected" && r.Status != "Voided"
                    })
                    .OrderBy(r => r.SoldAt)
                    .ToList();
                offlineData = true;
            }
            finally
            {
                btnLoad.Enabled = true;
                btnToday.Enabled = true;
            }

            loadedStart = startDay;
            loadedEnd = endDay;
            hasLoaded = true;
            BindRows();
        }

        // Fires when either picker moves. The rows on screen still belong to the old range, so warn
        // rather than silently letting Print/Export emit a document for a range nobody loaded.
        private void DateChanged(object sender, EventArgs e) => RefreshStaleHint();

        private void RefreshStaleHint()
        {
            bool stale = hasLoaded &&
                         (dtpStart.Value.Date != loadedStart || dtpEnd.Value.Date != loadedEnd);
            lblStale.Visible = stale;
            lblStale.Text = stale ? "Dates changed - press Load" : "";
        }

        private void BindRows()
        {
            dgvSales.DataSource = rows.Select(r => new SaleDisplay
            {
                No = r.No,
                Time = r.SoldAt.ToString("yyyy-MM-dd HH:mm"),
                Cashier = r.Cashier,
                Total = r.Total,
                Flag = r.Flag
            }).ToList();
            dgvSales.ClearSelection();
            HighlightRows();

            lblOffline.Visible = offlineData;
            lblOffline.Text = offlineData ? "OFFLINE - local copy only" : "";
            RefreshStaleHint();

            lblTotals.Text = BuildSummaryLine();
        }

        private void HighlightRows()
        {
            foreach (DataGridViewRow row in dgvSales.Rows)
            {
                if (row.DataBoundItem is not SaleDisplay d) continue;
                if (d.Flag == "VOIDED" || d.Flag == "REJECTED")
                {
                    row.DefaultCellStyle.BackColor = Color.Gainsboro;
                    row.DefaultCellStyle.ForeColor = Color.Gray;
                }
                else if (d.Flag == "SHORTFALL" || d.Flag == "UNSYNCED")
                {
                    row.DefaultCellStyle.BackColor = Color.MistyRose;
                }
            }
        }

        private string BuildSummaryLine()
        {
            var s = Summarize();
            return $"{s.CountedSales} sales, total {s.GrossTotal:N2}" +
                   (s.VoidedCount > 0 ? $"   -   {s.VoidedCount} voided ({s.VoidedAmount:N2}) excluded" : "") +
                   (s.ShortfallCount > 0 ? $"   -   {s.ShortfallCount} with shortfall" : "") +
                   (s.UnsyncedCount > 0 ? $"   -   {s.UnsyncedCount} not yet synced" : "");
        }

        private Summary Summarize() => new Summary
        {
            CountedSales = rows.Count(r => r.Counted),
            GrossTotal = rows.Where(r => r.Counted).Sum(r => r.Total),
            VoidedCount = rows.Count(r => r.Flag == "VOIDED"),
            VoidedAmount = rows.Where(r => r.Flag == "VOIDED").Sum(r => r.Total),
            ShortfallCount = rows.Count(r => r.Flag == "SHORTFALL"),
            RejectedCount = rows.Count(r => r.Flag == "REJECTED"),
            UnsyncedCount = rows.Count(r => r.Flag == "UNSYNCED")
        };

        // ---- print ----

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (rows.Count == 0)
            {
                MessageBox.Show("There are no sales in this date range to print.", "Nothing to Print",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            printIndex = 0;
            printDoc.PrintPage -= RenderReport;
            printDoc.PrintPage += RenderReport;

            using var previewForm = new Form
            {
                Text = "Sales Report Preview",
                Width = 800,
                Height = 1000,
                ShowIcon = false,
                StartPosition = FormStartPosition.CenterParent
            };
            var btnPrintNow = new Button
            {
                Text = "Print Report",
                Dock = DockStyle.Top,
                Height = 45,
                Cursor = Cursors.Hand,
                BackColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F)
            };
            btnPrintNow.FlatAppearance.BorderColor = Color.Silver;
            btnPrintNow.Click += (s, ev) =>
            {
                using var pd = new PrintDialog { Document = printDoc, UseEXDialog = true };
                if (pd.ShowDialog() != DialogResult.OK) return;
                try
                {
                    printIndex = 0; // the preview already walked the list to the end
                    printDoc.Print();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Print failed: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            var preview = new PrintPreviewControl { Dock = DockStyle.Fill, Document = printDoc };
            previewForm.Controls.Add(preview);
            previewForm.Controls.Add(btnPrintNow);
            previewForm.ShowDialog();
        }

        private void RenderReport(object? sender, PrintPageEventArgs e)
        {
            Graphics? g = e.Graphics;
            if (g == null) return;

            using var titleFont = new Font("Courier New", 16, FontStyle.Bold);
            using var headerFont = new Font("Courier New", 12, FontStyle.Bold);
            using var regularFont = new Font("Courier New", 11);
            Brush brush = Brushes.Black;

            int margin = 50;
            int y = 50;
            int rightEdge = e.PageBounds.Width - margin;

            // Column origins, left to right: no. / time / cashier / total (right-aligned) / flag.
            int colNo = margin;
            int colTime = margin + 60;
            int colCashier = margin + 190;
            int colTotalRight = margin + 420;
            int colFlag = margin + 440;

            g.DrawString("SKC BAKERY SUPPLIES", titleFont, brush, margin, y);
            y += 30;
            g.DrawString($"SALES REPORT - {branchName.ToUpper()}", headerFont, brush, margin, y);
            y += 25;

            // The loaded range, not the pickers - see the loadedStart/loadedEnd note above.
            string period = loadedStart == loadedEnd
                ? $"DATE: {loadedStart:yyyy-MM-dd}"
                : $"PERIOD: {loadedStart:yyyy-MM-dd} to {loadedEnd:yyyy-MM-dd}";
            g.DrawString(period, regularFont, brush, margin, y);
            y += 20;
            g.DrawString($"PRINTED: {DateTime.Now:yyyy-MM-dd hh:mm tt}", regularFont, brush, margin, y);
            y += 30;

            if (offlineData)
            {
                g.DrawString("*** OFFLINE COPY - printed from this computer's local records.", regularFont, brush, margin, y);
                y += 18;
                g.DrawString("    May exclude sales made on another POS device, and does not", regularFont, brush, margin, y);
                y += 18;
                g.DrawString("    reflect voids made elsewhere. ***", regularFont, brush, margin, y);
                y += 25;
            }

            g.DrawLine(Pens.Black, margin, y, rightEdge, y);
            y += 10;
            g.DrawString("NO.", headerFont, brush, colNo, y);
            g.DrawString("DATE / TIME", headerFont, brush, colTime, y);
            g.DrawString("CASHIER", headerFont, brush, colCashier, y);
            g.DrawString("TOTAL", headerFont, brush, colTotalRight - 40, y);
            g.DrawString("FLAG", headerFont, brush, colFlag, y);
            y += 25;
            g.DrawLine(Pens.Black, margin, y, rightEdge, y);
            y += 15;

            // Space the summary block below will need. Derived from the optional lines actually
            // present rather than a fixed guess: a flat reservation sized for the no-flags case
            // pushes the signature lines past the bottom margin as soon as a range contains a
            // voided or shortfall sale.
            var summary = Summarize();
            int optionalLines = (summary.VoidedCount > 0 ? 1 : 0)
                              + (summary.ShortfallCount > 0 ? 1 : 0)
                              + (summary.RejectedCount > 0 ? 1 : 0)
                              + (summary.UnsyncedCount > 0 ? 1 : 0);
            int footerHeight = 165 + (20 * optionalLines);

            while (printIndex < rows.Count)
            {
                var r = rows[printIndex];

                g.DrawString(r.No, regularFont, brush, colNo, y);
                g.DrawString(r.SoldAt.ToString("yyyy-MM-dd HH:mm"), regularFont, brush, colTime, y);
                g.DrawString(Truncate(r.Cashier, 18), regularFont, brush, colCashier, y);

                string total = r.Total.ToString("N2");
                SizeF totalSize = g.MeasureString(total, regularFont);
                g.DrawString(total, regularFont, brush, colTotalRight - totalSize.Width, y);

                g.DrawString(r.Flag, regularFont, brush, colFlag, y);
                y += 22;

                printIndex++;

                // Break early enough that the summary block below still fits on this page. Because
                // footerHeight grows by the same 20 per optional line that the block itself does,
                // the last signature line lands a constant ~36 above the bottom margin for any
                // combination of flags. (A row is always drawn before this check, so a page can
                // never come out empty and the loop can't stall.)
                if (y > e.MarginBounds.Bottom - footerHeight)
                {
                    e.HasMorePages = true;
                    return;
                }
            }

            // ---- summary, on the last page only ----
            var s = summary;
            y += 10;
            g.DrawLine(Pens.Black, margin, y, rightEdge, y);
            y += 15;

            g.DrawString($"SALES COUNTED ....... {s.CountedSales}", headerFont, brush, margin, y);
            y += 22;
            g.DrawString($"GROSS TOTAL ......... {s.GrossTotal:N2}", headerFont, brush, margin, y);
            y += 22;

            if (s.VoidedCount > 0)
            {
                g.DrawString($"VOIDED (excluded) ... {s.VoidedCount}  ({s.VoidedAmount:N2})", regularFont, brush, margin, y);
                y += 20;
            }
            if (s.ShortfallCount > 0)
            {
                g.DrawString($"WITH SHORTFALL ...... {s.ShortfallCount}", regularFont, brush, margin, y);
                y += 20;
            }
            if (s.RejectedCount > 0)
            {
                g.DrawString($"REJECTED (excluded) . {s.RejectedCount}", regularFont, brush, margin, y);
                y += 20;
            }
            if (s.UnsyncedCount > 0)
            {
                g.DrawString($"NOT YET SYNCED ...... {s.UnsyncedCount}  (included above)", regularFont, brush, margin, y);
                y += 20;
            }

            y += 30;
            g.DrawString("Counted by: ____________________", regularFont, brush, margin, y);
            y += 30;
            g.DrawString("Verified by: ____________________", regularFont, brush, margin, y);

            e.HasMorePages = false;
        }

        private static string Truncate(string value, int max) =>
            string.IsNullOrEmpty(value) || value.Length <= max ? value : value.Substring(0, max);

        // ---- CSV export ----

        // Line-level, so a SUMIFS in Excel can total per item - the printed report is sale-level and
        // can't support that. Server-only: item lines for already-synced sales live only in Postgres.
        private async void btnExportCsv_Click(object sender, EventArgs e)
        {
            // Export the range currently on screen, so the CSV and the printed report always agree.
            // This also keeps the offlineData guard below meaningful - it describes how THESE rows
            // were loaded, which would say nothing about some other range the pickers now show.
            if (!hasLoaded)
            {
                MessageBox.Show("Press Load first.", "Nothing Loaded",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DateTime startDay = loadedStart;
            DateTime endDay = loadedEnd;

            if (offlineData)
            {
                MessageBox.Show(
                    "The Excel export needs a connection - item details for synced sales are only " +
                    "stored on the server.\n\nThe printed report above still works offline; export " +
                    "once you're back online.",
                    "Offline", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var save = new SaveFileDialog
            {
                Title = "Save Sales Report for Excel",
                Filter = "CSV file (*.csv)|*.csv",
                FileName = startDay == endDay
                    ? $"SKC-Sales-{branchName}-{startDay:yyyy-MM-dd}.csv"
                    : $"SKC-Sales-{branchName}-{startDay:yyyy-MM-dd}_to_{endDay:yyyy-MM-dd}.csv"
            };
            if (save.ShowDialog() != DialogResult.OK) return;

            btnExportCsv.Enabled = false;
            try
            {
                var lines = await BranchApiClient.GetBranchSaleLinesRangeAsync(
                    branchName, startDay, endDay.AddDays(1).AddSeconds(-1));

                if (lines.Count == 0)
                {
                    MessageBox.Show("There are no sales in this date range to export.", "Nothing to Export",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                WriteCsv(save.FileName, lines);

                MessageBox.Show(
                    $"Saved {lines.Count} line(s) to:\n{save.FileName}\n\n" +
                    "One row per item per sale - use SUMIFS on the Item or SKU column for a per-item total.",
                    "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not save the export.\n\n{ex.Message}", "Export Failed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnExportCsv.Enabled = true;
            }
        }

        private static void WriteCsv(string path, List<BranchSaleLineExport> lines)
        {
            var sb = new StringBuilder();
            sb.AppendLine("SaleNo,Date,Time,Cashier,SKU,Item,Qty,UnitPrice,LineTotal,ShortfallQty,Voided");

            foreach (var l in lines)
            {
                sb.Append(l.SaleNo).Append(',')
                  .Append(l.SoldAt.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)).Append(',')
                  .Append(l.SoldAt.ToString("HH:mm:ss", CultureInfo.InvariantCulture)).Append(',')
                  .Append(Escape(l.StaffName)).Append(',')
                  .Append(Escape(l.SKU ?? "")).Append(',')
                  .Append(Escape(l.Description)).Append(',')
                  .Append(l.Qty.ToString(CultureInfo.InvariantCulture)).Append(',')
                  // Invariant, no thousands separator - a "1,234.00" would split into two columns.
                  .Append(l.UnitPrice.ToString("0.00", CultureInfo.InvariantCulture)).Append(',')
                  .Append(l.LineTotal.ToString("0.00", CultureInfo.InvariantCulture)).Append(',')
                  .Append(l.ShortfallQty.ToString(CultureInfo.InvariantCulture)).Append(',')
                  .Append(l.Voided ? "TRUE" : "FALSE")
                  .AppendLine();
            }

            // UTF-8 *with* BOM: without it Excel opens the file as ANSI and mangles any non-ASCII
            // character in a product name or cashier name.
            File.WriteAllText(path, sb.ToString(), new UTF8Encoding(true));
        }

        // Quotes a field only when it needs it, doubling any embedded quote - product descriptions
        // routinely contain commas ("Flour, All-Purpose"), which would otherwise shift every
        // following column.
        private static string Escape(string value)
        {
            if (string.IsNullOrEmpty(value)) return "";
            bool needsQuoting = value.IndexOfAny(new[] { ',', '"', '\r', '\n' }) >= 0;
            return needsQuoting ? $"\"{value.Replace("\"", "\"\"")}\"" : value;
        }

        // ---- view models ----

        private class ReportRow
        {
            public string No { get; set; } = string.Empty;
            public DateTime SoldAt { get; set; }
            public string Cashier { get; set; } = string.Empty;
            public decimal Total { get; set; }
            public string Flag { get; set; } = string.Empty;
            public bool Counted { get; set; }
        }

        private class Summary
        {
            public int CountedSales { get; set; }
            public decimal GrossTotal { get; set; }
            public int VoidedCount { get; set; }
            public decimal VoidedAmount { get; set; }
            public int ShortfallCount { get; set; }
            public int RejectedCount { get; set; }
            public int UnsyncedCount { get; set; }
        }

        private class SaleDisplay
        {
            [DisplayName("No.")]
            public string No { get; set; } = string.Empty;

            [DisplayName("Date / Time")]
            public string Time { get; set; } = string.Empty;

            [DisplayName("Cashier")]
            public string Cashier { get; set; } = string.Empty;

            [DisplayName("Total")]
            public decimal Total { get; set; }

            [DisplayName("Flag")]
            public string Flag { get; set; } = string.Empty;
        }
    }
}
