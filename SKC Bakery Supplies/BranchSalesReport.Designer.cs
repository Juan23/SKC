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
            // lblBranch
            //
            lblBranch.AutoSize = true;
            lblBranch.Location = new System.Drawing.Point(20, 20);
            lblBranch.Name = "lblBranch";
            lblBranch.Size = new System.Drawing.Size(50, 15);
            lblBranch.TabIndex = 0;
            lblBranch.Text = "Branch:";
            //
            // cmbBranch
            //
            cmbBranch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbBranch.FormattingEnabled = true;
            cmbBranch.Location = new System.Drawing.Point(76, 17);
            cmbBranch.Name = "cmbBranch";
            cmbBranch.Size = new System.Drawing.Size(130, 23);
            cmbBranch.TabIndex = 1;
            //
            // lblFrom
            //
            lblFrom.AutoSize = true;
            lblFrom.Location = new System.Drawing.Point(220, 20);
            lblFrom.Name = "lblFrom";
            lblFrom.Size = new System.Drawing.Size(38, 15);
            lblFrom.TabIndex = 2;
            lblFrom.Text = "From:";
            //
            // dtpStart
            //
            dtpStart.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            dtpStart.Location = new System.Drawing.Point(262, 17);
            dtpStart.Name = "dtpStart";
            dtpStart.Size = new System.Drawing.Size(110, 23);
            dtpStart.TabIndex = 3;
            //
            // lblTo
            //
            lblTo.AutoSize = true;
            lblTo.Location = new System.Drawing.Point(382, 20);
            lblTo.Name = "lblTo";
            lblTo.Size = new System.Drawing.Size(22, 15);
            lblTo.TabIndex = 4;
            lblTo.Text = "To:";
            //
            // dtpEnd
            //
            dtpEnd.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            dtpEnd.Location = new System.Drawing.Point(408, 17);
            dtpEnd.Name = "dtpEnd";
            dtpEnd.Size = new System.Drawing.Size(110, 23);
            dtpEnd.TabIndex = 5;
            //
            // btnLoad
            //
            btnLoad.Location = new System.Drawing.Point(532, 16);
            btnLoad.Name = "btnLoad";
            btnLoad.Size = new System.Drawing.Size(90, 25);
            btnLoad.TabIndex = 6;
            btnLoad.Text = "Load";
            btnLoad.UseVisualStyleBackColor = true;
            btnLoad.Click += btnLoad_Click;
            //
            // dgvSales
            //
            dgvSales.AllowUserToAddRows = false;
            dgvSales.AllowUserToDeleteRows = false;
            dgvSales.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom
                | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            dgvSales.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dgvSales.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvSales.Location = new System.Drawing.Point(20, 55);
            dgvSales.Name = "dgvSales";
            dgvSales.ReadOnly = true;
            dgvSales.RowHeadersVisible = false;
            dgvSales.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvSales.Size = new System.Drawing.Size(684, 250);
            dgvSales.TabIndex = 7;
            dgvSales.SelectionChanged += dgvSales_SelectionChanged;
            //
            // lblLinesHeader
            //
            lblLinesHeader.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            lblLinesHeader.AutoSize = true;
            lblLinesHeader.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblLinesHeader.Location = new System.Drawing.Point(20, 315);
            lblLinesHeader.Name = "lblLinesHeader";
            lblLinesHeader.Size = new System.Drawing.Size(103, 15);
            lblLinesHeader.TabIndex = 8;
            lblLinesHeader.Text = "Selected Sale Items";
            //
            // dgvLines
            //
            dgvLines.AllowUserToAddRows = false;
            dgvLines.AllowUserToDeleteRows = false;
            dgvLines.Anchor = System.Windows.Forms.AnchorStyles.Bottom
                | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            dgvLines.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dgvLines.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvLines.Location = new System.Drawing.Point(20, 335);
            dgvLines.Name = "dgvLines";
            dgvLines.ReadOnly = true;
            dgvLines.RowHeadersVisible = false;
            dgvLines.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvLines.Size = new System.Drawing.Size(684, 160);
            dgvLines.TabIndex = 9;
            //
            // lblTotals
            //
            lblTotals.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left
                | System.Windows.Forms.AnchorStyles.Right;
            lblTotals.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            lblTotals.Location = new System.Drawing.Point(20, 505);
            lblTotals.Name = "lblTotals";
            lblTotals.Size = new System.Drawing.Size(684, 20);
            lblTotals.TabIndex = 10;
            lblTotals.Text = "";
            //
            // frmBranchSalesReport
            //
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(724, 535);
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
            MinimumSize = new System.Drawing.Size(740, 500);
            Name = "frmBranchSalesReport";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Branch Sales Report";
            Load += frmBranchSalesReport_Load;
            ((System.ComponentModel.ISupportInitialize)dgvSales).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvLines).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

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
