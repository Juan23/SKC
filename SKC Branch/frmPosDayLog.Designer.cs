namespace SKC_Branch
{
    partial class frmPosDayLog
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            lblHeader = new System.Windows.Forms.Label();
            dgvSales = new System.Windows.Forms.DataGridView();
            lblTotals = new System.Windows.Forms.Label();
            btnRefresh = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)dgvSales).BeginInit();
            SuspendLayout();
            //
            // lblHeader
            //
            lblHeader.AutoSize = true;
            lblHeader.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            lblHeader.Location = new System.Drawing.Point(15, 15);
            lblHeader.Name = "lblHeader";
            lblHeader.Size = new System.Drawing.Size(120, 21);
            lblHeader.TabIndex = 0;
            lblHeader.Text = "Today's Sales";
            //
            // dgvSales
            //
            dgvSales.AllowUserToAddRows = false;
            dgvSales.AllowUserToDeleteRows = false;
            dgvSales.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom
                | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            dgvSales.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dgvSales.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvSales.Location = new System.Drawing.Point(15, 45);
            dgvSales.Name = "dgvSales";
            dgvSales.ReadOnly = true;
            dgvSales.RowHeadersVisible = false;
            dgvSales.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvSales.Size = new System.Drawing.Size(670, 330);
            dgvSales.TabIndex = 1;
            //
            // lblTotals
            //
            lblTotals.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left
                | System.Windows.Forms.AnchorStyles.Right;
            lblTotals.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            lblTotals.Location = new System.Drawing.Point(15, 385);
            lblTotals.Name = "lblTotals";
            lblTotals.Size = new System.Drawing.Size(550, 25);
            lblTotals.TabIndex = 2;
            lblTotals.Text = "Total: 0.00";
            //
            // btnRefresh
            //
            btnRefresh.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnRefresh.Location = new System.Drawing.Point(575, 382);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new System.Drawing.Size(110, 30);
            btnRefresh.TabIndex = 3;
            btnRefresh.Text = "Refresh";
            btnRefresh.UseVisualStyleBackColor = true;
            btnRefresh.Click += btnRefresh_Click;
            //
            // frmPosDayLog
            //
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(700, 425);
            Controls.Add(lblHeader);
            Controls.Add(dgvSales);
            Controls.Add(lblTotals);
            Controls.Add(btnRefresh);
            MinimumSize = new System.Drawing.Size(600, 380);
            Name = "frmPosDayLog";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Today's Sales";
            Load += frmPosDayLog_Load;
            ((System.ComponentModel.ISupportInitialize)dgvSales).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.DataGridView dgvSales;
        private System.Windows.Forms.Label lblTotals;
        private System.Windows.Forms.Button btnRefresh;
    }
}
