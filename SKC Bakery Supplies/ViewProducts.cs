using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;

namespace SKC_Bakery_Supplies
{
    public partial class frmViewProducts : Form
    {
        private List<BakeryProduct> masterList;
        private PrintDocument pDocInventory = new PrintDocument();
        private int printInventoryIndex = 0;

        public frmViewProducts()
        {
            InitializeComponent();
            LoadGrid();
        }

        private void frmViewProducts_Load(object sender, EventArgs e)
        {
            LoadGrid();
        }

        private async void LoadGrid()
        {
            masterList = await CentralApiClient.GetAllProductsAsync();
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

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvProducts.CurrentRow == null) return;

            var selected = (BakeryProduct)dgvProducts.CurrentRow.DataBoundItem;

            DialogResult confirm = MessageBox.Show($"Are you sure you want to discontinue {selected.SearchDisplay}?\n\nThis will remove it from search options, but historical logs will remain intact.", "Confirm Discontinue", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirm == DialogResult.Yes)
            {
                try
                {
                    await CentralApiClient.DeactivateProductAsync(selected.SKU);
                    LoadGrid(); // Refresh to hide the item
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error");
                }
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
            //string uomPart = FormatUOM(txtNewUOM.Text);

            List<string> parts = new List<string>();
            if (!string.IsNullOrWhiteSpace(brandPart)) parts.Add(brandPart);
            if (!string.IsNullOrWhiteSpace(namePart)) parts.Add(namePart);
            // if (!string.IsNullOrWhiteSpace(uomPart)) parts.Add(uomPart);

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

        /*
        private string FormatUOM(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return "";
            return input.Replace(" ", "").ToLower();
        }
        */

        // --- 3. THE SAVE EXECUTION ---
        private async void btnSaveNew_Click(object sender, EventArgs e)
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
                //UOM = txtNewUOM.Text.ToLower().Trim(),
                //PackMultiplier = numNewMultiplier.Value,
                Price = numNewPrice.Value,
                IsActive = true
            };

            try
            {
                await CentralApiClient.AddProductAsync(newProduct);

                // 1. Refresh the grid instantly
                LoadGrid();

                // 2. Wipe the Quick Add board clean for the next item
                txtNewBrand.Clear();
                txtNewBaseName.Clear();
                txtNewSKU.Clear();
                numNewPrice.Value = 0;

                txtNewBrand.Focus();
            }
            catch (Exception ex) when (ex.Message == "Duplicate SKU")
            {
                MessageBox.Show("A product with this generated SKU already exists. Please manually modify the SKU to make it unique (e.g., add a -2).", "Duplicate SKU");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
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

        private void HighlightNumericText(object sender, EventArgs e)
        {
            if (sender is NumericUpDown numericControl)
            {
                // Selects from index 0 to the absolute end of the string
                numericControl.Select(0, numericControl.Text.Length);
            }
        }

        private void btnPrintInventory_Click(object sender, EventArgs e)
        {
            if (masterList == null || masterList.Count == 0) return;

            printInventoryIndex = 0; // Reset pagination
            pDocInventory.PrintPage -= RenderInventoryReport;
            pDocInventory.PrintPage += RenderInventoryReport;

            Form previewForm = new Form { Text = "Inventory Sheet Preview", Width = 800, Height = 1000, ShowIcon = false };
            Button btnPrint = new Button { Text = "Print Report", Dock = DockStyle.Top, Height = 45, Cursor = Cursors.Hand };
            btnPrint.Click += (s, ev) =>
            {
                PrintDialog pd = new PrintDialog { Document = pDocInventory, UseEXDialog = true };
                if (pd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        printInventoryIndex = 0;
                        pDocInventory.Print();
                    }
                    catch (Exception ex) { MessageBox.Show($"Print failed: {ex.Message}", "Error"); }
                }
            };

            PrintPreviewControl ppc = new PrintPreviewControl { Dock = DockStyle.Fill, Document = pDocInventory };
            previewForm.Controls.Add(btnPrint);
            previewForm.Controls.Add(ppc);
            previewForm.ShowDialog();
        }

        private void RenderInventoryReport(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            Font titleFont = new Font("Courier New", 16, FontStyle.Bold);
            Font headerFont = new Font("Courier New", 12, FontStyle.Bold);
            Font regularFont = new Font("Courier New", 11);
            Brush brush = Brushes.Black;

            int y = 50;
            int margin = 50;
            int rightEdge = e.PageBounds.Width - 50;

            g.DrawString("SKC BAKERY SUPPLIES", titleFont, brush, margin, y);
            y += 30;
            g.DrawString("PHYSICAL INVENTORY COUNT SHEET", headerFont, brush, margin, y);
            y += 25;
            g.DrawString($"DATE: {DateTime.Now:yyyy-MM-dd hh:mm tt}", regularFont, brush, margin, y);
            y += 40;

            // Table Headers
            g.DrawLine(Pens.Black, margin, y, rightEdge, y);
            y += 10;
            g.DrawString("SYSTEM QTY", headerFont, brush, margin, y);
            g.DrawString("ITEM DESCRIPTION", headerFont, brush, margin + 120, y);
            g.DrawString("ACTUAL QTY", headerFont, brush, rightEdge - 150, y);
            y += 25;
            g.DrawLine(Pens.Black, margin, y, rightEdge, y);
            y += 15;

            // Render Items with Pagination
            while (printInventoryIndex < masterList.Count)
            {
                var item = masterList[printInventoryIndex];
                string description = string.IsNullOrWhiteSpace(item.BaseName) ? item.SKU : $"{item.Brand} {item.BaseName}";

                g.DrawString(item.CurrentStock.ToString(), regularFont, brush, margin, y);
                g.DrawString(description, regularFont, brush, margin + 120, y);
                g.DrawString("__________", regularFont, brush, rightEdge - 150, y); // Blank line for physical writing
                y += 25;

                printInventoryIndex++;

                // Page break logic
                if (y > e.MarginBounds.Bottom - 50)
                {
                    e.HasMorePages = true;
                    return;
                }
            }

            // End of Document
            g.DrawLine(Pens.Black, margin, y, rightEdge, y);
            y += 40;
            g.DrawString("Counted By: ___________________", regularFont, brush, margin, y);
            g.DrawString("Verified By: ___________________", regularFont, brush, rightEdge - 300, y);

            e.HasMorePages = false;
        }
    }
}
