using SKC_BakerySupplies;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;

namespace SKC_Bakery_Supplies
{
    public partial class frmDelivery : Form
    {
        private BindingList<DraftDeliveryItem> draftItems = new BindingList<DraftDeliveryItem>();
        private List<BakeryProduct> masterCatalog;
        private bool isSelecting = false;
        private string currentTransactionId;
        private List<DeliveryLog> completedLogsForPrint;
        private int printDeliveryIndex = 0;

        public frmDelivery()
        {
            InitializeComponent();
            dgvDeliveryItems.DataSource = draftItems;
        }

        private async void frmDelivery_Load(object sender, EventArgs e)
        {
            masterCatalog = await CentralDataClient.GetAllProductsAsync();
            txtProductSearch.AutoCompleteMode = AutoCompleteMode.None;
            // Load Requesters
            cmbRequester.Items.AddRange(new string[] { "Kaesseah P", "Gena-flor G.", "Allan A.", "Armando V.", "James M.", "Marites C.", "Anilien B.", "Allan V.",
            "Darlin V.","Julieta B.","Charmaine L","Gina M.","Hazel S.","Jessie M.","Mark C.","Anita E.","Angelie P.","Flordiliza G.","Anna S.","Razel G.",
            "Nino G.","Anna T.","Kim M.","JV M.","Jenifer P.","Janeath D."});

            // Load Branches
            cmbBranch.Items.AddRange(new string[] { "Yoho", "Gaisano", "Liloy", "Labason" });
            if (cmbBranch.Items.Count > 0) cmbBranch.SelectedIndex = 0;
        }

        private void txtProductSearch_TextChanged(object sender, EventArgs e)
        {
            if (isSelecting) return;
            string search = txtProductSearch.Text.Trim().ToLower();

            if (string.IsNullOrWhiteSpace(search))
            {
                lstSearch.Visible = false;
                return;
            }

            var matches = masterCatalog.Where(p =>
                (p.BaseName != null && p.BaseName.ToLower().Contains(search)) ||
                (p.Brand != null && p.Brand.ToLower().Contains(search)) ||
                (p.SKU != null && p.SKU.ToLower().Contains(search))
            ).ToList();

            if (matches.Any())
            {
                lstSearch.DataSource = matches;
                lstSearch.DisplayMember = "SearchDisplay";
                lstSearch.Visible = true;
                lstSearch.BringToFront();
            }
            else
            {
                lstSearch.Visible = false;
            }
        }

        private void CommitSearchSelection()
        {
            if (lstSearch.Items.Count == 0) return;
            if (lstSearch.SelectedIndex == -1) lstSearch.SelectedIndex = 0;

            if (lstSearch.SelectedItem is BakeryProduct selectedItem)
            {
                isSelecting = true;
                txtProductSearch.Text = selectedItem.SearchDisplay;
                isSelecting = false;

                lstSearch.Visible = false;
                numQty.Focus();
            }
        }

        private void lstSearch_Click(object sender, EventArgs e) => CommitSearchSelection();

        private void txtProductSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (!lstSearch.Visible) return;

            if (e.KeyCode == Keys.Down)
            {
                if (lstSearch.SelectedIndex < lstSearch.Items.Count - 1) lstSearch.SelectedIndex++;
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Up)
            {
                if (lstSearch.SelectedIndex > 0) lstSearch.SelectedIndex--;
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                CommitSearchSelection();
            }
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtProductSearch.Text) || numQty.Value <= 0) return;

            string input = txtProductSearch.Text.Trim();
            BakeryProduct item = masterCatalog.FirstOrDefault(p =>
                p.SearchDisplay.Equals(input, StringComparison.OrdinalIgnoreCase));

            if (item == null)
            {
                MessageBox.Show("Item not found in catalog.", "Unknown Item");
                return;
            }
             
            // Zero inventory checks. Instantly adds to draft.
            draftItems.Add(new DraftDeliveryItem
            {
                SKU = item.SKU,
                Brand = item.Brand,
                ItemName = item.BaseName,
                Qty = (int)numQty.Value
            });

            txtProductSearch.Clear();
            numQty.Value = 1;
            txtProductSearch.Focus();
        }

        private void btnSubmitDelivery_Click(object sender, EventArgs e)
        {
            if (draftItems.Count == 0) return;
            if (string.IsNullOrWhiteSpace(cmbBranch.Text))
            {
                MessageBox.Show("Please select a destination branch.");
                return;
            }
            if (string.IsNullOrWhiteSpace(cmbRequester.Text))
            {
                MessageBox.Show("Please select a requester.");
                return;
            }

            string requesterName = cmbRequester.Text;
            string reasonText = txtReason.Text.Trim();

            currentTransactionId = $"DEL-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper()}";
            string dateStr = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string targetBranch = cmbBranch.Text;

            List<DeliveryLog> finalLogs = new List<DeliveryLog>();

            foreach (var draft in draftItems)
            {
                finalLogs.Add(new DeliveryLog
                {
                    TransactionId = currentTransactionId,
                    Date = dateStr,
                    SKU = draft.SKU,
                    Qty = draft.Qty,
                    ToBranch = targetBranch,
                    Requester = requesterName,
                    Reason = reasonText
                });
            }

            // Replace the existing BakeryDatabaseManager.AddDeliveryBulk(finalLogs); and print logic with this:
            try
            {
                completedLogsForPrint = BakeryDatabaseManager.AddDeliveryBulk(finalLogs);

                // --- THE PRINT ENGINE TRIGGER ---
                PrintDocument pDoc = new PrintDocument();

                // Force A4 size (8.27" x 11.69") so the old PC doesn't squish it
                pDoc.DefaultPageSettings.PaperSize = new PaperSize("A4", 827, 1169);
                pDoc.PrintPage += RenderDeliverySlip;

                // Revert to the standard WinForms built-in preview dialog
                PrintPreviewDialog previewDialog = new PrintPreviewDialog
                {
                    Document = pDoc,
                    Width = 800,
                    Height = 1000,
                    ShowIcon = false,
                    Text = "Delivery Sheet Preview"
                };

                printDeliveryIndex = 0;
                previewDialog.ShowDialog();

                // 3. Cleanup & Reset
                pDoc.PrintPage -= RenderDeliverySlip;
                draftItems.Clear();
                cmbBranch.SelectedIndex = 0;
                currentTransactionId = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Delivery Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
            int rightEdge = e.PageBounds.Width - 50;

            g.DrawString("SKC BAKERY SUPPLIES", titleFont, brush, margin, y);
            y += 30;
            g.DrawString("INTERNAL DELIVERY SHEET", headerFont, brush, margin, y);
            y += 40;

            // Header Details
            g.DrawString($"BRANCH:    {cmbBranch.Text.ToUpper()}", headerFont, brush, margin, y);
            g.DrawString($"REQUESTER: {cmbRequester.Text.ToUpper()}", headerFont, brush, margin + 400, y);
            y += 20;
            g.DrawString($"TICKET:    {currentTransactionId}", regularFont, brush, margin, y);
            g.DrawString($"REASON:    {txtReason.Text.ToUpper()}", regularFont, brush, margin + 400, y);
            y += 20;
            g.DrawString($"DATE:      {DateTime.Now:yyyy-MM-dd HH:mm}", regularFont, brush, margin, y);
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

            // Render Items
            double grandTotal = 0;
            foreach (var item in completedLogsForPrint)
            {
                var product = masterCatalog.FirstOrDefault(p => p.SKU == item.SKU);
                string description = product != null ? $"{product.Brand} {product.BaseName}" : item.SKU;
                double unitCost = item.Qty > 0 ? item.TotalLineCost / item.Qty : 0;
                grandTotal += item.TotalLineCost;

                g.DrawString(item.Qty.ToString(), regularFont, brush, margin, y);
                g.DrawString(description, regularFont, brush, margin + 60, y);
                g.DrawString(unitCost.ToString("N2"), regularFont, brush, rightEdge - 200, y);
                g.DrawString(item.TotalLineCost.ToString("N2"), regularFont, brush, rightEdge - 80, y);
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
            g.DrawString("Received By: ___________________", regularFont, brush, rightEdge - 300, y);

            e.HasMorePages = false;
        }

        private void HighlightNumericText(object sender, EventArgs e)
        {
            if (sender is NumericUpDown numericControl)
            {
                // Selects from index 0 to the absolute end of the string
                numericControl.Select(0, numericControl.Text.Length);
            }
        }



    } // end
}