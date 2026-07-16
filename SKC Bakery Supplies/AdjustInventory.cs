using System;
using System.Windows.Forms;

namespace SKC_Bakery_Supplies
{
    public partial class frmAdjustInventory : Form
    {
        private readonly string targetSKU;
        private readonly string targetBranch;

        // Office stock (from ViewProducts).
        public frmAdjustInventory(BakeryProduct itemToAdjust)
            : this(itemToAdjust, "Office")
        {
        }

        // Branch-scoped stock (from Branch Inventory Report).
        public frmAdjustInventory(BakeryProduct itemToAdjust, string branch)
        {
            InitializeComponent();

            targetSKU = itemToAdjust.SKU;
            targetBranch = branch;
            lblItem.Text = $"{itemToAdjust.SearchDisplay} ({itemToAdjust.SKU})";
            lblSystemQty.Text = $"System Qty: {itemToAdjust.CurrentStock}";
            numActualQty.Value = itemToAdjust.CurrentStock;

            if (!string.Equals(branch, "Office", StringComparison.Ordinal))
                Text = $"Adjust Stock — {branch}";
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtReason.Text))
            {
                MessageBox.Show("Please enter a reason for this adjustment.");
                return;
            }

            // 0 is the "not set" sentinel for the cost field - leaving it lets the server
            // fall back to the SKU's last purchase cost instead of recording found stock at $0.
            decimal? unitCost = numUnitCost.Value > 0 ? numUnitCost.Value : (decimal?)null;

            try
            {
                await CentralApiClient.AdjustInventoryAsync(targetSKU, (int)numActualQty.Value, unitCost, txtReason.Text.Trim(), targetBranch);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
