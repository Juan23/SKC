using System;
using System.Windows.Forms;

namespace SKC_Bakery_Supplies
{
    public partial class frmAdjustmentHistory : Form
    {
        public frmAdjustmentHistory()
        {
            InitializeComponent();
        }

        private void frmAdjustmentHistory_Load(object sender, EventArgs e)
        {
            dtpStart.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtpEnd.Value = DateTime.Now.Date;
            LoadGrid();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            LoadGrid();
        }

        private async void LoadGrid()
        {
            try
            {
                dgvAdjustments.DataSource = await CentralApiClient.GetInventoryAdjustmentsAsync(dtpStart.Value.Date, dtpEnd.Value.Date);

                if (dgvAdjustments.Columns["UnitCost"] is { } unitCostColumn)
                    unitCostColumn.DefaultCellStyle.Format = "N2";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }
    }
}
