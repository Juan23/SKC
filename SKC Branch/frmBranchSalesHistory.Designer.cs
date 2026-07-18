namespace SKC_Branch
{
    partial class frmBranchSalesHistory
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
            btnLoad = new System.Windows.Forms.Button();
            dgvSales = new System.Windows.Forms.DataGridView();
            dgvLines = new System.Windows.Forms.DataGridView();
            lblTotals = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)dgvSales).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvLines).BeginInit();
            SuspendLayout();
            //
            // lblHeader
            //
            lblHeader.AutoSize = true;
            lblHeader.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            lblHeader.Location = new System.Drawing.Point(20, 15);
            lblHeader.Name = "lblHeader";
            lblHeader.Size = new System.Drawing.Size(150, 32);
            lblHeader.TabIndex = 0;
            lblHeader.Text = "Sales History";
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
            //
            // btnLoad
            //
            btnLoad.Font = new System.Drawing.Font("Segoe UI", 10F);
            btnLoad.Location = new System.Drawing.Point(440, 56);
            btnLoad.Name = "btnLoad";
            btnLoad.Size = new System.Drawing.Size(110, 30);
            btnLoad.TabIndex = 5;
            btnLoad.Text = "Load";
            btnLoad.UseVisualStyleBackColor = true;
            btnLoad.Click += btnLoad_Click;
            //
            // dgvSales
            //
            dgvSales.AllowUserToAddRows = false;
            dgvSales.AllowUserToDeleteRows = false;
            dgvSales.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom
                | System.Windows.Forms.AnchorStyles.Left;
            dgvSales.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dgvSales.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvSales.Location = new System.Drawing.Point(20, 100);
            dgvSales.Name = "dgvSales";
            dgvSales.ReadOnly = true;
            dgvSales.RowHeadersVisible = false;
            dgvSales.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvSales.Size = new System.Drawing.Size(740, 460);
            dgvSales.TabIndex = 6;
            dgvSales.SelectionChanged += dgvSales_SelectionChanged;
            //
            // dgvLines
            //
            dgvLines.AllowUserToAddRows = false;
            dgvLines.AllowUserToDeleteRows = false;
            dgvLines.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom
                | System.Windows.Forms.AnchorStyles.Right;
            dgvLines.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dgvLines.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvLines.Location = new System.Drawing.Point(775, 100);
            dgvLines.Name = "dgvLines";
            dgvLines.ReadOnly = true;
            dgvLines.RowHeadersVisible = false;
            dgvLines.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvLines.Size = new System.Drawing.Size(485, 460);
            dgvLines.TabIndex = 7;
            //
            // lblTotals
            //
            lblTotals.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            lblTotals.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            lblTotals.Location = new System.Drawing.Point(20, 575);
            lblTotals.Name = "lblTotals";
            lblTotals.Size = new System.Drawing.Size(900, 30);
            lblTotals.TabIndex = 8;
            lblTotals.Text = "";
            //
            // frmBranchSalesHistory
            //
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1280, 640);
            Controls.Add(lblHeader);
            Controls.Add(lblFrom);
            Controls.Add(dtpStart);
            Controls.Add(lblTo);
            Controls.Add(dtpEnd);
            Controls.Add(btnLoad);
            Controls.Add(dgvSales);
            Controls.Add(dgvLines);
            Controls.Add(lblTotals);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "frmBranchSalesHistory";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            WindowState = System.Windows.Forms.FormWindowState.Maximized;
            Text = "Sales History";
            Load += frmBranchSalesHistory_Load;
            ((System.ComponentModel.ISupportInitialize)dgvSales).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvLines).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.DateTimePicker dtpStart;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.DateTimePicker dtpEnd;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.DataGridView dgvSales;
        private System.Windows.Forms.DataGridView dgvLines;
        private System.Windows.Forms.Label lblTotals;
    }
}
