using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace SKC_Bakery_Supplies
{
    // Local-only view of today's office-counter sales: still-queued (Pending), synced, synced-
    // with-shortfall, and rejected ones - the poor man's Z-report until real cash-drawer
    // sessions exist. Reads only the local store, so it works fully offline. A synced
    // sale can be voided here (server-authoritative, online-only): the server restocks
    // what it consumed and flags the sale, then the local log is marked Voided too.
    public partial class frmPosDayLog : Form
    {
        private readonly string branchName;

        public frmPosDayLog(string branchName)
        {
            this.branchName = branchName;
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
                ClientSaleId = r.ClientSaleId,
                Time = r.SoldAt.ToString("HH:mm:ss"),
                Cashier = r.StaffName,
                Total = r.TotalAmount,
                Status = r.Status,
                Note = r.Detail
            }).ToList();

            if (dgvSales.Columns["ClientSaleId"] != null) dgvSales.Columns["ClientSaleId"].Visible = false;
            if (dgvSales.Columns["Total"] is { } totalColumn) totalColumn.DefaultCellStyle.Format = "N2";
            dgvSales.ClearSelection();

            // Rejected sales never counted server-side and voided sales were reversed, so both
            // are excluded from the day total.
            decimal counted = rows.Where(r => r.Status != "Rejected" && r.Status != "Voided").Sum(r => r.TotalAmount);
            int pending = rows.Count(r => r.Status == "Pending");
            int rejected = rows.Count(r => r.Status == "Rejected");
            int voided = rows.Count(r => r.Status == "Voided");

            lblTotals.Text = $"Total: {counted:N2}   ({rows.Count} sales, {pending} awaiting sync" +
                             (rejected > 0 ? $", {rejected} REJECTED" : "") +
                             (voided > 0 ? $", {voided} voided" : "") + ")";
        }

        // Void a synced sale. Requires connectivity - the sale must already exist on the server
        // (Pending sales aren't there yet; Rejected ones never counted; Voided ones are done).
        private async void btnVoid_Click(object sender, EventArgs e)
        {
            if (dgvSales.CurrentRow?.DataBoundItem is not DayLogDisplay row)
            {
                MessageBox.Show("Select a sale to void first.", "No Sale Selected",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (row.Status == "Pending")
            {
                MessageBox.Show("This sale hasn't synced to the server yet. Press Sync Now on the POS first, then void it.",
                    "Not Synced Yet", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (row.Status == "Rejected")
            {
                MessageBox.Show("A rejected sale never counted, so there's nothing to void.",
                    "Nothing to Void", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (row.Status == "Voided")
            {
                MessageBox.Show("This sale is already voided.", "Already Voided",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string voidedBy = PromptForVoidedBy();
            if (string.IsNullOrWhiteSpace(voidedBy)) return;

            var confirm = MessageBox.Show(
                $"Void this {row.Total:N2} sale by {row.Cashier}?\n\nIts stock will be returned. This cannot be undone.",
                "Confirm Void", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;

            btnVoid.Enabled = false;
            try
            {
                await CentralApiClient.VoidSaleAsync(branchName, row.ClientSaleId, voidedBy.Trim());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not void the sale - are you online?\n\n{ex.Message}",
                    "Void Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            finally
            {
                btnVoid.Enabled = true;
            }

            // The server void succeeded and is the source of truth. Reflect it in the local day log
            // best-effort - a local write failure here is only cosmetic (the sales history reads
            // the server's authoritative voided flag), so it must NOT be reported as a void
            // failure the way a network error is.
            try { PosLocalStore.MarkSaleVoided(row.ClientSaleId, voidedBy.Trim()); } catch { /* cosmetic */ }
            RefreshLog();
        }

        private static string PromptForVoidedBy()
        {
            using var prompt = new Form
            {
                Text = "Void Sale",
                Width = 340,
                Height = 180,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterParent,
                MaximizeBox = false,
                MinimizeBox = false
            };
            var lbl = new Label { Left = 20, Top = 15, Width = 290, Text = "Who is authorizing this void?" };
            var txt = new TextBox { Left = 20, Top = 40, Width = 285 };
            var btn = new Button
            {
                Text = "Void Sale",
                Left = 20,
                Top = 75,
                Width = 285,
                Height = 35,
                DialogResult = DialogResult.OK
            };
            prompt.Controls.Add(lbl);
            prompt.Controls.Add(txt);
            prompt.Controls.Add(btn);
            prompt.AcceptButton = btn;

            return prompt.ShowDialog() == DialogResult.OK ? txt.Text.Trim() : "";
        }

        private class DayLogDisplay
        {
            [Browsable(false)]
            public string ClientSaleId { get; set; } = string.Empty;

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
