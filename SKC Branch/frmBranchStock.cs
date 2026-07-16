using System;
using System.Windows.Forms;

namespace SKC_Branch
{
    public partial class frmBranchStock : Form
    {
        private readonly string branchName;

        public frmBranchStock(string branchName)
        {
            this.branchName = branchName;
            InitializeComponent();
            Text = $"My Stock - {branchName}";
        }

        private async void frmBranchStock_Load(object sender, EventArgs e)
        {
            await LoadStockAsync();
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            await LoadStockAsync();
        }

        private async System.Threading.Tasks.Task LoadStockAsync()
        {
            try
            {
                var stock = await BranchApiClient.GetMyStockAsync(branchName);
                dgvStock.DataSource = stock;
                dgvStock.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not load your stock.\n\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
