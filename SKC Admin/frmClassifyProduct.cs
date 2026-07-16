using System;
using System.Windows.Forms;

namespace SKC_Admin
{
    public partial class frmClassifyProduct : Form
    {
        private readonly AdminProduct product;

        public frmClassifyProduct(AdminProduct product)
        {
            this.product = product;
            InitializeComponent();

            lblItem.Text = $"{product.Display} ({product.SKU})";
            cmbCategory.Items.AddRange(new object[] { "RawMaterial", "BakedGood", "DecoratedGood" });
            cmbCategory.SelectedItem = product.Category;
            txtUom.Text = product.Uom ?? "";
            numPackMultiplier.Value = product.PackMultiplier <= 0 ? 1 : product.PackMultiplier;
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (cmbCategory.SelectedItem == null)
            {
                MessageBox.Show("Select a category.", "Missing Category", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string? uom = string.IsNullOrWhiteSpace(txtUom.Text) ? null : txtUom.Text.Trim();
                await AdminApiClient.SetClassificationAsync(product.SKU, cmbCategory.SelectedItem.ToString()!, uom, numPackMultiplier.Value);
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Save Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
