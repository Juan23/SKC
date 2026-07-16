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
        private int printDeliveryIndex = 0;

        private List<DailyDeliveryPrintItem> printDailyDetails;
        private DateTime printDailyDate;
        private PrintDocument pDocDaily = new PrintDocument();
        private int printDailyIndex = 0;
        private string printDailyCurrentBranch = "";
        private string printDailyCurrentTicket = "";
        private double printDailyTicketTotal = 0;
        private double printDailyGrandTotal = 0;

        private Button btnEdit;

        public frmViewDeliveries()
        {
            InitializeComponent();

            // Added in code rather than the Designer so we don't hand-edit ViewDeliveries.Designer.cs.
            btnEdit = new Button
            {
                Location = new Point(8, 480),
                Name = "btnEdit",
                Size = new Size(104, 23),
                Text = "Edit Ticket",
                UseVisualStyleBackColor = true,
                Enabled = false
            };
            btnEdit.Click += btnEdit_Click;
            groupBox1.Controls.Add(btnEdit);

            // Only InTransit tickets can be edited or deleted; once a branch has accepted, the
            // ticket is locked (server rejects both), so disable the buttons to match.
            dgvTickets.SelectionChanged += dgvTickets_SelectionChanged;
        }

        private void dgvTickets_SelectionChanged(object sender, EventArgs e)
        {
            bool isInTransit = dgvTickets.CurrentRow?.DataBoundItem is DeliveryTicketSummary t
                               && t.Status == "InTransit";
            btnEdit.Enabled = isInTransit;
            btnDelete.Enabled = isInTransit;
        }

        private async void frmViewDeliveries_Load(object sender, EventArgs e)
        {
            masterCatalog = await CentralApiClient.GetAllProductsAsync();

            dtpStart.Value = DateTime.Today;
            dtpEnd.Value = DateTime.Today;
            LoadGrid();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            LoadGrid();
        }

        private async void LoadGrid()
        {
            var tickets = await CentralApiClient.GetDeliveryTicketsAsync(dtpStart.Value, dtpEnd.Value);
            dgvTickets.DataSource = tickets;
            dgvTickets.ClearSelection();
        }

        private async void btnDelete_Click(object sender, EventArgs e)
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
                try
                {
                    await CentralApiClient.DeleteDeliveryTicketAsync(selected.TransactionId);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Delete Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                LoadGrid();
            }
        }

        private async void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvTickets.CurrentRow?.DataBoundItem is not DeliveryTicketSummary selected) return;
            if (selected.Status != "InTransit")
            {
                MessageBox.Show("Only deliveries still in transit can be edited.", "Cannot Edit",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            List<DeliveryLog> lines;
            try
            {
                lines = await CentralApiClient.GetDeliveryDetailsAsync(selected.TransactionId);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult confirm = MessageBox.Show(
                $"This will remove ticket {selected.TransactionId} (restocking its items to Office) and reopen it " +
                "in the Create Delivery screen, pre-filled, so you can adjust and re-submit it.\n\n" +
                "The branch will see the corrected ticket as a new delivery to accept. Continue?",
                "Edit Delivery",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            try
            {
                await CentralApiClient.DeleteDeliveryTicketAsync(selected.TransactionId);
            }
            catch (Exception ex)
            {
                // e.g. the branch accepted it a moment ago - abort without opening the editor.
                MessageBox.Show(ex.Message, "Edit Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                LoadGrid();
                return;
            }

            using (var editForm = new frmDelivery(lines))
            {
                editForm.ShowDialog();
            }
            LoadGrid();
        }

        private async void dgvTickets_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            printSummary = (DeliveryTicketSummary)dgvTickets.Rows[e.RowIndex].DataBoundItem;
            printDetails = await CentralApiClient.GetDeliveryDetailsAsync(printSummary.TransactionId);

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

            printDeliveryIndex = 0;
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

            // Render Items Row by Row (printDeliveryIndex persists across HasMorePages callbacks so
            // each page resumes where the last one left off instead of restarting from the first item)
            while (printDeliveryIndex < printDetails.Count)
            {
                var item = printDetails[printDeliveryIndex];

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

                printDeliveryIndex++;

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

        private async void btnPrintDaily_Click(object sender, EventArgs e)
        {
            printDailyDate = dtpStart.Value.Date;
            printDailyDetails = await CentralApiClient.GetDailyDeliveryConsolidationAsync(printDailyDate);

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

            printDailyIndex = 0;
            printDailyCurrentBranch = "";
            printDailyCurrentTicket = "";
            printDailyTicketTotal = 0;
            printDailyGrandTotal = 0;
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

            // printDailyIndex/CurrentBranch/CurrentTicket/TicketTotal/GrandTotal persist across
            // HasMorePages callbacks so each page resumes where the last one left off instead of
            // restarting the whole report from the first item.

            // Resuming on a new page: if we're picking back up mid-branch/mid-ticket, redraw the
            // header context so the continued rows are still identifiable, since the branch/ticket
            // change checks below won't fire again until the underlying values actually change.
            if (printDailyIndex > 0 && printDailyIndex < printDailyDetails.Count)
            {
                var nextItem = printDailyDetails[printDailyIndex];
                bool sameBranch = nextItem.ToBranch == printDailyCurrentBranch;
                bool sameTicket = sameBranch && nextItem.TransactionId == printDailyCurrentTicket;

                if (sameBranch)
                {
                    y += 10;
                    g.DrawLine(Pens.Black, margin, y, rightEdge, y);
                    y += 5;
                    g.DrawString($"BRANCH: {printDailyCurrentBranch.ToUpper()} (cont.)", headerFont, brush, margin, y);
                    y += 20;
                    g.DrawLine(Pens.Black, margin, y, rightEdge, y);
                    y += 15;
                }

                if (sameTicket && printDailyCurrentTicket != "")
                {
                    g.DrawString($"TICKET: {printDailyCurrentTicket} (cont.)", headerFont, brush, margin, y);
                    y += 20;

                    g.DrawString("QTY", headerFont, brush, margin, y);
                    g.DrawString("ITEM", headerFont, brush, margin + 50, y);
                    g.DrawString("TOTAL", headerFont, brush, rightEdge - 80, y);
                    y += 20;
                }
            }

            while (printDailyIndex < printDailyDetails.Count)
            {
                var item = printDailyDetails[printDailyIndex];

                // Branch Header
                if (item.ToBranch != printDailyCurrentBranch)
                {
                    printDailyCurrentBranch = item.ToBranch;
                    y += 10;
                    g.DrawLine(Pens.Black, margin, y, rightEdge, y);
                    y += 5;
                    g.DrawString($"BRANCH: {printDailyCurrentBranch.ToUpper()}", headerFont, brush, margin, y);
                    y += 20;
                    g.DrawLine(Pens.Black, margin, y, rightEdge, y);
                    y += 15;
                }

                // Ticket Sub-Header
                if (item.TransactionId != printDailyCurrentTicket)
                {
                    if (printDailyCurrentTicket != "")
                    {
                        // Print total for previous ticket
                        y += 5;
                        g.DrawString($"Ticket Total: {printDailyTicketTotal:N2}", headerFont, brush, rightEdge - 220, y);
                        y += 25;
                    }

                    printDailyCurrentTicket = item.TransactionId;
                    printDailyTicketTotal = 0;

                    g.DrawString($"TICKET: {printDailyCurrentTicket}", headerFont, brush, margin, y);
                    y += 15;
                    g.DrawString($"Req: {item.Requester} | Rsn: {item.Reason}", italicFont, brush, margin, y);
                    y += 20;

                    g.DrawString("QTY", headerFont, brush, margin, y);
                    g.DrawString("ITEM", headerFont, brush, margin + 50, y);
                    g.DrawString("TOTAL", headerFont, brush, rightEdge - 80, y);
                    y += 20;
                }

                string description = string.IsNullOrWhiteSpace(item.BaseName) ? item.SKU : $"{item.Brand} {item.BaseName}";
                printDailyTicketTotal += item.TotalLineCost;
                printDailyGrandTotal += item.TotalLineCost;

                g.DrawString(item.Qty.ToString(), regularFont, brush, margin, y);
                g.DrawString(description, regularFont, brush, margin + 50, y);
                g.DrawString(item.TotalLineCost.ToString("N2"), regularFont, brush, rightEdge - 80, y);
                y += 20;

                printDailyIndex++;

                if (y > e.MarginBounds.Bottom - 80)
                {
                    e.HasMorePages = true;
                    return;
                }
            }

            // Print the final ticket total
            y += 5;
            g.DrawString($"Ticket Total: {printDailyTicketTotal:N2}", headerFont, brush, rightEdge - 220, y);
            y += 30;

            // Daily Grand Total
            g.DrawLine(Pens.Black, margin, y, rightEdge, y);
            y += 10;
            g.DrawString("DAILY GRAND TOTAL:", titleFont, brush, margin, y);
            g.DrawString(printDailyGrandTotal.ToString("N2"), titleFont, brush, rightEdge - 200, y);

            e.HasMorePages = false;
        }


    }
}