using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SKC_Bakery_Supplies
{
    public partial class frmPurchasesReport : Form
    {
        public frmPurchasesReport()
        {
            InitializeComponent();
        }

        private void frmPurchasesReport_Load(object sender, EventArgs e)
        {
            // Default range: 1st of the current month to today
            dtpStart.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtpEnd.Value = DateTime.Now.Date;

            LoadTickets();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            LoadTickets();
        }

        private async void LoadTickets()
        {
            // Fetch tickets from the engine and bind to Grid 1
            List<PurchaseTicketSummary> tickets = await CentralApiClient.GetPurchaseTicketsAsync(dtpStart.Value.Date, dtpEnd.Value.Date);
            dgvTickets.DataSource = tickets;

            // Clear Grid 2 until a new selection is made
            dgvDetails.DataSource = null;
            dgvTickets.ClearSelection(); // remove selection so when user clicks, it triggers "changed"
        }

        // When the user clicks a row in Grid 1, update Grid 2
        private async void dgvTickets_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvTickets.CurrentRow != null && dgvTickets.CurrentRow.DataBoundItem is PurchaseTicketSummary selectedTicket)
            {
                // Fetch the exact breakdown for this specific ticket
                List<PurchaseLog> details = await CentralApiClient.GetPurchaseDetailsAsync(selectedTicket.TransactionId);
                dgvDetails.DataSource = details;

                // Hide system IDs from the user's view in Grid 2
                if (dgvDetails.Columns["Id"] != null) dgvDetails.Columns["Id"].Visible = false;
                if (dgvDetails.Columns["TransactionId"] != null) dgvDetails.Columns["TransactionId"].Visible = false;
            }
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvTickets.CurrentRow == null) return;

            var selectedTicket = (PurchaseTicketSummary)dgvTickets.CurrentRow.DataBoundItem;

            if (MessageBox.Show("Delete this ticket?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    await CentralApiClient.DeletePurchaseTicketAsync(selectedTicket.TransactionId);
                    LoadTickets();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error");
                }
            }
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            LoadTickets();
        }
    }
}
