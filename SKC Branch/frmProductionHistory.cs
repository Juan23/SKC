using System;
using System.Windows.Forms;

namespace SKC_Branch
{
    public partial class frmProductionHistory : Form
    {
        private readonly string branchName;

        public frmProductionHistory(string branchName)
        {
            this.branchName = branchName;
            InitializeComponent();
            Text = $"Production History - {branchName}";
        }

        private async void frmProductionHistory_Load(object sender, EventArgs e)
        {
            await LoadHistoryAsync();
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            await LoadHistoryAsync();
        }

        private async System.Threading.Tasks.Task LoadHistoryAsync()
        {
            try
            {
                var history = await BranchApiClient.GetProductionHistoryAsync(branchName);
                dgvHistory.DataSource = history;
                dgvHistory.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not load production history.\n\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
