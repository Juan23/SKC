using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
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
            // string uomPart = FormatUOM(txtUOM.Text);

            List<string> parts = new List<string>();
            if (!string.IsNullOrWhiteSpace(brandPart)) parts.Add(brandPart);
            if (!string.IsNullOrWhiteSpace(namePart)) parts.Add(namePart);
            // if (!string.IsNullOrWhiteSpace(uomPart)) parts.Add(uomPart);

            // Combine with hyphens (e.g., ber-dc-1kg)
            txtSKU.Text = string.Join("-", parts);
        }

        // First 4 alphanumerics of the brand (e.g., "Beryl's" -> "bery"); "" when there's no brand.
        private string FormatBrand(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return "";
            string clean = new string(input.Where(char.IsLetterOrDigit).ToArray()).ToLower();
            return clean.Length >= 4 ? clean.Substring(0, 4) : clean;
        }

        // Base name squashed to its first 8 alphanumerics (e.g., "Dark Chocolate" -> "darkchoc").
        // Far more entropy than the old word-initials ("dc"), so stems rarely collide - and the save
        // path (btnSave_Click) auto-appends -2, -3, ... to settle whatever still does.
        private string FormatBaseName(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return "";
            string clean = new string(input.Where(char.IsLetterOrDigit).ToArray()).ToLower();
            return clean.Length >= 8 ? clean.Substring(0, 8) : clean;
        }

        // Cleans the UOM (e.g., " 1 kg " -> "1kg")
        /*
        private string FormatUOM(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return "";
            return input.Replace(" ", "").ToLower();
        }
        */

        // --- BUTTON LOGIC ---
        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBaseName.Text) || string.IsNullOrWhiteSpace(txtSKU.Text))
            {
                MessageBox.Show("Base Name and SKU are required.");
                return;
            }

            // Build into a local, not the NewProduct property: the loop below awaits, and mutating a
            // form-level object across the await would let a re-entrant click corrupt this submission.
            var product = new BakeryProduct
            {
                SKU = txtSKU.Text.ToLower().Trim(),
                Brand = ToProperCase(txtBrand.Text),
                BaseName = ToProperCase(txtBaseName.Text),
                // UOM = txtUOM.Text.ToLower().Trim(),
                // PackMultiplier = numMultiplier.Value,
                Price = numPrice.Value,
                IsActive = true
            };

            // Disable the trigger while a save is in flight so a double-click or held Enter (this form's
            // AcceptButton) can't fire a second, concurrent submit - which the retry loop would otherwise
            // absorb as a "collision" and silently save as a duplicate -2 row.
            btnSave.Enabled = false;
            try
            {
                // The generated SKU is a readable stem, not a guaranteed-unique key. Instead of bouncing
                // a collision back to be hand-fixed, transparently append -2, -3, ... until the server
                // accepts one. The 23505 PK violation is the source of truth.
                string baseSku = product.SKU;
                for (int suffix = 1; suffix <= 99; suffix++)
                {
                    product.SKU = suffix == 1 ? baseSku : $"{baseSku}-{suffix}";
                    try
                    {
                        await CentralApiClient.AddProductAsync(product);

                        // A suffix means an item with this stem already existed. Rare now (wide stems),
                        // so surface it: tells the admin the real SKU and lets them catch a true dupe.
                        if (suffix > 1)
                            MessageBox.Show($"A product with a similar SKU already existed, so this was saved as \"{product.SKU}\". Please double-check it isn't a duplicate.",
                                "SKU Auto-Numbered", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        NewProduct = product;
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                        return;
                    }
                    catch (Exception ex) when (ex.Message == "Duplicate SKU")
                    {
                        // taken - fall through to the next suffix
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Save Failed");
                        return;
                    }
                }

                MessageBox.Show("Could not generate a unique SKU after 99 attempts. Please set one manually.", "Duplicate SKU Collision");
            }
            finally
            {
                btnSave.Enabled = true;
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
    }
}
