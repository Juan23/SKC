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
            lblBranch = new System.Windows.Forms.Label();
            cmbBranch = new System.Windows.Forms.ComboBox();
            btnLoad = new System.Windows.Forms.Button();
            btnAdjust = new System.Windows.Forms.Button();
            btnPrint = new System.Windows.Forms.Button();
            dgvStock = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)dgvStock).BeginInit();
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
            cmbBranch.Size = new System.Drawing.Size(160, 23);
            cmbBranch.TabIndex = 1;
            //
            // btnLoad
            //
            btnLoad.Location = new System.Drawing.Point(246, 16);
            btnLoad.Name = "btnLoad";
            btnLoad.Size = new System.Drawing.Size(90, 25);
            btnLoad.TabIndex = 2;
            btnLoad.Text = "Load";
            btnLoad.UseVisualStyleBackColor = true;
            btnLoad.Click += btnLoad_Click;
            //
            // btnAdjust
            //
            btnAdjust.Location = new System.Drawing.Point(342, 16);
            btnAdjust.Name = "btnAdjust";
            btnAdjust.Size = new System.Drawing.Size(100, 25);
            btnAdjust.TabIndex = 3;
            btnAdjust.Text = "Adjust Stock";
            btnAdjust.UseVisualStyleBackColor = true;
            btnAdjust.Click += btnAdjust_Click;
            //
            // btnPrint
            //
            btnPrint.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnPrint.Location = new System.Drawing.Point(614, 16);
            btnPrint.Name = "btnPrint";
            btnPrint.Size = new System.Drawing.Size(90, 25);
            btnPrint.TabIndex = 4;
            btnPrint.Text = "Print";
            btnPrint.UseVisualStyleBackColor = true;
            btnPrint.Click += btnPrint_Click;
            //
            // dgvStock
            //
            dgvStock.AllowUserToAddRows = false;
            dgvStock.AllowUserToDeleteRows = false;
            dgvStock.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            dgvStock.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dgvStock.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvStock.Location = new System.Drawing.Point(20, 55);
            dgvStock.Name = "dgvStock";
            dgvStock.ReadOnly = true;
            dgvStock.RowHeadersVisible = false;
            dgvStock.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvStock.Size = new System.Drawing.Size(684, 460);
            dgvStock.TabIndex = 4;
            //
            // frmBranchInventoryReport
            //
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(724, 535);
            Controls.Add(dgvStock);
            Controls.Add(btnPrint);
            Controls.Add(btnAdjust);
            Controls.Add(btnLoad);
            Controls.Add(cmbBranch);
            Controls.Add(lblBranch);
            MinimumSize = new System.Drawing.Size(600, 400);
            Name = "frmBranchInventoryReport";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Branch Inventory Report";
            Load += frmBranchInventoryReport_Load;
            ((System.ComponentModel.ISupportInitialize)dgvStock).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblBranch;
        private System.Windows.Forms.ComboBox cmbBranch;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnAdjust;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.DataGridView dgvStock;
    }
}
