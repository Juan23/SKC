using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace SKC_Bakery_Supplies
{
    public partial class frmBranchInventoryReport : Form
    {
        private List<BakeryProduct> currentList = new List<BakeryProduct>();
        private string loadedBranch = null; // the branch the grid currently shows, not just the dropdown
        private PrintDocument pDocStock = new PrintDocument();
        private int printStockIndex = 0;

        public frmBranchInventoryReport()
        {
            InitializeComponent();
        }

        private void frmBranchInventoryReport_Load(object sender, EventArgs e)
        {
            cmbBranch.Items.AddRange(new string[] { "Yoho", "Gaisano", "Liloy", "Labason" });
            cmbBranch.SelectedIndex = 0;
        }

        private async void btnLoad_Click(object sender, EventArgs e)
        {
            if (cmbBranch.SelectedItem == null) return;

            string branch = cmbBranch.SelectedItem.ToString();
            try
            {
                currentList = await CentralApiClient.GetBranchInventoryAsync(branch);
                loadedBranch = branch;
                dgvStock.DataSource = currentList;

                if (dgvStock.Columns["IsActive"] != null) dgvStock.Columns["IsActive"].Visible = false;
                if (dgvStock.Columns["SearchDisplay"] != null) dgvStock.Columns["SearchDisplay"].Visible = false;
                dgvStock.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not load stock for {branch}.\n\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAdjust_Click(object sender, EventArgs e)
        {
            if (loadedBranch == null || dgvStock.CurrentRow?.DataBoundItem is not BakeryProduct selected)
            {
                MessageBox.Show("Load a branch and select an item to adjust.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Adjust against the branch the grid is actually showing (loadedBranch), not the
            // dropdown's current value, in case the user changed it without reloading.
            using (var adjustForm = new frmAdjustInventory(selected, loadedBranch))
            {
                if (adjustForm.ShowDialog() == DialogResult.OK)
                {
                    btnLoad_Click(sender, e); // refresh so CurrentStock reflects the adjustment
                }
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (currentList == null || currentList.Count == 0) return;

            printStockIndex = 0; // Reset pagination
            pDocStock.PrintPage -= RenderStockReport;
            pDocStock.PrintPage += RenderStockReport;

            Form previewForm = new Form { Text = "Branch Stock Sheet Preview", Width = 800, Height = 1000, ShowIcon = false };
            Button btnPrintNow = new Button { Text = "Print Report", Dock = DockStyle.Top, Height = 45, Cursor = Cursors.Hand };
            btnPrintNow.Click += (s, ev) =>
            {
                PrintDialog pd = new PrintDialog { Document = pDocStock, UseEXDialog = true };
                if (pd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        printStockIndex = 0;
                        pDocStock.Print();
                    }
                    catch (Exception ex) { MessageBox.Show($"Print failed: {ex.Message}", "Error"); }
                }
            };

            PrintPreviewControl ppc = new PrintPreviewControl { Dock = DockStyle.Fill, Document = pDocStock };
            previewForm.Controls.Add(btnPrintNow);
            previewForm.Controls.Add(ppc);
            previewForm.ShowDialog();
        }

        private void RenderStockReport(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            Font titleFont = new Font("Courier New", 16, FontStyle.Bold);
            Font headerFont = new Font("Courier New", 12, FontStyle.Bold);
            Font regularFont = new Font("Courier New", 11);
            Brush brush = Brushes.Black;

            int y = 50;
            int margin = 50;
            int rightEdge = e.PageBounds.Width - 50;
            string branch = cmbBranch.SelectedItem?.ToString() ?? "";

            g.DrawString("SKC BAKERY SUPPLIES", titleFont, brush, margin, y);
            y += 30;
            g.DrawString($"BRANCH STOCK — {branch.ToUpper()}", headerFont, brush, margin, y);
            y += 25;
            g.DrawString($"DATE: {DateTime.Now:yyyy-MM-dd hh:mm tt}", regularFont, brush, margin, y);
            y += 40;

            g.DrawLine(Pens.Black, margin, y, rightEdge, y);
            y += 10;
            g.DrawString("QTY", headerFont, brush, margin, y);
            g.DrawString("ITEM DESCRIPTION", headerFont, brush, margin + 120, y);
            y += 25;
            g.DrawLine(Pens.Black, margin, y, rightEdge, y);
            y += 15;

            while (printStockIndex < currentList.Count)
            {
                var item = currentList[printStockIndex];
                string description = string.IsNullOrWhiteSpace(item.BaseName) ? item.SKU : $"{item.Brand} {item.BaseName}";

                g.DrawString(item.CurrentStock.ToString(), regularFont, brush, margin, y);
                g.DrawString(description, regularFont, brush, margin + 120, y);
                y += 25;

                printStockIndex++;

                if (y > e.MarginBounds.Bottom - 50)
                {
                    e.HasMorePages = true;
                    return;
                }
            }

            e.HasMorePages = false;
        }
    }
}
