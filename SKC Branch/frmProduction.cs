using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace SKC_Branch
{
    public partial class frmProduction : Form
    {
        private readonly string branchName;
        private List<BranchProduct> masterCatalog = new List<BranchProduct>();
        private List<Recipe> allRecipes = new List<Recipe>();

        public frmProduction(string branchName)
        {
            this.branchName = branchName;
            InitializeComponent();
            Text = $"Record Baking / Decorating - {branchName}";
        }

        private async void frmProduction_Load(object sender, EventArgs e)
        {
            try
            {
                masterCatalog = await BranchApiClient.GetAllProductsAsync();
            }
            catch
            {
                masterCatalog = new List<BranchProduct>();
            }

            try
            {
                allRecipes = await BranchApiClient.GetRecipesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not load recipes.\n\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                allRecipes = new List<Recipe>();
            }

            cmbKind.Items.AddRange(new object[] { "Baking", "Decorating" });
            if (cmbKind.Items.Count > 0) cmbKind.SelectedIndex = 0;
        }

        private void cmbKind_SelectedIndexChanged(object sender, EventArgs e)
        {
            string? kind = cmbKind.SelectedItem?.ToString();
            var filtered = allRecipes.Where(r => r.Kind == kind).ToList();
            cmbRecipe.DataSource = filtered;
            cmbRecipe.DisplayMember = "Name";

            if (filtered.Count == 0)
            {
                dgvPreview.DataSource = null;
                numOutputQty.Value = 0;
            }
        }

        private void cmbRecipe_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbRecipe.SelectedItem is Recipe recipe)
                numOutputQty.Value = Math.Max(0, Math.Round(recipe.OutputQty * numMultiplier.Value));

            RefreshPreview();
        }

        private void numMultiplier_ValueChanged(object sender, EventArgs e)
        {
            if (cmbRecipe.SelectedItem is Recipe recipe)
                numOutputQty.Value = Math.Max(0, Math.Round(recipe.OutputQty * numMultiplier.Value));

            RefreshPreview();
        }

        private void RefreshPreview()
        {
            if (cmbRecipe.SelectedItem is not Recipe recipe)
            {
                dgvPreview.DataSource = null;
                return;
            }

            var rows = recipe.Lines.Select(l =>
            {
                var product = masterCatalog.FirstOrDefault(p => p.SKU == l.InputSku);
                string name = product != null ? $"{product.Brand} {product.BaseName}" : l.InputSku;
                int qtyNeeded = (int)Math.Ceiling(l.Qty * numMultiplier.Value);
                return new PreviewRow { Item = name, QtyNeeded = qtyNeeded };
            }).ToList();

            dgvPreview.DataSource = rows;
        }

        private async void btnSubmit_Click(object sender, EventArgs e)
        {
            if (cmbRecipe.SelectedItem is not Recipe recipe)
            {
                MessageBox.Show("Select a recipe.", "No Recipe", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtStaff.Text))
            {
                MessageBox.Show("Enter who baked/decorated this batch.", "Missing Staff Name",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string transactionId = $"PRD-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper()}";

            btnSubmit.Enabled = false;
            try
            {
                var result = await BranchApiClient.SubmitProductionAsync(
                    branchName, recipe.RecipeId, txtStaff.Text.Trim(), numMultiplier.Value, (int)numOutputQty.Value, transactionId);

                MessageBox.Show($"Recorded. Produced {result.OutputQty} of {result.OutputSku}.", "Production Recorded",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                txtStaff.Clear();
                numMultiplier.Value = 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Production Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                btnSubmit.Enabled = true;
            }
        }

        private void btnHistory_Click(object sender, EventArgs e)
        {
            using var form = new frmProductionHistory(branchName);
            form.ShowDialog();
        }

        private class PreviewRow
        {
            [DisplayName("Item")]
            public string Item { get; set; } = string.Empty;

            [DisplayName("Qty Needed")]
            public int QtyNeeded { get; set; }
        }
    }
}
