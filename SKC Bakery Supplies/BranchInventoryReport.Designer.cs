namespace SKC_Bakery_Supplies
{
    partial class frmBranchInventoryReport
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
            btnLoad = new System.Windows.Forms.Button();
            btnAdjust = new System.Windows.Forms.Button();
            btnPrint = new System.Windows.Forms.Button();
            dgvStock = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)dgvStock).BeginInit();
            SuspendLayout();
            //
            // lblHeader
            //
            lblHeader.AutoSize = true;
            lblHeader.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            lblHeader.ForeColor = System.Drawing.Color.FromArgb(45, 52, 64);
            lblHeader.Location = new System.Drawing.Point(20, 15);
            lblHeader.Name = "lblHeader";
            lblHeader.Size = new System.Drawing.Size(220, 32);
            lblHeader.TabIndex = 0;
            lblHeader.Text = "Branch Inventory";
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
            cmbBranch.Location = new System.Drawing.Point(90, 62);
            cmbBranch.Name = "cmbBranch";
            cmbBranch.Size = new System.Drawing.Size(220, 28);
            cmbBranch.TabIndex = 2;
            //
            // btnLoad
            //
            btnLoad.BackColor = System.Drawing.Color.White;
            btnLoad.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            btnLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnLoad.Font = new System.Drawing.Font("Segoe UI", 10F);
            btnLoad.Location = new System.Drawing.Point(326, 60);
            btnLoad.Name = "btnLoad";
            btnLoad.Size = new System.Drawing.Size(120, 32);
            btnLoad.TabIndex = 3;
            btnLoad.Text = "Load";
            btnLoad.UseVisualStyleBackColor = false;
            btnLoad.Click += btnLoad_Click;
            //
            // btnAdjust
            //
            btnAdjust.BackColor = System.Drawing.Color.White;
            btnAdjust.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            btnAdjust.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnAdjust.Font = new System.Drawing.Font("Segoe UI", 10F);
            btnAdjust.Location = new System.Drawing.Point(456, 60);
            btnAdjust.Name = "btnAdjust";
            btnAdjust.Size = new System.Drawing.Size(140, 32);
            btnAdjust.TabIndex = 4;
            btnAdjust.Text = "Adjust Stock";
            btnAdjust.UseVisualStyleBackColor = false;
            btnAdjust.Click += btnAdjust_Click;
            //
            // btnPrint
            //
            btnPrint.BackColor = System.Drawing.Color.White;
            btnPrint.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnPrint.Font = new System.Drawing.Font("Segoe UI", 10F);
            btnPrint.Location = new System.Drawing.Point(606, 60);
            btnPrint.Name = "btnPrint";
            btnPrint.Size = new System.Drawing.Size(120, 32);
            btnPrint.TabIndex = 5;
            btnPrint.Text = "Print";
            btnPrint.UseVisualStyleBackColor = false;
            btnPrint.Click += btnPrint_Click;
            //
            // dgvStock
            //
            dgvStock.AllowUserToAddRows = false;
            dgvStock.AllowUserToDeleteRows = false;
            dgvStock.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            dgvStock.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dgvStock.BackgroundColor = System.Drawing.Color.White;
            dgvStock.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvStock.Location = new System.Drawing.Point(20, 105);
            dgvStock.Name = "dgvStock";
            dgvStock.ReadOnly = true;
            dgvStock.RowHeadersVisible = false;
            dgvStock.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvStock.Size = new System.Drawing.Size(1240, 515);
            dgvStock.TabIndex = 6;
            //
            // frmBranchInventoryReport
            //
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.WhiteSmoke;
            ClientSize = new System.Drawing.Size(1280, 640);
            Controls.Add(dgvStock);
            Controls.Add(btnPrint);
            Controls.Add(btnAdjust);
            Controls.Add(btnLoad);
            Controls.Add(cmbBranch);
            Controls.Add(lblBranch);
            Controls.Add(lblHeader);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "frmBranchInventoryReport";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            WindowState = System.Windows.Forms.FormWindowState.Maximized;
            Text = "Branch Inventory Report";
            Load += frmBranchInventoryReport_Load;
            ((System.ComponentModel.ISupportInitialize)dgvStock).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Label lblBranch;
        private System.Windows.Forms.ComboBox cmbBranch;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnAdjust;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.DataGridView dgvStock;
    }
}
