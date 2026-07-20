namespace SKC_Bakery_Supplies
{
    partial class frmHome
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
            lblHeader = new Label();
            lblSubtitle = new Label();
            btnAddNewPurchase = new Button();
            btnPurchasesReport = new Button();
            grpPurchases = new GroupBox();
            grpInventory = new GroupBox();
            btnAddInventoryItem = new Button();
            btnViewInventory = new Button();
            btnAdjustmentHistory = new Button();
            groupBox1 = new GroupBox();
            btnBranchInventoryReport = new Button();
            btnBranchSales = new Button();
            btnPrintReport = new Button();
            grpDelivery = new GroupBox();
            btnViewDeliveries = new Button();
            btnCreateDelivery = new Button();
            groupBox2 = new GroupBox();
            btnPos = new Button();
            btnSync = new Button();
            grpPurchases.SuspendLayout();
            grpInventory.SuspendLayout();
            groupBox1.SuspendLayout();
            grpDelivery.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            //
            // lblHeader
            //
            lblHeader.AutoSize = true;
            lblHeader.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblHeader.ForeColor = Color.FromArgb(45, 52, 64);
            lblHeader.Location = new Point(25, 20);
            lblHeader.Name = "lblHeader";
            lblHeader.Size = new Size(280, 41);
            lblHeader.TabIndex = 0;
            lblHeader.Text = "SKC Bakery Supplies";
            //
            // lblSubtitle
            //
            lblSubtitle.AutoSize = true;
            lblSubtitle.Font = new Font("Segoe UI", 10F);
            lblSubtitle.ForeColor = Color.Gray;
            lblSubtitle.Location = new Point(28, 62);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Size = new Size(80, 19);
            lblSubtitle.TabIndex = 1;
            lblSubtitle.Text = "Main Office";
            //
            // grpPurchases
            //
            grpPurchases.Controls.Add(btnAddNewPurchase);
            grpPurchases.Controls.Add(btnPurchasesReport);
            grpPurchases.Font = new Font("Segoe UI", 11F);
            grpPurchases.ForeColor = Color.FromArgb(45, 52, 64);
            grpPurchases.Location = new Point(25, 100);
            grpPurchases.Name = "grpPurchases";
            grpPurchases.Size = new Size(230, 420);
            grpPurchases.TabIndex = 2;
            grpPurchases.TabStop = false;
            grpPurchases.Text = "Purchases";
            //
            // btnAddNewPurchase
            //
            btnAddNewPurchase.BackColor = Color.White;
            btnAddNewPurchase.FlatAppearance.BorderColor = Color.Silver;
            btnAddNewPurchase.FlatStyle = FlatStyle.Flat;
            btnAddNewPurchase.Location = new Point(15, 45);
            btnAddNewPurchase.Name = "btnAddNewPurchase";
            btnAddNewPurchase.Size = new Size(200, 100);
            btnAddNewPurchase.TabIndex = 0;
            btnAddNewPurchase.Text = "New Purchase";
            btnAddNewPurchase.UseVisualStyleBackColor = false;
            btnAddNewPurchase.Click += btnAddNewPurchase_Click;
            //
            // btnPurchasesReport
            //
            btnPurchasesReport.BackColor = Color.White;
            btnPurchasesReport.FlatAppearance.BorderColor = Color.Silver;
            btnPurchasesReport.FlatStyle = FlatStyle.Flat;
            btnPurchasesReport.Location = new Point(15, 160);
            btnPurchasesReport.Name = "btnPurchasesReport";
            btnPurchasesReport.Size = new Size(200, 100);
            btnPurchasesReport.TabIndex = 1;
            btnPurchasesReport.Text = "Purchase Reports";
            btnPurchasesReport.UseVisualStyleBackColor = false;
            btnPurchasesReport.Click += btnPurchasesReport_Click;
            //
            // grpInventory
            //
            grpInventory.Controls.Add(btnViewInventory);
            grpInventory.Controls.Add(btnAddInventoryItem);
            grpInventory.Controls.Add(btnAdjustmentHistory);
            grpInventory.Font = new Font("Segoe UI", 11F);
            grpInventory.ForeColor = Color.FromArgb(45, 52, 64);
            grpInventory.Location = new Point(275, 100);
            grpInventory.Name = "grpInventory";
            grpInventory.Size = new Size(230, 420);
            grpInventory.TabIndex = 3;
            grpInventory.TabStop = false;
            grpInventory.Text = "Inventory";
            //
            // btnViewInventory
            //
            btnViewInventory.BackColor = Color.White;
            btnViewInventory.FlatAppearance.BorderColor = Color.Silver;
            btnViewInventory.FlatStyle = FlatStyle.Flat;
            btnViewInventory.Location = new Point(15, 45);
            btnViewInventory.Name = "btnViewInventory";
            btnViewInventory.Size = new Size(200, 100);
            btnViewInventory.TabIndex = 0;
            btnViewInventory.Text = "View Products";
            btnViewInventory.UseVisualStyleBackColor = false;
            btnViewInventory.Click += btnViewInventory_Click;
            //
            // btnAddInventoryItem
            //
            btnAddInventoryItem.BackColor = Color.White;
            btnAddInventoryItem.FlatAppearance.BorderColor = Color.Silver;
            btnAddInventoryItem.FlatStyle = FlatStyle.Flat;
            btnAddInventoryItem.Location = new Point(15, 160);
            btnAddInventoryItem.Name = "btnAddInventoryItem";
            btnAddInventoryItem.Size = new Size(200, 100);
            btnAddInventoryItem.TabIndex = 1;
            btnAddInventoryItem.Text = "Add Item";
            btnAddInventoryItem.UseVisualStyleBackColor = false;
            btnAddInventoryItem.Click += btnAddInventoryItem_Click;
            //
            // btnAdjustmentHistory
            //
            btnAdjustmentHistory.BackColor = Color.White;
            btnAdjustmentHistory.FlatAppearance.BorderColor = Color.Silver;
            btnAdjustmentHistory.FlatStyle = FlatStyle.Flat;
            btnAdjustmentHistory.Location = new Point(15, 275);
            btnAdjustmentHistory.Name = "btnAdjustmentHistory";
            btnAdjustmentHistory.Size = new Size(200, 100);
            btnAdjustmentHistory.TabIndex = 2;
            btnAdjustmentHistory.Text = "Adjustments";
            btnAdjustmentHistory.UseVisualStyleBackColor = false;
            btnAdjustmentHistory.Click += btnAdjustmentHistory_Click;
            //
            // grpDelivery
            //
            grpDelivery.Controls.Add(btnCreateDelivery);
            grpDelivery.Controls.Add(btnViewDeliveries);
            grpDelivery.Font = new Font("Segoe UI", 11F);
            grpDelivery.ForeColor = Color.FromArgb(45, 52, 64);
            grpDelivery.Location = new Point(525, 100);
            grpDelivery.Name = "grpDelivery";
            grpDelivery.Size = new Size(230, 420);
            grpDelivery.TabIndex = 4;
            grpDelivery.TabStop = false;
            grpDelivery.Text = "Deliveries";
            //
            // btnCreateDelivery
            //
            btnCreateDelivery.BackColor = Color.White;
            btnCreateDelivery.FlatAppearance.BorderColor = Color.Silver;
            btnCreateDelivery.FlatStyle = FlatStyle.Flat;
            btnCreateDelivery.Location = new Point(15, 45);
            btnCreateDelivery.Name = "btnCreateDelivery";
            btnCreateDelivery.Size = new Size(200, 100);
            btnCreateDelivery.TabIndex = 0;
            btnCreateDelivery.Text = "New Delivery";
            btnCreateDelivery.UseVisualStyleBackColor = false;
            btnCreateDelivery.Click += btnCreateDelivery_Click;
            //
            // btnViewDeliveries
            //
            btnViewDeliveries.BackColor = Color.White;
            btnViewDeliveries.FlatAppearance.BorderColor = Color.Silver;
            btnViewDeliveries.FlatStyle = FlatStyle.Flat;
            btnViewDeliveries.Location = new Point(15, 160);
            btnViewDeliveries.Name = "btnViewDeliveries";
            btnViewDeliveries.Size = new Size(200, 100);
            btnViewDeliveries.TabIndex = 1;
            btnViewDeliveries.Text = "View Deliveries";
            btnViewDeliveries.UseVisualStyleBackColor = false;
            btnViewDeliveries.Click += btnViewDeliveries_Click;
            //
            // groupBox1
            //
            groupBox1.Controls.Add(btnBranchInventoryReport);
            groupBox1.Controls.Add(btnBranchSales);
            groupBox1.Controls.Add(btnPrintReport);
            groupBox1.Font = new Font("Segoe UI", 11F);
            groupBox1.ForeColor = Color.FromArgb(45, 52, 64);
            groupBox1.Location = new Point(775, 100);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(230, 420);
            groupBox1.TabIndex = 5;
            groupBox1.TabStop = false;
            groupBox1.Text = "Branch Reports";
            //
            // btnBranchInventoryReport
            //
            btnBranchInventoryReport.BackColor = Color.White;
            btnBranchInventoryReport.FlatAppearance.BorderColor = Color.Silver;
            btnBranchInventoryReport.FlatStyle = FlatStyle.Flat;
            btnBranchInventoryReport.Location = new Point(15, 45);
            btnBranchInventoryReport.Name = "btnBranchInventoryReport";
            btnBranchInventoryReport.Size = new Size(200, 100);
            btnBranchInventoryReport.TabIndex = 0;
            btnBranchInventoryReport.Text = "Branch Stock";
            btnBranchInventoryReport.UseVisualStyleBackColor = false;
            btnBranchInventoryReport.Click += btnBranchInventoryReport_Click;
            //
            // btnBranchSales
            //
            btnBranchSales.BackColor = Color.White;
            btnBranchSales.FlatAppearance.BorderColor = Color.Silver;
            btnBranchSales.FlatStyle = FlatStyle.Flat;
            btnBranchSales.Location = new Point(15, 160);
            btnBranchSales.Name = "btnBranchSales";
            btnBranchSales.Size = new Size(200, 100);
            btnBranchSales.TabIndex = 1;
            btnBranchSales.Text = "Branch Sales";
            btnBranchSales.UseVisualStyleBackColor = false;
            btnBranchSales.Click += btnBranchSales_Click;
            //
            // btnPrintReport
            //
            btnPrintReport.Location = new Point(15, 275);
            btnPrintReport.Name = "btnPrintReport";
            btnPrintReport.Size = new Size(200, 100);
            btnPrintReport.TabIndex = 2;
            btnPrintReport.Text = "Print Report";
            btnPrintReport.UseVisualStyleBackColor = true;
            btnPrintReport.Visible = false;
            //
            // groupBox2
            //
            groupBox2.Controls.Add(btnPos);
            groupBox2.Controls.Add(btnSync);
            groupBox2.Font = new Font("Segoe UI", 11F);
            groupBox2.ForeColor = Color.FromArgb(45, 52, 64);
            groupBox2.Location = new Point(1025, 100);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(230, 420);
            groupBox2.TabIndex = 6;
            groupBox2.TabStop = false;
            groupBox2.Text = "Office POS && Sync";
            groupBox2.Enter += groupBox2_Enter;
            //
            // btnPos
            //
            btnPos.BackColor = Color.White;
            btnPos.FlatAppearance.BorderColor = Color.Silver;
            btnPos.FlatStyle = FlatStyle.Flat;
            btnPos.Location = new Point(15, 45);
            btnPos.Name = "btnPos";
            btnPos.Size = new Size(200, 100);
            btnPos.TabIndex = 0;
            btnPos.Text = "POS";
            btnPos.UseVisualStyleBackColor = false;
            btnPos.Click += btnPos_Click;
            //
            // btnSync
            //
            btnSync.BackColor = Color.White;
            btnSync.FlatAppearance.BorderColor = Color.Silver;
            btnSync.FlatStyle = FlatStyle.Flat;
            btnSync.Location = new Point(15, 160);
            btnSync.Name = "btnSync";
            btnSync.Size = new Size(200, 100);
            btnSync.TabIndex = 1;
            btnSync.Text = "Sync to Central Server";
            btnSync.UseVisualStyleBackColor = false;
            btnSync.Click += btnSync_Click;
            //
            // frmHome
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(1280, 640);
            Controls.Add(lblHeader);
            Controls.Add(lblSubtitle);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(grpDelivery);
            Controls.Add(grpInventory);
            Controls.Add(grpPurchases);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "frmHome";
            StartPosition = FormStartPosition.CenterScreen;
            WindowState = FormWindowState.Maximized;
            Text = "Home";
            grpPurchases.ResumeLayout(false);
            grpInventory.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            grpDelivery.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblHeader;
        private Label lblSubtitle;
        private Button btnAddNewPurchase;
        private Button btnPurchasesReport;
        private GroupBox grpPurchases;
        private GroupBox grpInventory;
        private Button btnViewInventory;
        private Button btnAddInventoryItem;
        private Button btnAdjustmentHistory;
        private GroupBox groupBox1;
        private Button btnBranchInventoryReport;
        private Button btnBranchSales;
        private Button btnPrintReport;
        private GroupBox grpDelivery;
        private Button btnCreateDelivery;
        private Button btnViewDeliveries;
        private GroupBox groupBox2;
        private Button btnPos;
        private Button btnSync;
    }
}
