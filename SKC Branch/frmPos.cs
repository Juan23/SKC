using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace SKC_Branch
{
    // Offline-first point of sale. Sales are written to a durable local queue
    // (PosLocalStore) and pushed to the server by PosSyncEngine when online; the
    // catalog/price/stock cache is pulled on every sync. Only priced items
    // (Price > 0) are findable - if baking wasn't recorded, the item shows no
    // stock and the cashier gets a warning, but the sale is never blocked.
    public partial class frmPos : Form
    {
        private readonly string branchName;
        private List<CachedProduct> catalog = new List<CachedProduct>();
        private readonly BindingList<CartLine> cart = new BindingList<CartLine>();
        private bool isSelecting;

        public frmPos(string branchName)
        {
            this.branchName = branchName;
            InitializeComponent();

            var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            Text = $"SKC Branch - {branchName} - POS  (v{version?.ToString(3)})";
            dgvCart.DataSource = cart;

            // Fill mode would split the cart into equal quarters; the item name deserves
            // the lion's share of the 900px grid. Columns exist already - binding a
            // BindingList materializes them synchronously. Null-guarded so a renamed
            // CartLine property degrades to equal widths instead of crashing startup.
            if (dgvCart.Columns["Item"] is { } itemColumn) itemColumn.FillWeight = 300;

            FormClosing += frmPos_FormClosing;
        }

        // Unsynced sales are durable (they retry on next launch), so this isn't a data-loss
        // warning - just a heads-up so a cashier closing at end of day knows sales are still
        // queued rather than assuming everything already reached the office.
        private void frmPos_FormClosing(object? sender, FormClosingEventArgs e)
        {
            int pending = PosLocalStore.PendingCount();
            if (pending == 0) return;

            var proceed = MessageBox.Show(
                $"{pending} sale(s) have not synced to the server yet.\n\n" +
                "They are saved on this computer and will sync automatically once online " +
                "(or press Sync Now first). Close anyway?",
                "Unsynced Sales", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (proceed != DialogResult.Yes) e.Cancel = true;
        }

        private void btnGoDeliveries_Click(object? sender, EventArgs e)
        {
            using var form = new frmMain(branchName);
            form.ShowDialog();
        }

        private void btnGoMyStock_Click(object? sender, EventArgs e)
        {
            using var form = new frmBranchStock(branchName);
            form.ShowDialog();
        }

        private void btnGoProduction_Click(object? sender, EventArgs e)
        {
            using var form = new frmProduction(branchName);
            form.ShowDialog();
        }

        private void btnGoSalesHistory_Click(object? sender, EventArgs e)
        {
            using var form = new frmBranchSalesHistory(branchName);
            form.ShowDialog();
        }

        private async void frmPos_Load(object sender, EventArgs e)
        {
            ReloadCatalogFromCache();

            if (catalog.Count == 0 && !PosLocalStore.HasCatalog())
            {
                lblSyncStatus.Text = "First run - fetching catalog from server...";
            }

            await RunSyncAsync();

            if (catalog.Count == 0 && !PosLocalStore.HasCatalog())
            {
                MessageBox.Show(
                    "The POS needs one online sync to fetch the catalog before it can sell.\n" +
                    "Connect to the network and press Sync Now.",
                    "No Catalog Yet", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            syncTimer.Start();
            txtStaff.Focus();
        }

        private void ReloadCatalogFromCache()
        {
            catalog = PosLocalStore.GetSellableCatalog();
        }

        // ---- item search (same floating-list pattern as the office app's Delivery screen) ----

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (isSelecting) return;
            string search = txtSearch.Text.Trim().ToLower();

            if (string.IsNullOrWhiteSpace(search))
            {
                lstSearch.Visible = false;
                return;
            }

            var matches = catalog.Where(p =>
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

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && lstSearch.Visible && lstSearch.Items.Count > 0)
            {
                e.SuppressKeyPress = true;
                AddToCart((CachedProduct)(lstSearch.SelectedItem ?? lstSearch.Items[0]));
            }
            else if (e.KeyCode == Keys.Down && lstSearch.Visible)
            {
                lstSearch.Focus();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                lstSearch.Visible = false;
            }
        }

        private void lstSearch_Click(object sender, EventArgs e)
        {
            if (lstSearch.SelectedItem is CachedProduct product)
                AddToCart(product);
        }

        private void AddToCart(CachedProduct product)
        {
            int qty = (int)numQty.Value;

            // Oversell is warn-but-allow: cached stock counts sales made offline minutes
            // ago, but production recording needs connectivity - so the counter must
            // never hard-stop. The server records the shortfall at sync for review.
            int alreadyInCart = cart.Where(c => c.SKU == product.SKU).Sum(c => c.Qty);
            if (alreadyInCart + qty > product.Stock)
            {
                var proceed = MessageBox.Show(
                    $"Stock shows only {product.Stock} of \"{product.Display}\" (cart already has {alreadyInCart}).\n\n" +
                    "If this was baked/decorated today, it may not be recorded yet - the sale will be " +
                    "flagged for the office.\n\nSell anyway?",
                    "Low Stock", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (proceed != DialogResult.Yes) return;
            }

            var existing = cart.FirstOrDefault(c => c.SKU == product.SKU);
            if (existing != null)
            {
                existing.Qty += qty;
                existing.LineTotal = existing.Qty * existing.Price;
                cart.ResetBindings();
            }
            else
            {
                cart.Add(new CartLine
                {
                    SKU = product.SKU,
                    Item = product.Display,
                    Qty = qty,
                    Price = product.Price,
                    LineTotal = qty * product.Price
                });
            }

            isSelecting = true;
            txtSearch.Clear();
            isSelecting = false;
            lstSearch.Visible = false;
            numQty.Value = 1;
            txtSearch.Focus();
            RefreshTotals();
        }

        // ---- discount / remove ----

        private void btnDiscount_Click(object sender, EventArgs e)
        {
            decimal? amount = PromptForDiscount();
            if (amount == null || amount.Value <= 0) return;

            decimal currentTotal = cart.Sum(c => c.LineTotal);
            if (amount.Value > currentTotal)
            {
                MessageBox.Show("Discount cannot be more than the sale total.", "Too Big",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            cart.Add(new CartLine
            {
                SKU = null,
                Item = "Discount",
                Qty = 1,
                Price = -amount.Value,
                LineTotal = -amount.Value
            });
            RefreshTotals();
        }

        private void btnRemoveLine_Click(object sender, EventArgs e)
        {
            if (dgvCart.CurrentRow?.DataBoundItem is CartLine line)
            {
                cart.Remove(line);
                RefreshTotals();
            }
        }

        // ---- totals / cash ----

        private void RefreshTotals()
        {
            decimal total = cart.Sum(c => c.LineTotal);
            lblTotal.Text = total.ToString("N2");
            RefreshChange();
        }

        private void txtCash_TextChanged(object sender, EventArgs e)
        {
            RefreshChange();
        }

        private void RefreshChange()
        {
            decimal total = cart.Sum(c => c.LineTotal);
            if (decimal.TryParse(txtCash.Text.Trim(), out decimal cash) && cash >= total)
                lblChange.Text = $"Change: {cash - total:N2}";
            else
                lblChange.Text = "Change: -";
        }

        // ---- complete sale ----

        private async void btnComplete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtStaff.Text))
            {
                MessageBox.Show("Enter the cashier's name first.", "Missing Cashier",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtStaff.Focus();
                return;
            }
            if (cart.Count == 0 || !cart.Any(c => c.SKU != null))
            {
                MessageBox.Show("Add at least one item to the sale.", "Empty Sale",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            decimal total = cart.Sum(c => c.LineTotal);
            if (total < 0)
            {
                MessageBox.Show("Sale total cannot be negative - reduce the discount.", "Invalid Total",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show($"Complete this sale for {total:N2}?", "Confirm Sale",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            var sale = new PosSaleDto
            {
                ClientSaleId = $"POS-{Guid.NewGuid():N}",
                Branch = branchName,
                StaffName = txtStaff.Text.Trim(),
                SoldAt = DateTime.Now,
                TotalAmount = total,
                Lines = cart.Select(c => new PosSaleLineDto
                {
                    SKU = c.SKU,
                    Description = c.Item,
                    Qty = c.Qty,
                    UnitPrice = c.Price,
                    LineTotal = c.LineTotal
                }).ToList()
            };

            // Queue first (durable), then decrement the cache and try to sync. If the
            // network is down the sale is safe on disk and the timer retries.
            PosLocalStore.QueueSale(sale);
            foreach (var line in cart.Where(c => c.SKU != null))
                PosLocalStore.DecrementCachedStock(line.SKU!, line.Qty);
            ReloadCatalogFromCache();

            decimal.TryParse(txtCash.Text.Trim(), out decimal cash);
            lblLastSale.Text = cash >= total && cash > 0
                ? $"Recorded. Change: {cash - total:N2}"
                : $"Recorded. Total: {total:N2}";

            cart.Clear();
            txtCash.Clear();
            RefreshTotals();
            txtSearch.Focus();
            UpdateStatusLabel();

            await RunSyncAsync();
        }

        // ---- sync plumbing ----

        private async void syncTimer_Tick(object sender, EventArgs e)
        {
            await RunSyncAsync();
        }

        private async void btnSyncNow_Click(object sender, EventArgs e)
        {
            btnSyncNow.Enabled = false;
            try { await RunSyncAsync(); }
            finally { btnSyncNow.Enabled = true; }
        }

        private async System.Threading.Tasks.Task RunSyncAsync()
        {
            var outcome = await PosSyncEngine.SyncAsync(branchName);

            if (outcome.CatalogRefreshed) ReloadCatalogFromCache();
            UpdateStatusLabel();

            // Rejections are rare and mean a sale did NOT count server-side - that
            // deserves a modal. Shortfalls are expected drift (unrecorded production);
            // they're flagged in the day log and the office report, not a popup.
            if (outcome.Rejected > 0)
            {
                MessageBox.Show(
                    "Some sales were rejected by the server and did not count:\n\n" +
                    string.Join("\n", outcome.Warnings.Where(w => w.Contains("REJECTED"))) +
                    "\n\nSee Today's Sales for details.",
                    "Sales Rejected", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateStatusLabel()
        {
            int pending = PosLocalStore.PendingCount();
            string state = PosSyncEngine.LastSyncOnline ? "ONLINE" : "OFFLINE";
            string last = PosSyncEngine.LastSyncAt?.ToString("HH:mm:ss") ?? "never";
            lblSyncStatus.Text = $"{state}   |   unsynced sales: {pending}   |   last sync: {last}";
        }

        private void btnDayLog_Click(object sender, EventArgs e)
        {
            using var form = new frmPosDayLog(branchName);
            form.ShowDialog();
        }

        // ---- helpers ----

        private static decimal? PromptForDiscount()
        {
            using var prompt = new Form
            {
                Text = "Discount Amount",
                Width = 320,
                Height = 160,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterParent,
                MaximizeBox = false,
                MinimizeBox = false
            };

            var txtAmount = new TextBox { Left = 20, Top = 20, Width = 260 };
            var btnConfirm = new Button
            {
                Text = "Apply Discount",
                Left = 20,
                Top = 60,
                Width = 260,
                Height = 35,
                DialogResult = DialogResult.OK
            };

            prompt.Controls.Add(txtAmount);
            prompt.Controls.Add(btnConfirm);
            prompt.AcceptButton = btnConfirm;

            if (prompt.ShowDialog() == DialogResult.OK &&
                decimal.TryParse(txtAmount.Text.Trim(), out decimal amount) && amount > 0)
                return amount;

            return null;
        }

        private class CartLine
        {
            [Browsable(false)]
            public string? SKU { get; set; }

            [DisplayName("Item")]
            public string Item { get; set; } = string.Empty;

            [DisplayName("Qty")]
            public int Qty { get; set; }

            [DisplayName("Price")]
            public decimal Price { get; set; }

            [DisplayName("Amount")]
            public decimal LineTotal { get; set; }
        }
    }
}
