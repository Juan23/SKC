using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace SKC_Branch
{
    // Local-only view of today's sales: still-queued (Pending), synced, synced-with-
    // shortfall, and rejected ones - the poor man's Z-report until real cash-drawer
    // sessions exist. Reads only the local store, so it works fully offline.
    public partial class frmPosDayLog : Form
    {
        public frmPosDayLog()
        {
            InitializeComponent();
        }

        private void frmPosDayLog_Load(object sender, EventArgs e)
        {
            RefreshLog();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshLog();
        }

        private void RefreshLog()
        {
            var rows = PosLocalStore.GetDayLog(DateTime.Today);

            dgvSales.DataSource = rows.Select(r => new DayLogDisplay
            {
                Time = r.SoldAt.ToString("HH:mm:ss"),
                Cashier = r.StaffName,
                Total = r.TotalAmount,
                Status = r.Status,
                Note = r.Detail
            }).ToList();
            dgvSales.ClearSelection();

            // Rejected sales never counted server-side, so they're excluded from the total.
            decimal counted = rows.Where(r => r.Status != "Rejected").Sum(r => r.TotalAmount);
            int pending = rows.Count(r => r.Status == "Pending");
            int rejected = rows.Count(r => r.Status == "Rejected");

            lblTotals.Text = $"Total: {counted:N2}   ({rows.Count} sales, {pending} awaiting sync" +
                             (rejected > 0 ? $", {rejected} REJECTED)" : ")");
        }

        private class DayLogDisplay
        {
            [DisplayName("Time")]
            public string Time { get; set; } = string.Empty;

            [DisplayName("Cashier")]
            public string Cashier { get; set; } = string.Empty;

            [DisplayName("Total")]
            public decimal Total { get; set; }

            [DisplayName("Status")]
            public string Status { get; set; } = string.Empty;

            [DisplayName("Note")]
            public string Note { get; set; } = string.Empty;
        }
    }
}
