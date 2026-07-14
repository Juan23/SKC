using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;

namespace SKC_Bakery_Supplies
{
    public partial class frmEditProduct : Form
    {
        private string targetSKU;

        public frmEditProduct(BakeryProduct itemToEdit)
        {
            InitializeComponent();

            // Map the data into the form
            targetSKU = itemToEdit.SKU;
            txtSKU.Text = itemToEdit.SKU;
            //txtUOM.Text = itemToEdit.UOM;
            //txtMultiplier.Text = itemToEdit.PackMultiplier.ToString();

            txtBrand.Text = itemToEdit.Brand;
            txtBaseName.Text = itemToEdit.BaseName;
        }

        private string ToProperCase(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return text;
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text.ToLower().Trim());
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBaseName.Text))
            {
                MessageBox.Show("Base Name cannot be blank.");
                return;
            }

            string cleanBrand = ToProperCase(txtBrand.Text);
            string cleanBaseName = ToProperCase(txtBaseName.Text);

            try
            {
                // Execute the partial update
                await CentralApiClient.UpdateProductAsync(targetSKU, cleanBrand, cleanBaseName);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }
    }
}