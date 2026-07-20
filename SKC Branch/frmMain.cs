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

            var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            Text = $"SKC Branch - {branchName}  (v{version?.ToString(3)})";
            lblHeader.Text = $"Pending Deliveries for {branchName}";

            // Deliveries is now reached from the POS hub (frmPos, the app's startup screen)
            // rather than the other way around - navigation buttons live there now.
        }

        private async void frmMain_Load(object sender, EventArgs e)
        {
            // Startup must survive being offline without blocking modals - the POS works
            // from its local cache, so an offline launch should land straight in a usable
            // app. Failures here show in the header instead of a MessageBox; deliveries
            // and production still surface their own errors when actually used.
            try
            {
                masterCatalog = await BranchApiClient.GetAllProductsAsync();
            }
            catch
            {
                masterCatalog = new List<BranchProduct>();
            }

            await RefreshPendingAsync(suppressErrors: true);
        }

        private async Task RefreshPendingAsync(bool suppressErrors = false)
        {
            try
            {
                var pending = await BranchApiClient.GetPendingDeliveriesAsync(branchName);
                dgvPending.DataSource = pending;
                dgvPending.ClearSelection();
                dgvLines.DataSource = null;
                btnAccept.Enabled = pending.Count > 0;
                lblHeader.Text = $"Pending Deliveries for {branchName}";
            }
            catch (Exception ex)
            {
                lblHeader.Text = $"Pending Deliveries for {branchName}  (offline - could not reach server)";
                if (!suppressErrors)
                {
                    MessageBox.Show($"Could not load pending deliveries.\n\n{ex.Message}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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
            catch (AlreadyAcceptedException ex)
            {
                // The stock is already in - a retry after a lost response, or a double-accept. Report
                // it as done (not a scary failure); the refresh below drops it from the pending list.
                MessageBox.Show(ex.Message, "Already Accepted", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (DeliveryChangedException ex)
            {
                MessageBox.Show(ex.Message, "Delivery Changed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Accept Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                // Never leave Accept stuck disabled: on an offline failure the refresh below also
                // fails and wouldn't re-enable it, trapping the user until they hit Refresh. The
                // refresh re-tunes this to pending.Count > 0 on success.
                btnAccept.Enabled = true;
            }

            // suppressErrors: the accept above already reported its own outcome (including offline);
            // a failed refresh here shouldn't stack a second "could not load" dialog on top.
            await RefreshPendingAsync(suppressErrors: true);
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
