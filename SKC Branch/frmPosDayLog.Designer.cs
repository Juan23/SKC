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
            btnVoid = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)dgvSales).BeginInit();
            SuspendLayout();
            //
            // lblHeader
            //
            lblHeader.AutoSize = true;
            lblHeader.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            lblHeader.Location = new System.Drawing.Point(20, 15);
            lblHeader.Name = "lblHeader";
            lblHeader.Size = new System.Drawing.Size(160, 32);
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
            dgvSales.Location = new System.Drawing.Point(20, 60);
            dgvSales.Name = "dgvSales";
            dgvSales.ReadOnly = true;
            dgvSales.RowHeadersVisible = false;
            dgvSales.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvSales.Size = new System.Drawing.Size(1240, 510);
            dgvSales.TabIndex = 1;
            //
            // lblTotals
            //
            lblTotals.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left
                | System.Windows.Forms.AnchorStyles.Right;
            lblTotals.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            lblTotals.Location = new System.Drawing.Point(20, 590);
            lblTotals.Name = "lblTotals";
            lblTotals.Size = new System.Drawing.Size(780, 30);
            lblTotals.TabIndex = 2;
            lblTotals.Text = "Total: 0.00";
            //
            // btnRefresh
            //
            btnRefresh.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnRefresh.Font = new System.Drawing.Font("Segoe UI", 10F);
            btnRefresh.Location = new System.Drawing.Point(1130, 585);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new System.Drawing.Size(130, 40);
            btnRefresh.TabIndex = 3;
            btnRefresh.Text = "Refresh";
            btnRefresh.UseVisualStyleBackColor = true;
            btnRefresh.Click += btnRefresh_Click;
            //
            // btnVoid
            //
            btnVoid.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnVoid.Font = new System.Drawing.Font("Segoe UI", 10F);
            btnVoid.Location = new System.Drawing.Point(990, 585);
            btnVoid.Name = "btnVoid";
            btnVoid.Size = new System.Drawing.Size(130, 40);
            btnVoid.TabIndex = 4;
            btnVoid.Text = "Void Sale";
            btnVoid.UseVisualStyleBackColor = true;
            btnVoid.Click += btnVoid_Click;
            //
            // frmPosDayLog
            //
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1280, 640);
            Controls.Add(lblHeader);
            Controls.Add(dgvSales);
            Controls.Add(lblTotals);
            Controls.Add(btnRefresh);
            Controls.Add(btnVoid);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "frmPosDayLog";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            WindowState = System.Windows.Forms.FormWindowState.Maximized;
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
        private System.Windows.Forms.Button btnVoid;
    }
}
