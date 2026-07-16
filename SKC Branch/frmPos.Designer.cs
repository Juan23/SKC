namespace SKC_Branch
{
    partial class frmPos
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
            components = new System.ComponentModel.Container();
            lblStaff = new System.Windows.Forms.Label();
            txtStaff = new System.Windows.Forms.TextBox();
            lblSearch = new System.Windows.Forms.Label();
            txtSearch = new System.Windows.Forms.TextBox();
            numQty = new System.Windows.Forms.NumericUpDown();
            lstSearch = new System.Windows.Forms.ListBox();
            dgvCart = new System.Windows.Forms.DataGridView();
            btnDiscount = new System.Windows.Forms.Button();
            btnRemoveLine = new System.Windows.Forms.Button();
            btnDayLog = new System.Windows.Forms.Button();
            btnSyncNow = new System.Windows.Forms.Button();
            lblTotalCaption = new System.Windows.Forms.Label();
            lblTotal = new System.Windows.Forms.Label();
            lblCash = new System.Windows.Forms.Label();
            txtCash = new System.Windows.Forms.TextBox();
            lblChange = new System.Windows.Forms.Label();
            btnComplete = new System.Windows.Forms.Button();
            lblLastSale = new System.Windows.Forms.Label();
            statusStrip = new System.Windows.Forms.StatusStrip();
            lblSyncStatus = new System.Windows.Forms.ToolStripStatusLabel();
            syncTimer = new System.Windows.Forms.Timer(components);
            ((System.ComponentModel.ISupportInitialize)numQty).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvCart).BeginInit();
            statusStrip.SuspendLayout();
            SuspendLayout();
            //
            // lblStaff
            //
            lblStaff.AutoSize = true;
            lblStaff.Location = new System.Drawing.Point(15, 18);
            lblStaff.Name = "lblStaff";
            lblStaff.Size = new System.Drawing.Size(48, 15);
            lblStaff.TabIndex = 0;
            lblStaff.Text = "Cashier";
            //
            // txtStaff
            //
            txtStaff.Location = new System.Drawing.Point(80, 15);
            txtStaff.Name = "txtStaff";
            txtStaff.Size = new System.Drawing.Size(200, 23);
            txtStaff.TabIndex = 1;
            //
            // lblSearch
            //
            lblSearch.AutoSize = true;
            lblSearch.Location = new System.Drawing.Point(15, 55);
            lblSearch.Name = "lblSearch";
            lblSearch.Size = new System.Drawing.Size(31, 15);
            lblSearch.TabIndex = 2;
            lblSearch.Text = "Item";
            //
            // txtSearch
            //
            txtSearch.Location = new System.Drawing.Point(80, 52);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new System.Drawing.Size(370, 23);
            txtSearch.TabIndex = 3;
            txtSearch.TextChanged += txtSearch_TextChanged;
            txtSearch.KeyDown += txtSearch_KeyDown;
            //
            // numQty
            //
            numQty.Location = new System.Drawing.Point(460, 52);
            numQty.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            numQty.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numQty.Name = "numQty";
            numQty.Size = new System.Drawing.Size(70, 23);
            numQty.TabIndex = 4;
            numQty.Value = new decimal(new int[] { 1, 0, 0, 0 });
            //
            // lstSearch
            //
            lstSearch.ItemHeight = 15;
            lstSearch.Location = new System.Drawing.Point(80, 78);
            lstSearch.Name = "lstSearch";
            lstSearch.Size = new System.Drawing.Size(450, 139);
            lstSearch.TabIndex = 5;
            lstSearch.Visible = false;
            lstSearch.Click += lstSearch_Click;
            //
            // dgvCart
            //
            dgvCart.AllowUserToAddRows = false;
            dgvCart.AllowUserToDeleteRows = false;
            dgvCart.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom
                | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            dgvCart.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dgvCart.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvCart.Location = new System.Drawing.Point(15, 90);
            dgvCart.Name = "dgvCart";
            dgvCart.ReadOnly = true;
            dgvCart.RowHeadersVisible = false;
            dgvCart.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvCart.Size = new System.Drawing.Size(615, 440);
            dgvCart.TabIndex = 6;
            //
            // btnDiscount
            //
            btnDiscount.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnDiscount.Location = new System.Drawing.Point(645, 90);
            btnDiscount.Name = "btnDiscount";
            btnDiscount.Size = new System.Drawing.Size(240, 32);
            btnDiscount.TabIndex = 7;
            btnDiscount.Text = "Discount...";
            btnDiscount.UseVisualStyleBackColor = true;
            btnDiscount.Click += btnDiscount_Click;
            //
            // btnRemoveLine
            //
            btnRemoveLine.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnRemoveLine.Location = new System.Drawing.Point(645, 130);
            btnRemoveLine.Name = "btnRemoveLine";
            btnRemoveLine.Size = new System.Drawing.Size(240, 32);
            btnRemoveLine.TabIndex = 8;
            btnRemoveLine.Text = "Remove Selected Line";
            btnRemoveLine.UseVisualStyleBackColor = true;
            btnRemoveLine.Click += btnRemoveLine_Click;
            //
            // btnDayLog
            //
            btnDayLog.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnDayLog.Location = new System.Drawing.Point(645, 170);
            btnDayLog.Name = "btnDayLog";
            btnDayLog.Size = new System.Drawing.Size(240, 32);
            btnDayLog.TabIndex = 9;
            btnDayLog.Text = "Today's Sales...";
            btnDayLog.UseVisualStyleBackColor = true;
            btnDayLog.Click += btnDayLog_Click;
            //
            // btnSyncNow
            //
            btnSyncNow.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnSyncNow.Location = new System.Drawing.Point(645, 210);
            btnSyncNow.Name = "btnSyncNow";
            btnSyncNow.Size = new System.Drawing.Size(240, 32);
            btnSyncNow.TabIndex = 10;
            btnSyncNow.Text = "Sync Now";
            btnSyncNow.UseVisualStyleBackColor = true;
            btnSyncNow.Click += btnSyncNow_Click;
            //
            // lblTotalCaption
            //
            lblTotalCaption.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            lblTotalCaption.AutoSize = true;
            lblTotalCaption.Location = new System.Drawing.Point(645, 290);
            lblTotalCaption.Name = "lblTotalCaption";
            lblTotalCaption.Size = new System.Drawing.Size(35, 15);
            lblTotalCaption.TabIndex = 11;
            lblTotalCaption.Text = "Total";
            //
            // lblTotal
            //
            lblTotal.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            lblTotal.Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold);
            lblTotal.Location = new System.Drawing.Point(645, 308);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new System.Drawing.Size(240, 45);
            lblTotal.TabIndex = 12;
            lblTotal.Text = "0.00";
            lblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // lblCash
            //
            lblCash.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            lblCash.AutoSize = true;
            lblCash.Location = new System.Drawing.Point(645, 368);
            lblCash.Name = "lblCash";
            lblCash.Size = new System.Drawing.Size(67, 15);
            lblCash.TabIndex = 13;
            lblCash.Text = "Cash Given";
            //
            // txtCash
            //
            txtCash.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            txtCash.Font = new System.Drawing.Font("Segoe UI", 12F);
            txtCash.Location = new System.Drawing.Point(645, 388);
            txtCash.Name = "txtCash";
            txtCash.Size = new System.Drawing.Size(240, 29);
            txtCash.TabIndex = 14;
            txtCash.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            txtCash.TextChanged += txtCash_TextChanged;
            //
            // lblChange
            //
            lblChange.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            lblChange.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            lblChange.Location = new System.Drawing.Point(645, 424);
            lblChange.Name = "lblChange";
            lblChange.Size = new System.Drawing.Size(240, 30);
            lblChange.TabIndex = 15;
            lblChange.Text = "Change: 0.00";
            lblChange.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // btnComplete
            //
            btnComplete.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnComplete.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            btnComplete.Location = new System.Drawing.Point(645, 465);
            btnComplete.Name = "btnComplete";
            btnComplete.Size = new System.Drawing.Size(240, 60);
            btnComplete.TabIndex = 16;
            btnComplete.Text = "COMPLETE SALE";
            btnComplete.UseVisualStyleBackColor = true;
            btnComplete.Click += btnComplete_Click;
            //
            // lblLastSale
            //
            lblLastSale.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            lblLastSale.Location = new System.Drawing.Point(645, 530);
            lblLastSale.Name = "lblLastSale";
            lblLastSale.Size = new System.Drawing.Size(240, 20);
            lblLastSale.TabIndex = 17;
            lblLastSale.Text = "";
            lblLastSale.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // statusStrip
            //
            statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { lblSyncStatus });
            statusStrip.Location = new System.Drawing.Point(0, 553);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new System.Drawing.Size(900, 22);
            statusStrip.TabIndex = 18;
            //
            // lblSyncStatus
            //
            lblSyncStatus.Name = "lblSyncStatus";
            lblSyncStatus.Size = new System.Drawing.Size(100, 17);
            lblSyncStatus.Text = "Starting...";
            //
            // syncTimer
            //
            syncTimer.Interval = 60000;
            syncTimer.Tick += syncTimer_Tick;
            //
            // frmPos
            //
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(900, 575);
            Controls.Add(lstSearch);
            Controls.Add(lblStaff);
            Controls.Add(txtStaff);
            Controls.Add(lblSearch);
            Controls.Add(txtSearch);
            Controls.Add(numQty);
            Controls.Add(dgvCart);
            Controls.Add(btnDiscount);
            Controls.Add(btnRemoveLine);
            Controls.Add(btnDayLog);
            Controls.Add(btnSyncNow);
            Controls.Add(lblTotalCaption);
            Controls.Add(lblTotal);
            Controls.Add(lblCash);
            Controls.Add(txtCash);
            Controls.Add(lblChange);
            Controls.Add(btnComplete);
            Controls.Add(lblLastSale);
            Controls.Add(statusStrip);
            MinimumSize = new System.Drawing.Size(916, 614);
            Name = "frmPos";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Point of Sale";
            Load += frmPos_Load;
            ((System.ComponentModel.ISupportInitialize)numQty).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvCart).EndInit();
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblStaff;
        private System.Windows.Forms.TextBox txtStaff;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.NumericUpDown numQty;
        private System.Windows.Forms.ListBox lstSearch;
        private System.Windows.Forms.DataGridView dgvCart;
        private System.Windows.Forms.Button btnDiscount;
        private System.Windows.Forms.Button btnRemoveLine;
        private System.Windows.Forms.Button btnDayLog;
        private System.Windows.Forms.Button btnSyncNow;
        private System.Windows.Forms.Label lblTotalCaption;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label lblCash;
        private System.Windows.Forms.TextBox txtCash;
        private System.Windows.Forms.Label lblChange;
        private System.Windows.Forms.Button btnComplete;
        private System.Windows.Forms.Label lblLastSale;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel lblSyncStatus;
        private System.Windows.Forms.Timer syncTimer;
    }
}
