namespace SKC_Branch
{
    partial class frmSalesReport
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
            lblFrom = new System.Windows.Forms.Label();
            dtpStart = new System.Windows.Forms.DateTimePicker();
            lblTo = new System.Windows.Forms.Label();
            dtpEnd = new System.Windows.Forms.DateTimePicker();
            btnToday = new System.Windows.Forms.Button();
            btnLoad = new System.Windows.Forms.Button();
            btnPrint = new System.Windows.Forms.Button();
            btnExportCsv = new System.Windows.Forms.Button();
            lblOffline = new System.Windows.Forms.Label();
            lblStale = new System.Windows.Forms.Label();
            dgvSales = new System.Windows.Forms.DataGridView();
            lblTotals = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)dgvSales).BeginInit();
            SuspendLayout();
            //
            // lblHeader
            //
            lblHeader.AutoSize = true;
            lblHeader.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            lblHeader.ForeColor = System.Drawing.Color.FromArgb(45, 52, 64);
            lblHeader.Location = new System.Drawing.Point(20, 15);
            lblHeader.Name = "lblHeader";
            lblHeader.Size = new System.Drawing.Size(260, 32);
            lblHeader.TabIndex = 0;
            lblHeader.Text = "Sales Report";
            //
            // lblFrom
            //
            lblFrom.AutoSize = true;
            lblFrom.Font = new System.Drawing.Font("Segoe UI", 10F);
            lblFrom.Location = new System.Drawing.Point(20, 63);
            lblFrom.Name = "lblFrom";
            lblFrom.Size = new System.Drawing.Size(44, 19);
            lblFrom.TabIndex = 1;
            lblFrom.Text = "From:";
            //
            // dtpStart
            //
            dtpStart.Font = new System.Drawing.Font("Segoe UI", 10F);
            dtpStart.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            dtpStart.Location = new System.Drawing.Point(70, 58);
            dtpStart.Name = "dtpStart";
            dtpStart.Size = new System.Drawing.Size(150, 25);
            dtpStart.TabIndex = 2;
            dtpStart.ValueChanged += DateChanged;
            //
            // lblTo
            //
            lblTo.AutoSize = true;
            lblTo.Font = new System.Drawing.Font("Segoe UI", 10F);
            lblTo.Location = new System.Drawing.Point(235, 63);
            lblTo.Name = "lblTo";
            lblTo.Size = new System.Drawing.Size(26, 19);
            lblTo.TabIndex = 3;
            lblTo.Text = "To:";
            //
            // dtpEnd
            //
            dtpEnd.Font = new System.Drawing.Font("Segoe UI", 10F);
            dtpEnd.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            dtpEnd.Location = new System.Drawing.Point(270, 58);
            dtpEnd.Name = "dtpEnd";
            dtpEnd.Size = new System.Drawing.Size(150, 25);
            dtpEnd.TabIndex = 4;
            dtpEnd.ValueChanged += DateChanged;
            //
            // btnToday
            //
            btnToday.BackColor = System.Drawing.Color.White;
            btnToday.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            btnToday.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnToday.Font = new System.Drawing.Font("Segoe UI", 10F);
            btnToday.Location = new System.Drawing.Point(440, 56);
            btnToday.Name = "btnToday";
            btnToday.Size = new System.Drawing.Size(120, 32);
            btnToday.TabIndex = 5;
            btnToday.Text = "Today";
            btnToday.UseVisualStyleBackColor = false;
            btnToday.Click += btnToday_Click;
            //
            // btnLoad
            //
            btnLoad.BackColor = System.Drawing.Color.White;
            btnLoad.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            btnLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnLoad.Font = new System.Drawing.Font("Segoe UI", 10F);
            btnLoad.Location = new System.Drawing.Point(570, 56);
            btnLoad.Name = "btnLoad";
            btnLoad.Size = new System.Drawing.Size(120, 32);
            btnLoad.TabIndex = 6;
            btnLoad.Text = "Load";
            btnLoad.UseVisualStyleBackColor = false;
            btnLoad.Click += btnLoad_Click;
            //
            // btnPrint
            //
            btnPrint.BackColor = System.Drawing.Color.White;
            btnPrint.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnPrint.Font = new System.Drawing.Font("Segoe UI", 10F);
            btnPrint.Location = new System.Drawing.Point(700, 56);
            btnPrint.Name = "btnPrint";
            btnPrint.Size = new System.Drawing.Size(120, 32);
            btnPrint.TabIndex = 7;
            btnPrint.Text = "Print...";
            btnPrint.UseVisualStyleBackColor = false;
            btnPrint.Click += btnPrint_Click;
            //
            // btnExportCsv
            //
            btnExportCsv.BackColor = System.Drawing.Color.White;
            btnExportCsv.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            btnExportCsv.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnExportCsv.Font = new System.Drawing.Font("Segoe UI", 10F);
            btnExportCsv.Location = new System.Drawing.Point(830, 56);
            btnExportCsv.Name = "btnExportCsv";
            btnExportCsv.Size = new System.Drawing.Size(150, 32);
            btnExportCsv.TabIndex = 8;
            btnExportCsv.Text = "Save for Excel...";
            btnExportCsv.UseVisualStyleBackColor = false;
            btnExportCsv.Click += btnExportCsv_Click;
            //
            // lblOffline
            //
            lblOffline.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            lblOffline.ForeColor = System.Drawing.Color.Firebrick;
            lblOffline.Location = new System.Drawing.Point(995, 62);
            lblOffline.Name = "lblOffline";
            lblOffline.Size = new System.Drawing.Size(265, 25);
            lblOffline.TabIndex = 9;
            lblOffline.Text = "";
            lblOffline.Visible = false;
            //
            // lblStale
            //
            lblStale.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            lblStale.ForeColor = System.Drawing.Color.Firebrick;
            lblStale.Location = new System.Drawing.Point(995, 22);
            lblStale.Name = "lblStale";
            lblStale.Size = new System.Drawing.Size(265, 25);
            lblStale.TabIndex = 12;
            lblStale.Text = "";
            lblStale.Visible = false;
            //
            // dgvSales
            //
            dgvSales.AllowUserToAddRows = false;
            dgvSales.AllowUserToDeleteRows = false;
            dgvSales.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom
                | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            dgvSales.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dgvSales.BackgroundColor = System.Drawing.Color.White;
            dgvSales.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvSales.Location = new System.Drawing.Point(20, 100);
            dgvSales.Name = "dgvSales";
            dgvSales.ReadOnly = true;
            dgvSales.RowHeadersVisible = false;
            dgvSales.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvSales.Size = new System.Drawing.Size(1240, 460);
            dgvSales.TabIndex = 10;
            //
            // lblTotals
            //
            lblTotals.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            lblTotals.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            lblTotals.Location = new System.Drawing.Point(20, 575);
            lblTotals.Name = "lblTotals";
            lblTotals.Size = new System.Drawing.Size(1240, 30);
            lblTotals.TabIndex = 11;
            lblTotals.Text = "";
            //
            // frmSalesReport
            //
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.WhiteSmoke;
            ClientSize = new System.Drawing.Size(1280, 640);
            Controls.Add(lblHeader);
            Controls.Add(lblFrom);
            Controls.Add(dtpStart);
            Controls.Add(lblTo);
            Controls.Add(dtpEnd);
            Controls.Add(btnToday);
            Controls.Add(btnLoad);
            Controls.Add(btnPrint);
            Controls.Add(btnExportCsv);
            Controls.Add(lblOffline);
            Controls.Add(lblStale);
            Controls.Add(dgvSales);
            Controls.Add(lblTotals);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "frmSalesReport";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            WindowState = System.Windows.Forms.FormWindowState.Maximized;
            Text = "Sales Report";
            Load += frmSalesReport_Load;
            ((System.ComponentModel.ISupportInitialize)dgvSales).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.DateTimePicker dtpStart;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.DateTimePicker dtpEnd;
        private System.Windows.Forms.Button btnToday;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnExportCsv;
        private System.Windows.Forms.Label lblOffline;
        private System.Windows.Forms.Label lblStale;
        private System.Windows.Forms.DataGridView dgvSales;
        private System.Windows.Forms.Label lblTotals;
    }
}
