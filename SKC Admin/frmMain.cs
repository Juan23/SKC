using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SKC_Admin
{
    public partial class frmMain : Form
    {
        private List<AdminProduct> masterCatalog = new List<AdminProduct>();
        private List<Recipe> recipes = new List<Recipe>();

        public frmMain()
        {
            InitializeComponent();

            var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            Text = $"SKC Admin  (v{version?.ToString(3)})";

            // Added in code rather than the Designer so we don't hand-edit frmMain.Designer.cs
            // (workspace convention - see SKC Bakery Supplies/.claudesettings.json).
            var btnSetPrice = new Button
            {
                Location = new System.Drawing.Point(307, 6),
                Name = "btnSetPrice",
                Size = new System.Drawing.Size(120, 28),
                Text = "Set Price...",
                UseVisualStyleBackColor = true
            };
            btnSetPrice.Click += btnSetPrice_Click;
            pnlProductActions.Controls.Add(btnSetPrice);
        }

        private async void frmMain_Load(object sender, EventArgs e)
        {
            await RefreshProductsAsync();
            await RefreshRecipesAsync();
        }

        private async System.Threading.Tasks.Task RefreshProductsAsync()
        {
            try
            {
                masterCatalog = await AdminApiClient.GetAllProductsAsync();
                dgvProducts.DataSource = masterCatalog;
                dgvProducts.ClearSelection();
            }
            catch (Exception ex)
            {
                // A 403 here means this machine isn't on the office/owner IP allowlist -
                // the server enforces the actual restriction, this is just the friendly message.
                MessageBox.Show($"Could not load the product catalog.\n\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                masterCatalog = new List<AdminProduct>();
            }
        }

        private async System.Threading.Tasks.Task RefreshRecipesAsync()
        {
            try
            {
                recipes = await AdminApiClient.GetRecipesAsync();
                dgvRecipes.DataSource = recipes.Select(r => new
                {
                    r.RecipeId,
                    r.Name,
                    r.Kind,
                    r.OutputSku,
                    r.OutputQty,
                    Lines = r.Lines.Count
                }).ToList();
                dgvRecipes.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not load recipes.\n\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                recipes = new List<Recipe>();
            }
        }

        private async void btnRefreshProducts_Click(object sender, EventArgs e)
        {
            await RefreshProductsAsync();
        }

        private async void btnClassify_Click(object sender, EventArgs e)
        {
            if (dgvProducts.CurrentRow?.DataBoundItem is not AdminProduct selected)
            {
                MessageBox.Show("Select a product first.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var form = new frmClassifyProduct(selected))
            {
                if (form.ShowDialog() == DialogResult.OK)
                    await RefreshProductsAsync();
            }
        }

        private async void btnSetPrice_Click(object? sender, EventArgs e)
        {
            if (dgvProducts.CurrentRow?.DataBoundItem is not AdminProduct selected)
            {
                MessageBox.Show("Select a product first.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            decimal? price = PromptForPrice(selected);
            if (price == null) return;

            try
            {
                await AdminApiClient.SetPriceAsync(selected.SKU, price.Value);
                await RefreshProductsAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Set Price Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Selling price sets POS visibility: price > 0 = sellable at branches, 0 = not
        // sellable (raw materials, intermediaries like chiffon). Same inline-prompt shape
        // as SKC Branch's PromptForName.
        private static decimal? PromptForPrice(AdminProduct product)
        {
            using var prompt = new Form
            {
                Text = "Set Selling Price",
                Width = 420,
                Height = 210,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterParent,
                MaximizeBox = false,
                MinimizeBox = false
            };

            var lblInfo = new Label
            {
                Left = 20,
                Top = 15,
                Width = 360,
                Height = 40,
                Text = $"{product.Display} ({product.SKU})\nCurrent price: {product.Price:N2}   -   0 = not sellable at POS"
            };
            var txtPrice = new TextBox { Left = 20, Top = 60, Width = 360, Text = product.Price.ToString("0.##") };
            var btnConfirm = new Button
            {
                Text = "Save",
                Left = 20,
                Top = 100,
                Width = 360,
                Height = 35,
                DialogResult = DialogResult.OK
            };

            prompt.Controls.Add(lblInfo);
            prompt.Controls.Add(txtPrice);
            prompt.Controls.Add(btnConfirm);
            prompt.AcceptButton = btnConfirm;

            while (prompt.ShowDialog() == DialogResult.OK)
            {
                if (decimal.TryParse(txtPrice.Text.Trim(), out decimal price) && price >= 0)
                    return price;

                MessageBox.Show("Enter a valid non-negative price.", "Invalid Price",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return null;
        }

        private async void btnRefreshRecipes_Click(object sender, EventArgs e)
        {
            await RefreshRecipesAsync();
        }

        private async void btnNewRecipe_Click(object sender, EventArgs e)
        {
            if (masterCatalog.Count == 0)
            {
                MessageBox.Show("Load the product catalog first (Products tab) so recipe inputs/outputs can be picked.",
                    "No Products", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var form = new frmRecipeEditor(masterCatalog))
            {
                if (form.ShowDialog() == DialogResult.OK)
                    await RefreshRecipesAsync();
            }
        }

        private async void btnEditRecipe_Click(object sender, EventArgs e)
        {
            if (dgvRecipes.CurrentRow == null)
            {
                MessageBox.Show("Select a recipe first.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int recipeId = (int)dgvRecipes.CurrentRow.Cells["RecipeId"].Value;
            var recipe = recipes.FirstOrDefault(r => r.RecipeId == recipeId);
            if (recipe == null) return;

            using (var form = new frmRecipeEditor(recipe, masterCatalog))
            {
                if (form.ShowDialog() == DialogResult.OK)
                    await RefreshRecipesAsync();
            }
        }

        private async void btnDeactivateRecipe_Click(object sender, EventArgs e)
        {
            if (dgvRecipes.CurrentRow == null)
            {
                MessageBox.Show("Select a recipe first.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int recipeId = (int)dgvRecipes.CurrentRow.Cells["RecipeId"].Value;
            string name = dgvRecipes.CurrentRow.Cells["Name"].Value?.ToString() ?? recipeId.ToString();

            DialogResult confirm = MessageBox.Show($"Deactivate recipe \"{name}\"? Branches will no longer see it as an option.",
                "Confirm Deactivate", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            try
            {
                await AdminApiClient.DeactivateRecipeAsync(recipeId);
                await RefreshRecipesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Deactivate Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
