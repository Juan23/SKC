namespace SKC_Bakery_Supplies
{
    partial class frmBranchSalesReport
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblHeader = new System.Windows.Forms.Label();
            lblBranch = new System.Windows.Forms.Label();
            cmbBranch = new System.Windows.Forms.ComboBox();
            lblFrom = new System.Windows.Forms.Label();
            dtpStart = new System.Windows.Forms.DateTimePicker();
            lblTo = new System.Windows.Forms.Label();
            dtpEnd = new System.Windows.Forms.DateTimePicker();
            btnLoad = new System.Windows.Forms.Button();
            dgvSales = new System.Windows.Forms.DataGridView();
            lblLinesHeader = new System.Windows.Forms.Label();
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
            lblHeader.ForeColor = System.Drawing.Color.FromArgb(45, 52, 64);
            lblHeader.Location = new System.Drawing.Point(20, 15);
            lblHeader.Name = "lblHeader";
            lblHeader.Size = new System.Drawing.Size(180, 32);
            lblHeader.TabIndex = 0;
            lblHeader.Text = "Branch Sales";
            //
            // lblBranch
            //
            lblBranch.AutoSize = true;
            lblBranch.Font = new System.Drawing.Font("Segoe UI", 10F);
            lblBranch.Location = new System.Drawing.Point(20, 66);
            lblBranch.Name = "lblBranch";
            lblBranch.Size = new System.Drawing.Size(58, 20);
            lblBranch.TabIndex = 1;
            lblBranch.Text = "Branch:";
            //
            // cmbBranch
            //
            cmbBranch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbBranch.Font = new System.Drawing.Font("Segoe UI", 10F);
            cmbBranch.FormattingEnabled = true;
            cmbBranch.Location = new System.Drawing.Point(86, 62);
            cmbBranch.Name = "cmbBranch";
            cmbBranch.Size = new System.Drawing.Size(180, 28);
            cmbBranch.TabIndex = 2;
            //
            // lblFrom
            //
            lblFrom.AutoSize = true;
            lblFrom.Font = new System.Drawing.Font("Segoe UI", 10F);
            lblFrom.Location = new System.Drawing.Point(284, 66);
            lblFrom.Name = "lblFrom";
            lblFrom.Size = new System.Drawing.Size(46, 20);
            lblFrom.TabIndex = 3;
            lblFrom.Text = "From:";
            //
            // dtpStart
            //
            dtpStart.Font = new System.Drawing.Font("Segoe UI", 10F);
            dtpStart.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            dtpStart.Location = new System.Drawing.Point(336, 62);
            dtpStart.Name = "dtpStart";
            dtpStart.Size = new System.Drawing.Size(140, 28);
            dtpStart.TabIndex = 4;
            //
            // lblTo
            //
            lblTo.AutoSize = true;
            lblTo.Font = new System.Drawing.Font("Segoe UI", 10F);
            lblTo.Location = new System.Drawing.Point(492, 66);
            lblTo.Name = "lblTo";
            lblTo.Size = new System.Drawing.Size(28, 20);
            lblTo.TabIndex = 5;
            lblTo.Text = "To:";
            //
            // dtpEnd
            //
            dtpEnd.Font = new System.Drawing.Font("Segoe UI", 10F);
            dtpEnd.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            dtpEnd.Location = new System.Drawing.Point(526, 62);
            dtpEnd.Name = "dtpEnd";
            dtpEnd.Size = new System.Drawing.Size(140, 28);
            dtpEnd.TabIndex = 6;
            //
            // btnLoad
            //
            btnLoad.BackColor = System.Drawing.Color.White;
            btnLoad.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            btnLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnLoad.Font = new System.Drawing.Font("Segoe UI", 10F);
            btnLoad.Location = new System.Drawing.Point(686, 60);
            btnLoad.Name = "btnLoad";
            btnLoad.Size = new System.Drawing.Size(120, 32);
            btnLoad.TabIndex = 7;
            btnLoad.Text = "Load";
            btnLoad.UseVisualStyleBackColor = false;
            btnLoad.Click += btnLoad_Click;
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
            dgvSales.Location = new System.Drawing.Point(20, 105);
            dgvSales.Name = "dgvSales";
            dgvSales.ReadOnly = true;
            dgvSales.RowHeadersVisible = false;
            dgvSales.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvSales.Size = new System.Drawing.Size(1240, 300);
            dgvSales.TabIndex = 8;
            dgvSales.SelectionChanged += dgvSales_SelectionChanged;
            //
            // lblLinesHeader
            //
            lblLinesHeader.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            lblLinesHeader.AutoSize = true;
            lblLinesHeader.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            lblLinesHeader.Location = new System.Drawing.Point(20, 416);
            lblLinesHeader.Name = "lblLinesHeader";
            lblLinesHeader.Size = new System.Drawing.Size(140, 20);
            lblLinesHeader.TabIndex = 9;
            lblLinesHeader.Text = "Selected Sale Items";
            //
            // dgvLines
            //
            dgvLines.AllowUserToAddRows = false;
            dgvLines.AllowUserToDeleteRows = false;
            dgvLines.Anchor = System.Windows.Forms.AnchorStyles.Bottom
                | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            dgvLines.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dgvLines.BackgroundColor = System.Drawing.Color.White;
            dgvLines.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvLines.Location = new System.Drawing.Point(20, 440);
            dgvLines.Name = "dgvLines";
            dgvLines.ReadOnly = true;
            dgvLines.RowHeadersVisible = false;
            dgvLines.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvLines.Size = new System.Drawing.Size(1240, 165);
            dgvLines.TabIndex = 10;
            //
            // lblTotals
            //
            lblTotals.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left
                | System.Windows.Forms.AnchorStyles.Right;
            lblTotals.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            lblTotals.Location = new System.Drawing.Point(20, 612);
            lblTotals.Name = "lblTotals";
            lblTotals.Size = new System.Drawing.Size(1240, 22);
            lblTotals.TabIndex = 11;
            lblTotals.Text = "";
            //
            // frmBranchSalesReport
            //
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.WhiteSmoke;
            ClientSize = new System.Drawing.Size(1280, 640);
            Controls.Add(lblHeader);
            Controls.Add(lblBranch);
            Controls.Add(cmbBranch);
            Controls.Add(lblFrom);
            Controls.Add(dtpStart);
            Controls.Add(lblTo);
            Controls.Add(dtpEnd);
            Controls.Add(btnLoad);
            Controls.Add(dgvSales);
            Controls.Add(lblLinesHeader);
            Controls.Add(dgvLines);
            Controls.Add(lblTotals);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "frmBranchSalesReport";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            WindowState = System.Windows.Forms.FormWindowState.Maximized;
            Text = "Branch Sales Report";
            Load += frmBranchSalesReport_Load;
            ((System.ComponentModel.ISupportInitialize)dgvSales).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvLines).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Label lblBranch;
        private System.Windows.Forms.ComboBox cmbBranch;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.DateTimePicker dtpStart;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.DateTimePicker dtpEnd;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.DataGridView dgvSales;
        private System.Windows.Forms.Label lblLinesHeader;
        private System.Windows.Forms.DataGridView dgvLines;
        private System.Windows.Forms.Label lblTotals;
    }
}
