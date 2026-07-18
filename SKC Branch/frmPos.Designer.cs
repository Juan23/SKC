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
            btnGoDeliveries = new System.Windows.Forms.Button();
            btnGoMyStock = new System.Windows.Forms.Button();
            btnGoProduction = new System.Windows.Forms.Button();
            btnGoSalesHistory = new System.Windows.Forms.Button();
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
            lblStaff.Font = new System.Drawing.Font("Segoe UI", 10F);
            lblStaff.Location = new System.Drawing.Point(20, 27);
            lblStaff.Name = "lblStaff";
            lblStaff.Size = new System.Drawing.Size(62, 23);
            lblStaff.TabIndex = 0;
            lblStaff.Text = "Cashier";
            //
            // txtStaff
            //
            txtStaff.Font = new System.Drawing.Font("Segoe UI", 12F);
            txtStaff.Location = new System.Drawing.Point(95, 20);
            txtStaff.Name = "txtStaff";
            txtStaff.Size = new System.Drawing.Size(225, 29);
            txtStaff.TabIndex = 1;
            //
            // btnGoDeliveries
            //
            btnGoDeliveries.Font = new System.Drawing.Font("Segoe UI", 10F);
            btnGoDeliveries.Location = new System.Drawing.Point(330, 15);
            btnGoDeliveries.Name = "btnGoDeliveries";
            btnGoDeliveries.Size = new System.Drawing.Size(140, 40);
            btnGoDeliveries.TabIndex = 2;
            btnGoDeliveries.Text = "Deliveries";
            btnGoDeliveries.UseVisualStyleBackColor = true;
            btnGoDeliveries.Click += btnGoDeliveries_Click;
            //
            // btnGoMyStock
            //
            btnGoMyStock.Font = new System.Drawing.Font("Segoe UI", 10F);
            btnGoMyStock.Location = new System.Drawing.Point(480, 15);
            btnGoMyStock.Name = "btnGoMyStock";
            btnGoMyStock.Size = new System.Drawing.Size(140, 40);
            btnGoMyStock.TabIndex = 3;
            btnGoMyStock.Text = "My Stock";
            btnGoMyStock.UseVisualStyleBackColor = true;
            btnGoMyStock.Click += btnGoMyStock_Click;
            //
            // btnGoProduction
            //
            btnGoProduction.Font = new System.Drawing.Font("Segoe UI", 10F);
            btnGoProduction.Location = new System.Drawing.Point(630, 15);
            btnGoProduction.Name = "btnGoProduction";
            btnGoProduction.Size = new System.Drawing.Size(140, 40);
            btnGoProduction.TabIndex = 4;
            btnGoProduction.Text = "Bake / Decorate";
            btnGoProduction.UseVisualStyleBackColor = true;
            btnGoProduction.Click += btnGoProduction_Click;
            //
            // btnGoSalesHistory
            //
            btnGoSalesHistory.Font = new System.Drawing.Font("Segoe UI", 10F);
            btnGoSalesHistory.Location = new System.Drawing.Point(780, 15);
            btnGoSalesHistory.Name = "btnGoSalesHistory";
            btnGoSalesHistory.Size = new System.Drawing.Size(140, 40);
            btnGoSalesHistory.TabIndex = 5;
            btnGoSalesHistory.Text = "Sales History";
            btnGoSalesHistory.UseVisualStyleBackColor = true;
            btnGoSalesHistory.Click += btnGoSalesHistory_Click;
            //
            // lblSearch
            //
            lblSearch.AutoSize = true;
            lblSearch.Font = new System.Drawing.Font("Segoe UI", 10F);
            lblSearch.Location = new System.Drawing.Point(20, 72);
            lblSearch.Name = "lblSearch";
            lblSearch.Size = new System.Drawing.Size(40, 23);
            lblSearch.TabIndex = 5;
            lblSearch.Text = "Item";
            //
            // txtSearch
            //
            txtSearch.Font = new System.Drawing.Font("Segoe UI", 12F);
            txtSearch.Location = new System.Drawing.Point(95, 65);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new System.Drawing.Size(610, 29);
            txtSearch.TabIndex = 6;
            txtSearch.TextChanged += txtSearch_TextChanged;
            txtSearch.KeyDown += txtSearch_KeyDown;
            //
            // numQty
            //
            numQty.Font = new System.Drawing.Font("Segoe UI", 12F);
            numQty.Location = new System.Drawing.Point(720, 65);
            numQty.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            numQty.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numQty.Name = "numQty";
            numQty.Size = new System.Drawing.Size(90, 29);
            numQty.TabIndex = 7;
            numQty.Value = new decimal(new int[] { 1, 0, 0, 0 });
            //
            // lstSearch
            //
            lstSearch.Font = new System.Drawing.Font("Segoe UI", 12F);
            lstSearch.ItemHeight = 21;
            lstSearch.Location = new System.Drawing.Point(95, 96);
            lstSearch.Name = "lstSearch";
            lstSearch.Size = new System.Drawing.Size(610, 256);
            lstSearch.TabIndex = 8;
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
            dgvCart.Font = new System.Drawing.Font("Segoe UI", 12F);
            dgvCart.Location = new System.Drawing.Point(20, 115);
            dgvCart.Name = "dgvCart";
            dgvCart.ReadOnly = true;
            dgvCart.RowHeadersVisible = false;
            dgvCart.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvCart.Size = new System.Drawing.Size(900, 490);
            dgvCart.TabIndex = 9;
            //
            // btnDiscount
            //
            btnDiscount.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnDiscount.Font = new System.Drawing.Font("Segoe UI", 10F);
            btnDiscount.Location = new System.Drawing.Point(940, 15);
            btnDiscount.Name = "btnDiscount";
            btnDiscount.Size = new System.Drawing.Size(320, 40);
            btnDiscount.TabIndex = 10;
            btnDiscount.Text = "Discount...";
            btnDiscount.UseVisualStyleBackColor = true;
            btnDiscount.Click += btnDiscount_Click;
            //
            // btnRemoveLine
            //
            btnRemoveLine.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnRemoveLine.Font = new System.Drawing.Font("Segoe UI", 10F);
            btnRemoveLine.Location = new System.Drawing.Point(940, 65);
            btnRemoveLine.Name = "btnRemoveLine";
            btnRemoveLine.Size = new System.Drawing.Size(320, 40);
            btnRemoveLine.TabIndex = 11;
            btnRemoveLine.Text = "Remove Selected Line";
            btnRemoveLine.UseVisualStyleBackColor = true;
            btnRemoveLine.Click += btnRemoveLine_Click;
            //
            // btnDayLog
            //
            btnDayLog.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnDayLog.Font = new System.Drawing.Font("Segoe UI", 10F);
            btnDayLog.Location = new System.Drawing.Point(940, 115);
            btnDayLog.Name = "btnDayLog";
            btnDayLog.Size = new System.Drawing.Size(320, 40);
            btnDayLog.TabIndex = 12;
            btnDayLog.Text = "Today's Sales...";
            btnDayLog.UseVisualStyleBackColor = true;
            btnDayLog.Click += btnDayLog_Click;
            //
            // btnSyncNow
            //
            btnSyncNow.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnSyncNow.Font = new System.Drawing.Font("Segoe UI", 10F);
            btnSyncNow.Location = new System.Drawing.Point(940, 165);
            btnSyncNow.Name = "btnSyncNow";
            btnSyncNow.Size = new System.Drawing.Size(320, 40);
            btnSyncNow.TabIndex = 13;
            btnSyncNow.Text = "Sync Now";
            btnSyncNow.UseVisualStyleBackColor = true;
            btnSyncNow.Click += btnSyncNow_Click;
            //
            // lblTotalCaption
            //
            lblTotalCaption.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            lblTotalCaption.AutoSize = true;
            lblTotalCaption.Font = new System.Drawing.Font("Segoe UI", 11F);
            lblTotalCaption.Location = new System.Drawing.Point(940, 230);
            lblTotalCaption.Name = "lblTotalCaption";
            lblTotalCaption.Size = new System.Drawing.Size(47, 25);
            lblTotalCaption.TabIndex = 14;
            lblTotalCaption.Text = "Total";
            //
            // lblTotal
            //
            lblTotal.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            lblTotal.Font = new System.Drawing.Font("Segoe UI", 28F, System.Drawing.FontStyle.Bold);
            lblTotal.Location = new System.Drawing.Point(940, 260);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new System.Drawing.Size(320, 62);
            lblTotal.TabIndex = 15;
            lblTotal.Text = "0.00";
            lblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // lblCash
            //
            lblCash.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            lblCash.AutoSize = true;
            lblCash.Font = new System.Drawing.Font("Segoe UI", 11F);
            lblCash.Location = new System.Drawing.Point(940, 340);
            lblCash.Name = "lblCash";
            lblCash.Size = new System.Drawing.Size(90, 25);
            lblCash.TabIndex = 16;
            lblCash.Text = "Cash Given";
            //
            // txtCash
            //
            txtCash.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            txtCash.Font = new System.Drawing.Font("Segoe UI", 14F);
            txtCash.Location = new System.Drawing.Point(940, 370);
            txtCash.Name = "txtCash";
            txtCash.Size = new System.Drawing.Size(320, 32);
            txtCash.TabIndex = 17;
            txtCash.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            txtCash.TextChanged += txtCash_TextChanged;
            //
            // lblChange
            //
            lblChange.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            lblChange.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            lblChange.Location = new System.Drawing.Point(940, 412);
            lblChange.Name = "lblChange";
            lblChange.Size = new System.Drawing.Size(320, 36);
            lblChange.TabIndex = 18;
            lblChange.Text = "Change: 0.00";
            lblChange.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // btnComplete
            //
            btnComplete.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnComplete.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            btnComplete.Location = new System.Drawing.Point(940, 495);
            btnComplete.Name = "btnComplete";
            btnComplete.Size = new System.Drawing.Size(320, 80);
            btnComplete.TabIndex = 19;
            btnComplete.Text = "COMPLETE SALE";
            btnComplete.UseVisualStyleBackColor = true;
            btnComplete.Click += btnComplete_Click;
            //
            // lblLastSale
            //
            lblLastSale.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            lblLastSale.Location = new System.Drawing.Point(940, 585);
            lblLastSale.Name = "lblLastSale";
            lblLastSale.Size = new System.Drawing.Size(320, 22);
            lblLastSale.TabIndex = 20;
            lblLastSale.Text = "";
            lblLastSale.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // statusStrip
            //
            statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { lblSyncStatus });
            statusStrip.Location = new System.Drawing.Point(0, 618);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new System.Drawing.Size(1280, 22);
            statusStrip.TabIndex = 21;
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
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1280, 640);
            Controls.Add(lstSearch);
            Controls.Add(lblStaff);
            Controls.Add(txtStaff);
            Controls.Add(btnGoDeliveries);
            Controls.Add(btnGoMyStock);
            Controls.Add(btnGoProduction);
            Controls.Add(btnGoSalesHistory);
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
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "frmPos";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Point of Sale";
            WindowState = System.Windows.Forms.FormWindowState.Maximized;
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
        private System.Windows.Forms.Button btnGoDeliveries;
        private System.Windows.Forms.Button btnGoMyStock;
        private System.Windows.Forms.Button btnGoProduction;
        private System.Windows.Forms.Button btnGoSalesHistory;
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
