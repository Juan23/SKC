using Microsoft.VisualBasic;
using SKC_BakerySupplies;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SKC_Bakery_Supplies
{
    public partial class frmPurchases : Form
    {
        // Temporary UI state for the DataGridView
        private BindingList<DraftPurchaseItem> draftItems = new BindingList<DraftPurchaseItem>();

        // RAM-based master catalog for instant, lag-free searching
        private List<BakeryProduct> masterCatalog;

        // Stops the dropdown from aggressively reopening when a selection is made
        private bool isSelecting = false;
        private bool isAutoCalculating = false;

        // Holds the just-submitted ticket's line items so the print renderer can read them,
        // and the transaction ID so it survives past the async submit call into the render callback.
        private List<PurchaseLog> completedLogsForPrint;
        private string currentTransactionId;
        private int printPurchaseIndex = 0;

        public frmPurchases()
        {
            InitializeComponent();
            txtTotalCost.TextChanged += CalculateUnitCostFromTotal;
            dgvPurchaseItems.DataSource = draftItems;
            draftItems.ListChanged += (s, e) => UpdateRunningTotal();
        }

        private void frmPurchases_Load(object sender, EventArgs e)
        {
            RefreshCatalog();
            txtProductSearch.AutoCompleteMode = AutoCompleteMode.None; // Kill native autocomplete
        }

        private async void RefreshCatalog()
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
        }

        private void txtProductSearch_TextChanged(object sender, EventArgs e)
        {
            // If the system is currently locking in a choice, ignore the text change
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

            // If nothing is explicitly highlighted, default to the top item
            if (lstSearch.SelectedIndex == -1)
            {
                lstSearch.SelectedIndex = 0;
            }

            if (lstSearch.SelectedItem is BakeryProduct selectedItem)
            {
                isSelecting = true; // Lock the search engine
                txtProductSearch.Text = selectedItem.SearchDisplay;
                isSelecting = false; // Unlock it

                lstSearch.Visible = false;
                UpdatePackToggle(selectedItem);
                numQty.Focus();
            }
        }

        // Shows the "buy by pack" toggle only for products with a purchase-unit conversion
        // set up (see SKC Admin's classification screen). Unset/1.0 multiplier products
        // behave exactly as before - qty entered is already the base unit.
        private void UpdatePackToggle(BakeryProduct product)
        {
            if (product.PackMultiplier > 1)
            {
                chkByPack.Text = $"Buy by pack ({(string.IsNullOrWhiteSpace(product.Uom) ? "pack" : product.Uom)} = {product.PackMultiplier:0.####} base units)";
                chkByPack.Visible = true;
                chkByPack.Checked = false;
            }
            else
            {
                chkByPack.Visible = false;
                chkByPack.Checked = false;
            }
        }

        private void lstSearch_Click(object sender, EventArgs e)
        {
            CommitSearchSelection();
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtProductSearch.Text) || numQty.Value <= 0) return;

            string input = txtProductSearch.Text.Trim();

            // Look for the exact matching SearchDisplay string in RAM
            BakeryProduct item = masterCatalog.FirstOrDefault(p =>
                p.SearchDisplay.Equals(input, StringComparison.OrdinalIgnoreCase));

            // The Logic Fork (Intercept if missing)
            if (item == null)
            {
                DialogResult dialogResult = MessageBox.Show($"Item '{input}' not found in catalog. Add it now?", "Unknown Item", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (dialogResult == DialogResult.Yes)
                {
                    using (var addForm = new frmAddMasterItem(input)) // Passes typed text to BaseName
                    {
                        if (addForm.ShowDialog() == DialogResult.OK)
                        {
                            item = addForm.NewProduct;
                            RefreshCatalog(); // Update RAM with the new item so it can be searched
                        }
                        else
                        {
                            return;
                        }
                    }
                }
                else
                {
                    return;
                }
            }

            // "Buy by pack": numQty/numUnitCost are entered as pack count / cost-per-pack
            // (e.g. "2 sacks at 500 each"); convert to base units (grams) before storing,
            // since inventory_lots and recipes always work in base units. The displayed
            // running total is unaffected either way - it's pack-count * pack-cost either way.
            int qty;
            decimal unitCost;
            if (chkByPack.Visible && chkByPack.Checked && item.PackMultiplier > 0)
            {
                qty = (int)(numQty.Value * item.PackMultiplier);
                unitCost = numUnitCost.Value / item.PackMultiplier;
            }
            else
            {
                qty = (int)numQty.Value;
                unitCost = numUnitCost.Value;
            }

            // Add to the Draft Grid
            draftItems.Add(new DraftPurchaseItem
            {
                SKU = item.SKU,
                Brand = item.Brand,
                ItemName = item.BaseName,
                Qty = qty,
                UnitCost = unitCost
            });

            // Reset entry bar
            txtProductSearch.Clear();
            numQty.Value = 1;
            numUnitCost.Value = 0;
            txtTotalCost.Text = "0.00";
            chkByPack.Visible = false;
            chkByPack.Checked = false;
            txtProductSearch.Focus();
        }

        private async void btnSubmitPurchase_Click(object sender, EventArgs e)
        {
            if (draftItems.Count == 0) return;
            if (string.IsNullOrWhiteSpace(txtSupplier.Text))
            {
                MessageBox.Show("Please enter a Supplier.");
                return;
            }

            List<PurchaseLog> finalLogs = new List<PurchaseLog>();
            DateTime entryDate = dtpDate.Value.Date;
            string supplier = ToProperCase(txtSupplier.Text);

            // Auto transaction ID
            currentTransactionId = $"PUR-{entryDate:yyyyMMdd}-{Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper()}";

            foreach (var draft in draftItems)
            {
                finalLogs.Add(new PurchaseLog
                {
                    TransactionId = currentTransactionId,
                    Date = entryDate,
                    Supplier = supplier,
                    SKU = draft.SKU,
                    Qty = draft.Qty,
                    UnitCost = draft.UnitCost
                });
            }

            try
            {
                await CentralApiClient.SubmitPurchasesAsync(finalLogs);

                // Unlike deliveries, a purchase line is never split across lots server-side,
                // so what we sent is exactly what to print - no need to read back a server response.
                completedLogsForPrint = finalLogs;

                // --- THE PRINT ENGINE TRIGGER (mirrors Delivery.cs's slip printing) ---
                PrintDocument pDoc = new PrintDocument();
                pDoc.DefaultPageSettings.PaperSize = new PaperSize("A4", 827, 1169);
                pDoc.PrintPage += RenderPurchaseSlip;

                PrintPreviewDialog previewDialog = new PrintPreviewDialog
                {
                    Document = pDoc,
                    Width = 800,
                    Height = 1000,
                    ShowIcon = false,
                    Text = "Purchase Receipt Preview"
                };
                printPurchaseIndex = 0;
                previewDialog.ShowDialog();
                pDoc.PrintPage -= RenderPurchaseSlip;

                // Wipe the board clean only after the receipt has been previewed/printed
                draftItems.Clear();
                txtSupplier.Clear();
                currentTransactionId = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Purchase Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- THE DOCUMENT RENDERER (mirrors Delivery.cs's RenderDeliverySlip) ---
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
            g.DrawString("PURCHASE RECEIPT", headerFont, brush, margin, y);
            y += 40;

            // Header Details
            g.DrawString($"SUPPLIER:  {txtSupplier.Text.ToUpper()}", headerFont, brush, margin, y);
            g.DrawString($"DATE:      {dtpDate.Value:yyyy-MM-dd}", headerFont, brush, margin + 400, y);
            y += 20;
            g.DrawString($"TICKET:    {currentTransactionId}", regularFont, brush, margin, y);
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

            // Render Items (printPurchaseIndex persists across HasMorePages callbacks so each page
            // resumes where the last one left off instead of restarting from the first item)
            // Computed over the full list up front (not accumulated per-page) so the footer on the
            // final page always shows the true document total, not just the last page's subtotal.
            decimal grandTotal = completedLogsForPrint.Sum(i => i.Qty * i.UnitCost);
            while (printPurchaseIndex < completedLogsForPrint.Count)
            {
                var item = completedLogsForPrint[printPurchaseIndex];
                var product = masterCatalog.FirstOrDefault(p => p.SKU == item.SKU);
                string description = product != null ? $"{product.Brand} {product.BaseName}" : item.SKU;
                decimal lineTotal = item.Qty * item.UnitCost;

                g.DrawString(item.Qty.ToString(), regularFont, brush, margin, y);
                g.DrawString(description, regularFont, brush, margin + 60, y);
                g.DrawString(item.UnitCost.ToString("N2"), regularFont, brush, rightEdge - 200, y);
                g.DrawString(lineTotal.ToString("N2"), regularFont, brush, rightEdge - 80, y);
                y += 25;

                printPurchaseIndex++;

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

            // No "Received By" line here (unlike the delivery slip) - a purchase receipt is
            // filed internally alongside the vendor's own paper receipt, nobody signs for it.
            g.DrawString("Prepared By: ___________________", regularFont, brush, margin, y);

            e.HasMorePages = false;
        }

        // Keyboard catcher
        private void txtProductSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (!lstSearch.Visible) return;

            // Arrow Down: Navigate the list
            if (e.KeyCode == Keys.Down)
            {
                if (lstSearch.SelectedIndex < lstSearch.Items.Count - 1)
                    lstSearch.SelectedIndex++;
                e.Handled = true;
            }
            // Arrow Up: Navigate the list
            else if (e.KeyCode == Keys.Up)
            {
                if (lstSearch.SelectedIndex > 0)
                    lstSearch.SelectedIndex--;
                e.Handled = true;
            }
            // Enter: Lock in the choice
            else if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // Silences the Windows error "ding" sound
                CommitSearchSelection();
            }
        }

        private string ToProperCase(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return text;
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text.ToLower().Trim());
        }

        private void UpdateRunningTotal()
        {
            decimal total = draftItems.Sum(item => item.Qty * item.UnitCost);
            lblRunningTotal.Text = $"Total: {total:N2}";
        }

        private void HighlightNumericText(object sender, EventArgs e)
        {
            if (sender is NumericUpDown numericControl)
            {
                numericControl.Select(0, numericControl.Text.Length);
            }
        }

        private void CalculateTotalCost(object sender, EventArgs e)
        {
            if (isAutoCalculating) return;
            isAutoCalculating = true;

            decimal.TryParse(numQty.Text, out decimal quantity);
            decimal.TryParse(numUnitCost.Text, out decimal unitCost);

            txtTotalCost.Text = (quantity * unitCost).ToString("0.00");

            isAutoCalculating = false;
        }

        private void CalculateUnitCostFromTotal(object sender, EventArgs e)
        {
            if (isAutoCalculating) return;
            isAutoCalculating = true;

            decimal.TryParse(numQty.Text, out decimal quantity);
            decimal.TryParse(txtTotalCost.Text, out decimal totalCost);

            if (quantity > 0)
            {
                numUnitCost.Value = totalCost / quantity;
            }

            isAutoCalculating = false;
        }
    }
}