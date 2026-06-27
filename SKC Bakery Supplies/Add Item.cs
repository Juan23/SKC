using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Dapper;
using Microsoft.Data.Sqlite;
using System.Globalization;

namespace SKC_Bakery_Supplies
{
    public partial class frmAddMasterItem : Form
    {
        public BakeryProduct NewProduct { get; private set; }

        public frmAddMasterItem(string unknownName)
        {
            InitializeComponent();

            // Map the unknown text directly into the BaseName, NOT the SKU
            txtBaseName.Text = unknownName;

            // Trigger the auto-generator immediately in case they already typed a good name
            GenerateSKU();
        }

        // --- THE AUTO-SKU ENGINE ---
        private void GenerateSKU()
        {
            string brandPart = FormatBrand(txtBrand.Text);
            string namePart = FormatBaseName(txtBaseName.Text);
            string uomPart = FormatUOM(txtUOM.Text);

            List<string> parts = new List<string>();
            if (!string.IsNullOrWhiteSpace(brandPart)) parts.Add(brandPart);
            if (!string.IsNullOrWhiteSpace(namePart)) parts.Add(namePart);
            if (!string.IsNullOrWhiteSpace(uomPart)) parts.Add(uomPart);

            // Combine with hyphens (e.g., ber-dc-1kg)
            txtSKU.Text = string.Join("-", parts);
        }

        // Grabs the first 3 letters of the brand (e.g., "Beryl's" -> "ber")
        private string FormatBrand(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return "";
            string clean = new string(input.Where(char.IsLetterOrDigit).ToArray()).ToLower();
            return clean.Length >= 3 ? clean.Substring(0, 3) : clean;
        }

        // Grabs the initials of the base name (e.g., "Dark Chocolate" -> "dc")
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

        // Cleans the UOM (e.g., " 1 kg " -> "1kg")
        private string FormatUOM(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return "";
            return input.Replace(" ", "").ToLower();
        }

        // --- BUTTON LOGIC ---
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBaseName.Text) || string.IsNullOrWhiteSpace(txtSKU.Text))
            {
                MessageBox.Show("Base Name and SKU are required.");
                return;
            }

            NewProduct = new BakeryProduct
            {
                SKU = txtSKU.Text.ToLower().Trim(),
                Brand = ToProperCase(txtBrand.Text),
                BaseName = ToProperCase(txtBaseName.Text),
                UOM = txtUOM.Text.ToLower().Trim(),
                PackMultiplier = numMultiplier.Value,
                Price = numPrice.Value
            };

            try
            {
                using (var connection = new SqliteConnection("Data Source=bakery_inventory.db"))
                {
                    string sql = "INSERT INTO Inventory (SKU, Brand, BaseName, UOM, PackMultiplier, Price) VALUES (@SKU, @Brand, @BaseName, @UOM, @PackMultiplier, @Price)";
                    connection.Execute(sql, NewProduct);
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (SqliteException ex) when (ex.SqliteErrorCode == 19) // SQLite constraint violation (Duplicate SKU)
            {
                MessageBox.Show("A product with this generated SKU already exists. Please modify the Brand, Name, or UOM to make it unique.", "Duplicate SKU Collision");
            }
        }

        private string ToProperCase(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return text;
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text.ToLower().Trim());
        }

        private void txtBrand_TextChanged(object sender, EventArgs e)
        {
            GenerateSKU();
        }

        private void txtBaseName_TextChanged(object sender, EventArgs e)
        {
            GenerateSKU();
        }

        private void txtUOM_TextChanged(object sender, EventArgs e)
        {
            GenerateSKU();
        }
    }
}
