using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;
using Dapper;
using System.Globalization;

namespace SKC_Bakery_Supplies
{
    public partial class frmViewProducts : Form
    {
        private List<BakeryProduct> masterList;

        public frmViewProducts()
        {
            InitializeComponent();
            LoadGrid();
        }

        private void frmViewProducts_Load(object sender, EventArgs e)
        {
            LoadGrid();
        }

        private void LoadGrid()
        {
            masterList = BakeryDatabaseManager.GetAllProducts();
            dgvProducts.DataSource = masterList;

            // Clean up the visuals
            if (dgvProducts.Columns["IsActive"] != null) dgvProducts.Columns["IsActive"].Visible = false;
            if (dgvProducts.Columns["SearchDisplay"] != null) dgvProducts.Columns["SearchDisplay"].Visible = false; // remove search display
            dgvProducts.ClearSelection();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string search = txtSearch.Text.Trim().ToLower();

            if (string.IsNullOrWhiteSpace(search))
            {
                dgvProducts.DataSource = masterList;
            }
            else
            {
                var filtered = masterList.Where(p =>
                    p.SearchDisplay.ToLower().Contains(search) ||
                    p.SKU.ToLower().Contains(search)).ToList();

                dgvProducts.DataSource = filtered;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvProducts.CurrentRow == null) return;

            var selected = (BakeryProduct)dgvProducts.CurrentRow.DataBoundItem;

            DialogResult confirm = MessageBox.Show($"Are you sure you want to discontinue {selected.SearchDisplay}?\n\nThis will remove it from search options, but historical logs will remain intact.", "Confirm Discontinue", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirm == DialogResult.Yes)
            {
                BakeryDatabaseManager.SoftDeleteProduct(selected.SKU);
                LoadGrid(); // Refresh to hide the item
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvProducts.CurrentRow == null) return;

            var selected = (BakeryProduct)dgvProducts.CurrentRow.DataBoundItem;

            using (var editForm = new frmEditProduct(selected))
            {
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    LoadGrid(); // Refresh grid with updated text
                }
            }
        }

        private string ToProperCase(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return text;
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text.ToLower().Trim());
        }

        // --- 2. THE AUTO-SKU ENGINE ---
        private void GenerateSKU()
        {
            string brandPart = FormatBrand(txtNewBrand.Text);
            string namePart = FormatBaseName(txtNewBaseName.Text);
            string uomPart = FormatUOM(txtNewUOM.Text);

            List<string> parts = new List<string>();
            if (!string.IsNullOrWhiteSpace(brandPart)) parts.Add(brandPart);
            if (!string.IsNullOrWhiteSpace(namePart)) parts.Add(namePart);
            if (!string.IsNullOrWhiteSpace(uomPart)) parts.Add(uomPart);

            txtNewSKU.Text = string.Join("-", parts);
        }

        private string FormatBrand(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return "";
            string clean = new string(input.Where(char.IsLetterOrDigit).ToArray()).ToLower();
            return clean.Length >= 3 ? clean.Substring(0, 3) : clean;
        }

        private string FormatBaseName(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return "";
            var words = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string initials = "";
            foreach (var word in words)
            {
                if (char.IsLetterOrDigit(word[0])) initials += word[0];
            }
            return initials.ToLower();
        }

        private string FormatUOM(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return "";
            return input.Replace(" ", "").ToLower();
        }

        // --- 3. THE SAVE EXECUTION ---
        private void btnSaveNew_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNewBaseName.Text) || string.IsNullOrWhiteSpace(txtNewSKU.Text))
            {
                MessageBox.Show("Base Name and SKU are required.");
                return;
            }

            BakeryProduct newProduct = new BakeryProduct
            {
                SKU = txtNewSKU.Text.ToLower().Trim(),
                Brand = ToProperCase(txtNewBrand.Text),
                BaseName = ToProperCase(txtNewBaseName.Text),
                UOM = txtNewUOM.Text.ToLower().Trim(),
                PackMultiplier = numNewMultiplier.Value,
                Price = numNewPrice.Value,
                IsActive = true
            };

            try
            {
                using (var connection = new SqliteConnection("Data Source=bakery_inventory.db"))
                {
                    string sql = "INSERT INTO Inventory (SKU, Brand, BaseName, UOM, PackMultiplier, Price, IsActive) VALUES (@SKU, @Brand, @BaseName, @UOM, @PackMultiplier, @Price, @IsActive)";
                    connection.Execute(sql, newProduct);
                }

                // 1. Refresh the grid instantly
                LoadGrid();

                // 2. Wipe the Quick Add board clean for the next item
                txtNewBrand.Clear();
                txtNewBaseName.Clear();
                txtNewUOM.Clear();
                txtNewSKU.Clear();
                numNewMultiplier.Value = 0;
                numNewPrice.Value = 0;

                txtNewBrand.Focus();
            }
            catch (SqliteException ex) when (ex.SqliteErrorCode == 19)
            {
                MessageBox.Show("A product with this generated SKU already exists. Please manually modify the SKU to make it unique (e.g., add a -2).", "Duplicate SKU");
            }
        }

        private void txtNewBrand_TextChanged(object sender, EventArgs e)
        {
            GenerateSKU();
        }

        private void txtNewBaseName_TextChanged(object sender, EventArgs e)
        {
            GenerateSKU();
        }

        private void txtNewUOM_TextChanged(object sender, EventArgs e)
        {
            GenerateSKU();
        }

        private void HighlightNumericText(object sender, EventArgs e)
        {
            if (sender is NumericUpDown numericControl)
            {
                // Selects from index 0 to the absolute end of the string
                numericControl.Select(0, numericControl.Text.Length);
            }
        }
    }
}
