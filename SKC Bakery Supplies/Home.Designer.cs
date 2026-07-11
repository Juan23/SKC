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
            btnAddNewPurchase = new Button();
            btnPurchasesReport = new Button();
            grpPurchases = new GroupBox();
            grpInventory = new GroupBox();
            btnAddInventoryItem = new Button();
            btnViewInventory = new Button();
            groupBox1 = new GroupBox();
            btnPrintReport = new Button();
            grpDelivery = new GroupBox();
            btnViewDeliveries = new Button();
            btnCreateDelivery = new Button();
            groupBox2 = new GroupBox();
            btnSync = new Button();
            grpPurchases.SuspendLayout();
            grpInventory.SuspendLayout();
            groupBox1.SuspendLayout();
            grpDelivery.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // btnAddNewPurchase
            // 
            btnAddNewPurchase.Location = new Point(8, 24);
            btnAddNewPurchase.Name = "btnAddNewPurchase";
            btnAddNewPurchase.Size = new Size(75, 23);
            btnAddNewPurchase.TabIndex = 0;
            btnAddNewPurchase.Text = "Add New";
            btnAddNewPurchase.UseVisualStyleBackColor = true;
            btnAddNewPurchase.Click += btnAddNewPurchase_Click;
            // 
            // btnPurchasesReport
            // 
            btnPurchasesReport.Location = new Point(8, 48);
            btnPurchasesReport.Name = "btnPurchasesReport";
            btnPurchasesReport.Size = new Size(75, 23);
            btnPurchasesReport.TabIndex = 1;
            btnPurchasesReport.Text = "View Reports";
            btnPurchasesReport.UseVisualStyleBackColor = true;
            btnPurchasesReport.Click += btnPurchasesReport_Click;
            // 
            // grpPurchases
            // 
            grpPurchases.Controls.Add(btnAddNewPurchase);
            grpPurchases.Controls.Add(btnPurchasesReport);
            grpPurchases.Location = new Point(8, 8);
            grpPurchases.Name = "grpPurchases";
            grpPurchases.Size = new Size(96, 104);
            grpPurchases.TabIndex = 2;
            grpPurchases.TabStop = false;
            grpPurchases.Text = "Purchases";
            // 
            // grpInventory
            // 
            grpInventory.Controls.Add(btnAddInventoryItem);
            grpInventory.Controls.Add(btnViewInventory);
            grpInventory.Location = new Point(120, 8);
            grpInventory.Name = "grpInventory";
            grpInventory.Size = new Size(96, 104);
            grpInventory.TabIndex = 3;
            grpInventory.TabStop = false;
            grpInventory.Text = "Inventory";
            // 
            // btnAddInventoryItem
            // 
            btnAddInventoryItem.Location = new Point(8, 48);
            btnAddInventoryItem.Name = "btnAddInventoryItem";
            btnAddInventoryItem.Size = new Size(75, 23);
            btnAddInventoryItem.TabIndex = 2;
            btnAddInventoryItem.Text = "Add Item";
            btnAddInventoryItem.UseVisualStyleBackColor = true;
            btnAddInventoryItem.Click += btnAddInventoryItem_Click;
            // 
            // btnViewInventory
            // 
            btnViewInventory.Location = new Point(8, 24);
            btnViewInventory.Name = "btnViewInventory";
            btnViewInventory.Size = new Size(75, 23);
            btnViewInventory.TabIndex = 1;
            btnViewInventory.Text = "View Items";
            btnViewInventory.UseVisualStyleBackColor = true;
            btnViewInventory.Click += btnViewInventory_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btnPrintReport);
            groupBox1.Location = new Point(232, 8);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(96, 104);
            groupBox1.TabIndex = 4;
            groupBox1.TabStop = false;
            groupBox1.Text = "Reports";
            // 
            // btnPrintReport
            // 
            btnPrintReport.Location = new Point(8, 24);
            btnPrintReport.Name = "btnPrintReport";
            btnPrintReport.Size = new Size(75, 23);
            btnPrintReport.TabIndex = 1;
            btnPrintReport.Text = "Print Report";
            btnPrintReport.UseVisualStyleBackColor = true;
            // 
            // grpDelivery
            // 
            grpDelivery.Controls.Add(btnViewDeliveries);
            grpDelivery.Controls.Add(btnCreateDelivery);
            grpDelivery.Location = new Point(344, 8);
            grpDelivery.Name = "grpDelivery";
            grpDelivery.Size = new Size(96, 104);
            grpDelivery.TabIndex = 5;
            grpDelivery.TabStop = false;
            grpDelivery.Text = "Delivery";
            // 
            // btnViewDeliveries
            // 
            btnViewDeliveries.Location = new Point(8, 48);
            btnViewDeliveries.Name = "btnViewDeliveries";
            btnViewDeliveries.Size = new Size(75, 23);
            btnViewDeliveries.TabIndex = 2;
            btnViewDeliveries.Text = "View Deliveries";
            btnViewDeliveries.UseVisualStyleBackColor = true;
            btnViewDeliveries.Click += btnViewDeliveries_Click;
            // 
            // btnCreateDelivery
            // 
            btnCreateDelivery.Location = new Point(8, 24);
            btnCreateDelivery.Name = "btnCreateDelivery";
            btnCreateDelivery.Size = new Size(75, 23);
            btnCreateDelivery.TabIndex = 1;
            btnCreateDelivery.Text = "Create Delivery";
            btnCreateDelivery.UseVisualStyleBackColor = true;
            btnCreateDelivery.Click += btnCreateDelivery_Click;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(btnSync);
            groupBox2.Location = new Point(456, 8);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(112, 104);
            groupBox2.TabIndex = 6;
            groupBox2.TabStop = false;
            groupBox2.Text = "MAIN OFFICE";
            groupBox2.Enter += groupBox2_Enter;
            // 
            // btnSync
            // 
            btnSync.Location = new Point(8, 24);
            btnSync.Name = "btnSync";
            btnSync.Size = new Size(96, 23);
            btnSync.TabIndex = 3;
            btnSync.Text = "Sync Reports";
            btnSync.UseVisualStyleBackColor = true;
            btnSync.Click += btnSync_Click;
            // 
            // frmHome
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(groupBox2);
            Controls.Add(grpDelivery);
            Controls.Add(groupBox1);
            Controls.Add(grpInventory);
            Controls.Add(grpPurchases);
            Name = "frmHome";
            Text = "Home";
            grpPurchases.ResumeLayout(false);
            grpInventory.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            grpDelivery.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Button btnAddNewPurchase;
        private Button btnPurchasesReport;
        private GroupBox grpPurchases;
        private GroupBox grpInventory;
        private Button btnViewInventory;
        private Button btnAddInventoryItem;
        private GroupBox groupBox1;
        private Button btnPrintReport;
        private GroupBox grpDelivery;
        private Button btnCreateDelivery;
        private Button btnViewDeliveries;
        private GroupBox groupBox2;
        private Button btnSync;
    }
}