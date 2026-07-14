using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SKC_Bakery_Supplies
{
    public partial class frmPurchasesReport : Form
    {
        // Needed to translate a ticket line's SKU back into a readable description when reprinting.
        private List<BakeryProduct> masterCatalog;
        private PrintDocument pDoc = new PrintDocument();

        // Holds the selected ticket's data for the print renderer callback (mirrors ViewDeliveries.cs's reprint fields).
        private PurchaseTicketSummary printSummary;
        private List<PurchaseLog> printDetails;

        public frmPurchasesReport()
        {
            InitializeComponent();
        }

        private async void frmPurchasesReport_Load(object sender, EventArgs e)
        {
            try
            {
                masterCatalog = await CentralApiClient.GetAllProductsAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
                masterCatalog = new List<BakeryProduct>();
            }

            // Default range: 1st of the current month to today
            dtpStart.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtpEnd.Value = DateTime.Now.Date;

            LoadTickets();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            LoadTickets();
        }

        private async void LoadTickets()
        {
            // Fetch tickets from the engine and bind to Grid 1
            List<PurchaseTicketSummary> tickets = await CentralApiClient.GetPurchaseTicketsAsync(dtpStart.Value.Date, dtpEnd.Value.Date);
            dgvTickets.DataSource = tickets;

            // Clear Grid 2 until a new selection is made
            dgvDetails.DataSource = null;
            dgvTickets.ClearSelection(); // remove selection so when user clicks, it triggers "changed"
        }

        // When the user clicks a row in Grid 1, update Grid 2
        private async void dgvTickets_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvTickets.CurrentRow != null && dgvTickets.CurrentRow.DataBoundItem is PurchaseTicketSummary selectedTicket)
            {
                // Fetch the exact breakdown for this specific ticket
                List<PurchaseLog> details = await CentralApiClient.GetPurchaseDetailsAsync(selectedTicket.TransactionId);
                dgvDetails.DataSource = details;

                // Hide system IDs from the user's view in Grid 2
                if (dgvDetails.Columns["Id"] != null) dgvDetails.Columns["Id"].Visible = false;
                if (dgvDetails.Columns["TransactionId"] != null) dgvDetails.Columns["TransactionId"].Visible = false;
            }
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvTickets.CurrentRow == null) return;

            var selectedTicket = (PurchaseTicketSummary)dgvTickets.CurrentRow.DataBoundItem;

            if (MessageBox.Show("Delete this ticket?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    await CentralApiClient.DeletePurchaseTicketAsync(selectedTicket.TransactionId);
                    LoadTickets();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error");
                }
            }
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            LoadTickets();
        }

        private async void btnPrint_Click(object sender, EventArgs e)
        {
            if (dgvTickets.CurrentRow == null) return;

            printSummary = (PurchaseTicketSummary)dgvTickets.CurrentRow.DataBoundItem;

            try
            {
                printDetails = await CentralApiClient.GetPurchaseDetailsAsync(printSummary.TransactionId);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
                return;
            }

            pDoc.PrintPage -= RenderPurchaseSlip;
            pDoc.PrintPage += RenderPurchaseSlip;

            // Same "preview + explicit Print Report button" pattern as ViewDeliveries.cs's reprint flow.
            Form previewForm = new Form { Text = "Purchase Receipt Preview", Width = 800, Height = 1000, ShowIcon = false };
            Button btnPrintReport = new Button { Text = "Print Report", Dock = DockStyle.Top, Height = 45, Cursor = Cursors.Hand };
            btnPrintReport.Click += (s, ev) =>
            {
                PrintDialog pd = new PrintDialog { Document = pDoc, UseEXDialog = true };
                if (pd.ShowDialog() == DialogResult.OK)
                {
                    try { pDoc.Print(); }
                    catch (System.ComponentModel.Win32Exception) { /* user closed the native print dialog */ }
                    catch (Exception ex) { MessageBox.Show($"Print failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
            };

            PrintPreviewControl ppc = new PrintPreviewControl { Dock = DockStyle.Fill, Document = pDoc };
            previewForm.Controls.Add(btnPrintReport);
            previewForm.Controls.Add(ppc);
            previewForm.ShowDialog();

            pDoc.PrintPage -= RenderPurchaseSlip;
        }

        // --- THE DOCUMENT RENDERER (reprint version of Purchases.cs's RenderPurchaseSlip) ---
        private void RenderPurchaseSlip(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            Font titleFont = new Font("Courier New", 18, FontStyle.Bold);
            Font headerFont = new Font("Courier New", 12, FontStyle.Bold);
            Font regularFont = new Font("Courier New", 11);
            Brush brush = Brushes.Black;

            int y = 50;
            int margin = 50;
            int rightEdge = e.PageBounds.Width - 50;

            g.DrawString("SKC BAKERY SUPPLIES", titleFont, brush, margin, y);
            y += 30;
            g.DrawString("PURCHASE RECEIPT (REPRINT)", headerFont, brush, margin, y);
            y += 40;

            g.DrawString($"SUPPLIER:  {printSummary.Supplier.ToUpper()}", headerFont, brush, margin, y);
            g.DrawString($"DATE:      {printSummary.Date:yyyy-MM-dd}", headerFont, brush, margin + 400, y);
            y += 20;
            g.DrawString($"TICKET:    {printSummary.TransactionId}", regularFont, brush, margin, y);
            y += 40;

            g.DrawLine(Pens.Black, margin, y, rightEdge, y);
            y += 10;
            g.DrawString("QTY", headerFont, brush, margin, y);
            g.DrawString("ITEM DESCRIPTION", headerFont, brush, margin + 60, y);
            g.DrawString("UNIT COST", headerFont, brush, rightEdge - 200, y);
            g.DrawString("TOTAL", headerFont, brush, rightEdge - 80, y);
            y += 25;
            g.DrawLine(Pens.Black, margin, y, rightEdge, y);
            y += 15;

            decimal grandTotal = 0;
            foreach (var item in printDetails)
            {
                var product = masterCatalog.FirstOrDefault(p => p.SKU == item.SKU);
                string description = product != null ? $"{product.Brand} {product.BaseName}" : item.SKU;
                decimal lineTotal = item.Qty * item.UnitCost;
                grandTotal += lineTotal;

                g.DrawString(item.Qty.ToString(), regularFont, brush, margin, y);
                g.DrawString(description, regularFont, brush, margin + 60, y);
                g.DrawString(item.UnitCost.ToString("N2"), regularFont, brush, rightEdge - 200, y);
                g.DrawString(lineTotal.ToString("N2"), regularFont, brush, rightEdge - 80, y);
                y += 25;

                if (y > e.MarginBounds.Bottom - 100)
                {
                    e.HasMorePages = true;
                    return;
                }
            }

            g.DrawLine(Pens.Black, margin, y, rightEdge, y);
            y += 10;
            g.DrawString("GRAND TOTAL:", headerFont, brush, rightEdge - 200, y);
            g.DrawString(grandTotal.ToString("N2"), headerFont, brush, rightEdge - 80, y);
            y += 40;

            g.DrawString("Prepared By: ___________________", regularFont, brush, margin, y);

            e.HasMorePages = false;
        }
    }
}
