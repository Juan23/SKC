using Microsoft.VisualBasic;
using SKC_BakerySupplies;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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

        public frmPurchases()
        {
            InitializeComponent();
            dgvPurchaseItems.DataSource = draftItems;
            draftItems.ListChanged += (s, e) => UpdateRunningTotal();
        }

        private void frmPurchases_Load(object sender, EventArgs e)
        {
            RefreshCatalog();
            txtProductSearch.AutoCompleteMode = AutoCompleteMode.None; // Kill native autocomplete
        }

        private void RefreshCatalog()
        {
            masterCatalog = BakeryDatabaseManager.GetAllProducts();
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
                numQty.Focus();
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

            // Add to the Draft Grid
            draftItems.Add(new DraftPurchaseItem
            {
                SKU = item.SKU,
                Brand = item.Brand,
                ItemName = item.BaseName,
                Qty = (int)numQty.Value,
                UnitCost = numUnitCost.Value
            });

            // Reset entry bar
            txtProductSearch.Clear();
            numQty.Value = 1;
            numUnitCost.Value = 0;
            txtProductSearch.Focus();
        }

        private void btnSubmitPurchase_Click(object sender, EventArgs e)
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
            string transactionId = $"PUR-{entryDate:yyyyMMdd}-{Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper()}";

            foreach (var draft in draftItems)
            {
                finalLogs.Add(new PurchaseLog
                {
                    TransactionId = transactionId,
                    Date = entryDate,
                    Supplier = supplier,
                    SKU = draft.SKU,
                    Qty = draft.Qty,
                    UnitCost = draft.UnitCost
                });
            }

            // Fire to the SQLite Ledger
            BakeryDatabaseManager.AddPurchasesBulk(finalLogs);

            MessageBox.Show($"Purchase Sheet committed safely.\nTicket ID: {transactionId}", "Success");

            // Wipe the board clean
            draftItems.Clear();
            txtSupplier.Clear();
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
            // 1. Set default values
            decimal quantity = 0;
            decimal unitCost = 0;

            // 2. Safely parse the RAW TEXT instead of the .Value property. 
            // If the box is empty or has an invalid character, it safely falls back to 0 without crashing.
            decimal.TryParse(numQty.Text, out quantity);
            decimal.TryParse(numUnitCost.Text, out unitCost);

            // 3. Execute the math and update the locked textbox
            decimal total = quantity * unitCost;
            txtTotalCost.Text = $"{total:N2}";
        }
    }
}