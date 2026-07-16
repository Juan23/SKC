using System.ComponentModel;

namespace SKC_Branch
{
    public partial class frmMain : Form
    {
        private readonly string branchName;
        private List<BranchProduct> masterCatalog = new List<BranchProduct>();

        public frmMain(string branchName)
        {
            this.branchName = branchName;
            InitializeComponent();
            Text = $"SKC Branch - {branchName}";
            lblHeader.Text = $"Pending Deliveries for {branchName}";
        }

        private async void frmMain_Load(object sender, EventArgs e)
        {
            try
            {
                masterCatalog = await BranchApiClient.GetAllProductsAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not load the product catalog. Item names will show as SKUs.\n\n{ex.Message}",
                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            await RefreshPendingAsync();
        }

        private async Task RefreshPendingAsync()
        {
            try
            {
                var pending = await BranchApiClient.GetPendingDeliveriesAsync(branchName);
                dgvPending.DataSource = pending;
                dgvPending.ClearSelection();
                dgvLines.DataSource = null;
                btnAccept.Enabled = pending.Count > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not load pending deliveries.\n\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            await RefreshPendingAsync();
        }

        private async void dgvPending_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvPending.CurrentRow?.DataBoundItem is not DeliveryTicketSummary selected) return;

            try
            {
                var lines = await BranchApiClient.GetDeliveryDetailsAsync(selected.TransactionId);

                // Translate SKUs into readable names for the branch staff.
                var display = lines.Select(l =>
                {
                    var product = masterCatalog.FirstOrDefault(p => p.SKU == l.SKU);
                    return new DeliveryLineDisplay
                    {
                        Item = product != null ? $"{product.Brand} {product.BaseName}" : l.SKU,
                        Qty = l.Qty
                    };
                })
                .GroupBy(d => d.Item)
                .Select(g => new DeliveryLineDisplay { Item = g.Key, Qty = g.Sum(d => d.Qty) })
                .ToList();

                dgvLines.DataSource = display;
                dgvLines.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not load delivery details.\n\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnAccept_Click(object sender, EventArgs e)
        {
            if (dgvPending.CurrentRow?.DataBoundItem is not DeliveryTicketSummary selected)
            {
                MessageBox.Show("Select a delivery to accept.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string acceptedBy = PromptForName();
            if (string.IsNullOrWhiteSpace(acceptedBy)) return;

            DialogResult confirm = MessageBox.Show(
                $"Accept delivery {selected.TransactionId} ({selected.TotalItems} items) as {acceptedBy}?\n\n" +
                "Only accept after physically checking the items against the delivery sheet.",
                "Confirm Acceptance",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            btnAccept.Enabled = false;
            try
            {
                await BranchApiClient.AcceptDeliveryAsync(selected.TransactionId, branchName, acceptedBy.Trim());
                MessageBox.Show($"Delivery {selected.TransactionId} accepted.", "Accepted",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Accept Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            await RefreshPendingAsync();
        }

        private static string PromptForName()
        {
            using var prompt = new Form
            {
                Text = "Who is accepting this delivery?",
                Width = 380,
                Height = 160,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterParent,
                MaximizeBox = false,
                MinimizeBox = false
            };

            var txtName = new TextBox { Left = 20, Top = 20, Width = 320 };
            var btnConfirm = new Button
            {
                Text = "Confirm",
                Left = 20,
                Top = 60,
                Width = 320,
                Height = 35,
                DialogResult = DialogResult.OK
            };

            prompt.Controls.Add(txtName);
            prompt.Controls.Add(btnConfirm);
            prompt.AcceptButton = btnConfirm;

            return prompt.ShowDialog() == DialogResult.OK ? txtName.Text : string.Empty;
        }

        private class DeliveryLineDisplay
        {
            [DisplayName("Item")]
            public string Item { get; set; } = string.Empty;

            [DisplayName("Qty")]
            public int Qty { get; set; }
        }
    }
}
