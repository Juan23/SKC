using SKC_BakerySupplies;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;

namespace SKC_Bakery_Supplies
{
    public partial class frmViewDeliveries : Form
    {
        private List<BakeryProduct> masterCatalog;
        private PrintDocument pDoc = new PrintDocument();

        // Temporary holding variables for the print engine
        private DeliveryTicketSummary printSummary;
        private List<DeliveryLog> printDetails;

        private List<DailyDeliveryPrintItem> printDailyDetails;
        private DateTime printDailyDate;
        private PrintDocument pDocDaily = new PrintDocument();

        public frmViewDeliveries()
        {
            InitializeComponent();
        }

        private void frmViewDeliveries_Load(object sender, EventArgs e)
        {
            masterCatalog = BakeryDatabaseManager.GetAllProducts();

            // Default to viewing the last 7 days
            dtpStart.Value = DateTime.Today.AddDays(-7);
            dtpEnd.Value = DateTime.Today;
            LoadGrid();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            LoadGrid();
        }

        private void LoadGrid()
        {
            var tickets = BakeryDatabaseManager.GetDeliveryTickets(dtpStart.Value, dtpEnd.Value);
            dgvTickets.DataSource = tickets;
            dgvTickets.ClearSelection();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvTickets.CurrentRow == null) return;

            var selected = (DeliveryTicketSummary)dgvTickets.CurrentRow.DataBoundItem;

            DialogResult confirm = MessageBox.Show(
                $"Are you sure you want to completely delete delivery ticket {selected.TransactionId}?\n\nThis cannot be undone.",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm == DialogResult.Yes)
            {
                BakeryDatabaseManager.DeleteDeliveryTicket(selected.TransactionId);
                LoadGrid();
            }
        }
        private void dgvTickets_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            printSummary = (DeliveryTicketSummary)dgvTickets.Rows[e.RowIndex].DataBoundItem;
            printDetails = BakeryDatabaseManager.GetDeliveryDetails(printSummary.TransactionId);

            pDoc.PrintPage -= RenderDeliverySlip;
            pDoc.PrintPage += RenderDeliverySlip;

            // 1. Create a clean, silent preview window
            // 1. Create a clean, silent preview window
            Form previewForm = new Form { Text = "Delivery Sheet Preview", Width = 800, Height = 1000, ShowIcon = false };

            // 2. Create our own Print Button (Moved to Top)
            Button btnPrint = new Button { Text = "Print Report", Dock = DockStyle.Top, Height = 45, Cursor = Cursors.Hand };
            btnPrint.Click += (s, ev) =>
            {
                PrintDialog pd = new PrintDialog { Document = pDoc, UseEXDialog = true };
                if (pd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        pDoc.Print();
                    }
                    catch (System.ComponentModel.Win32Exception)
                    {
                        // stop win32 error when closing save dialog
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Print failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            };

            PrintPreviewControl ppc = new PrintPreviewControl { Dock = DockStyle.Fill, Document = pDoc };

            // 3. Assemble and show (Button added FIRST so it claims the top space)
            previewForm.Controls.Add(btnPrint);
            previewForm.Controls.Add(ppc);

            previewForm.ShowDialog();

            // 4. Cleanup
            pDoc.PrintPage -= RenderDeliverySlip;
        }

        // --- THE DOCUMENT RENDERER ---
        private void RenderDeliverySlip(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            Font titleFont = new Font("Courier New", 18, FontStyle.Bold);
            Font headerFont = new Font("Courier New", 12, FontStyle.Bold);
            Font regularFont = new Font("Courier New", 11);
            Brush brush = Brushes.Black;

            int y = 50;
            int margin = 50;
            int rightEdge = e.PageBounds.Width - 50; // Dynamically finds the right side of the paper

            g.DrawString("SKC BAKERY SUPPLIES", titleFont, brush, margin, y);
            y += 30;
            g.DrawString("INTERNAL DELIVERY SHEET (REPRINT)", headerFont, brush, margin, y);
            y += 40;

            g.DrawString($"BRANCH:    {printSummary.ToBranch.ToUpper()}", headerFont, brush, margin, y);
            g.DrawString($"REQUESTER: {printSummary.Requester?.ToUpper()}", headerFont, brush, margin + 400, y);
            y += 20;
            g.DrawString($"TICKET:    {printSummary.TransactionId}", regularFont, brush, margin, y);
            g.DrawString($"REASON:    {printSummary.Reason?.ToUpper()}", regularFont, brush, margin + 400, y);
            y += 20;
            g.DrawString($"DATE:      {printSummary.Date}", regularFont, brush, margin, y);
            y += 40;

            // Table Headers
            g.DrawLine(Pens.Black, margin, y, rightEdge, y);
            y += 10;
            g.DrawString("QTY", headerFont, brush, margin, y);
            g.DrawString("ITEM DESCRIPTION", headerFont, brush, margin + 60, y);
            g.DrawString("UNIT COST", headerFont, brush, rightEdge - 200, y);
            g.DrawString("TOTAL", headerFont, brush, rightEdge - 80, y);
            y += 25;
            g.DrawLine(Pens.Black, margin, y, rightEdge, y);
            y += 15;

            // Render Items Row by Row
            foreach (var item in printDetails)
            {
                // Translate the database SKU back into a readable name
                string description = item.SKU;
                var product = masterCatalog.FirstOrDefault(p => p.SKU == item.SKU);
                if (product != null)
                {
                    description = $"{product.Brand} {product.BaseName}";
                }

                double unitCost = item.Qty > 0 ? item.TotalLineCost / item.Qty : 0;
                g.DrawString(item.Qty.ToString(), regularFont, brush, margin, y);
                g.DrawString(description, regularFont, brush, margin + 60, y);
                g.DrawString(unitCost.ToString("N2"), regularFont, brush, rightEdge - 200, y);
                g.DrawString(item.TotalLineCost.ToString("N2"), regularFont, brush, rightEdge - 80, y);
                y += 25;

                // Handle page breaks if the list exceeds the paper height
                if (y > e.MarginBounds.Bottom)
                {
                    e.HasMorePages = true;
                    return;
                }
            }


            g.DrawLine(Pens.Black, margin, y, rightEdge, y);
            y += 10;
            g.DrawString("GRAND TOTAL:", headerFont, brush, rightEdge - 200, y);
            g.DrawString(printSummary.TotalCost.ToString("N2"), headerFont, brush, rightEdge - 80, y);
            y += 40;

            g.DrawString("Prepared By: ___________________", regularFont, brush, margin, y);
            g.DrawString("Received By: ___________________", regularFont, brush, rightEdge - 300, y);

            e.HasMorePages = false;
        }

        private void btnPrintDaily_Click(object sender, EventArgs e)
        {
            printDailyDate = dtpStart.Value.Date;
            printDailyDetails = BakeryDatabaseManager.GetDailyDeliveryConsolidation(printDailyDate);

            if (printDailyDetails.Count == 0)
            {
                MessageBox.Show("No deliveries found for " + printDailyDate.ToString("yyyy-MM-dd"));
                return;
            }

            pDocDaily.PrintPage -= RenderDailyReport;
            pDocDaily.PrintPage += RenderDailyReport;

            Form previewForm = new Form { Text = "Daily Summary Preview", Width = 800, Height = 1000, ShowIcon = false };
            Button btnPrint = new Button { Text = "Print Report", Dock = DockStyle.Top, Height = 45, Cursor = Cursors.Hand };
            btnPrint.Click += (s, ev) =>
            {
                PrintDialog pd = new PrintDialog { Document = pDocDaily, UseEXDialog = true };
                if (pd.ShowDialog() == DialogResult.OK)
                {
                    try { pDocDaily.Print(); }
                    catch (Exception ex) { MessageBox.Show($"Print failed: {ex.Message}", "Error"); }
                }
            };

            PrintPreviewControl ppc = new PrintPreviewControl { Dock = DockStyle.Fill, Document = pDocDaily };
            previewForm.Controls.Add(btnPrint);
            previewForm.Controls.Add(ppc);
            previewForm.ShowDialog();
        }

        private void RenderDailyReport(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            Font titleFont = new Font("Courier New", 16, FontStyle.Bold);
            Font headerFont = new Font("Courier New", 12, FontStyle.Bold);
            Font regularFont = new Font("Courier New", 11);
            Font italicFont = new Font("Courier New", 10, FontStyle.Italic);
            Brush brush = Brushes.Black;

            int y = 50;
            int margin = 50;
            int rightEdge = e.PageBounds.Width - 50;

            g.DrawString("SKC BAKERY SUPPLIES", titleFont, brush, margin, y);
            y += 30;
            g.DrawString($"DAILY DELIVERY CONSOLIDATION", headerFont, brush, margin, y);
            y += 25;
            g.DrawString($"DATE: {printDailyDate:yyyy-MM-dd}", regularFont, brush, margin, y);
            y += 40;

            string currentBranch = "";
            string currentTicket = "";
            double dailyGrandTotal = 0;
            double ticketTotal = 0;

            foreach (var item in printDailyDetails)
            {
                // Branch Header
                if (item.ToBranch != currentBranch)
                {
                    currentBranch = item.ToBranch;
                    y += 10;
                    g.DrawLine(Pens.Black, margin, y, rightEdge, y);
                    y += 5;
                    g.DrawString($"BRANCH: {currentBranch.ToUpper()}", headerFont, brush, margin, y);
                    y += 20;
                    g.DrawLine(Pens.Black, margin, y, rightEdge, y);
                    y += 15;
                }

                // Ticket Sub-Header
                if (item.TransactionId != currentTicket)
                {
                    if (currentTicket != "")
                    {
                        // Print total for previous ticket
                        y += 5;
                        g.DrawString($"Ticket Total: {ticketTotal:N2}", headerFont, brush, rightEdge - 220, y);
                        y += 25;
                    }

                    currentTicket = item.TransactionId;
                    ticketTotal = 0;

                    g.DrawString($"TICKET: {currentTicket}", headerFont, brush, margin, y);
                    y += 15;
                    g.DrawString($"Req: {item.Requester} | Rsn: {item.Reason}", italicFont, brush, margin, y);
                    y += 20;

                    g.DrawString("QTY", headerFont, brush, margin, y);
                    g.DrawString("ITEM", headerFont, brush, margin + 50, y);
                    g.DrawString("TOTAL", headerFont, brush, rightEdge - 80, y);
                    y += 20;
                }

                string description = string.IsNullOrWhiteSpace(item.BaseName) ? item.SKU : $"{item.Brand} {item.BaseName}";
                ticketTotal += item.TotalLineCost;
                dailyGrandTotal += item.TotalLineCost;

                g.DrawString(item.Qty.ToString(), regularFont, brush, margin, y);
                g.DrawString(description, regularFont, brush, margin + 50, y);
                g.DrawString(item.TotalLineCost.ToString("N2"), regularFont, brush, rightEdge - 80, y);
                y += 20;

                if (y > e.MarginBounds.Bottom - 80)
                {
                    e.HasMorePages = true;
                    return;
                }
            }

            // Print the final ticket total
            y += 5;
            g.DrawString($"Ticket Total: {ticketTotal:N2}", headerFont, brush, rightEdge - 220, y);
            y += 30;

            // Daily Grand Total
            g.DrawLine(Pens.Black, margin, y, rightEdge, y);
            y += 10;
            g.DrawString("DAILY GRAND TOTAL:", titleFont, brush, margin, y);
            g.DrawString(dailyGrandTotal.ToString("N2"), titleFont, brush, rightEdge - 200, y);

            e.HasMorePages = false;
        }


    }
}