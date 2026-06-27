namespace SKC_Bakery_Supplies
{
    partial class frmPurchases
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
            txtSupplier = new TextBox();
            label1 = new Label();
            dtpDate = new DateTimePicker();
            label2 = new Label();
            txtProductSearch = new TextBox();
            label3 = new Label();
            numQty = new NumericUpDown();
            label4 = new Label();
            numUnitCost = new NumericUpDown();
            label5 = new Label();
            btnAddItem = new Button();
            groupBox1 = new GroupBox();
            txtTotalCost = new TextBox();
            label6 = new Label();
            lstSearch = new ListBox();
            dgvPurchaseItems = new DataGridView();
            btnSubmitPurchase = new Button();
            lblRunningTotal = new Label();
            ((System.ComponentModel.ISupportInitialize)numQty).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numUnitCost).BeginInit();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPurchaseItems).BeginInit();
            SuspendLayout();
            // 
            // txtSupplier
            // 
            txtSupplier.Location = new Point(16, 48);
            txtSupplier.Name = "txtSupplier";
            txtSupplier.Size = new Size(100, 23);
            txtSupplier.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(16, 32);
            label1.Name = "label1";
            label1.Size = new Size(50, 15);
            label1.TabIndex = 1;
            label1.Text = "Supplier";
            // 
            // dtpDate
            // 
            dtpDate.Location = new Point(128, 48);
            dtpDate.Name = "dtpDate";
            dtpDate.Size = new Size(200, 23);
            dtpDate.TabIndex = 2;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(128, 32);
            label2.Name = "label2";
            label2.Size = new Size(31, 15);
            label2.TabIndex = 3;
            label2.Text = "Date";
            // 
            // txtProductSearch
            // 
            txtProductSearch.Location = new Point(16, 104);
            txtProductSearch.Name = "txtProductSearch";
            txtProductSearch.Size = new Size(312, 23);
            txtProductSearch.TabIndex = 4;
            txtProductSearch.TextChanged += txtProductSearch_TextChanged;
            txtProductSearch.KeyDown += txtProductSearch_KeyDown;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(16, 88);
            label3.Name = "label3";
            label3.Size = new Size(87, 15);
            label3.TabIndex = 5;
            label3.Text = "Search Product";
            // 
            // numQty
            // 
            numQty.Location = new Point(16, 168);
            numQty.Name = "numQty";
            numQty.Size = new Size(56, 23);
            numQty.TabIndex = 6;
            numQty.Value = new decimal(new int[] { 1, 0, 0, 0 });
            numQty.ValueChanged += CalculateTotalCost;
            numQty.Enter += HighlightNumericText;
            numQty.KeyUp += CalculateTotalCost;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(16, 152);
            label4.Name = "label4";
            label4.Size = new Size(53, 15);
            label4.TabIndex = 7;
            label4.Text = "Quantity";
            // 
            // numUnitCost
            // 
            numUnitCost.DecimalPlaces = 2;
            numUnitCost.Location = new Point(80, 168);
            numUnitCost.Maximum = new decimal(new int[] { 99999, 0, 0, 0 });
            numUnitCost.Name = "numUnitCost";
            numUnitCost.Size = new Size(104, 23);
            numUnitCost.TabIndex = 8;
            numUnitCost.ValueChanged += CalculateTotalCost;
            numUnitCost.Enter += HighlightNumericText;
            numUnitCost.KeyUp += CalculateTotalCost;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(80, 152);
            label5.Name = "label5";
            label5.Size = new Size(56, 15);
            label5.TabIndex = 9;
            label5.Text = "Unit Cost";
            // 
            // btnAddItem
            // 
            btnAddItem.Location = new Point(248, 224);
            btnAddItem.Name = "btnAddItem";
            btnAddItem.Size = new Size(75, 23);
            btnAddItem.TabIndex = 10;
            btnAddItem.Text = "Add Item";
            btnAddItem.UseVisualStyleBackColor = true;
            btnAddItem.Click += btnAddItem_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(txtTotalCost);
            groupBox1.Controls.Add(label6);
            groupBox1.Controls.Add(txtSupplier);
            groupBox1.Controls.Add(btnAddItem);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(dtpDate);
            groupBox1.Controls.Add(numUnitCost);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(txtProductSearch);
            groupBox1.Controls.Add(numQty);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(lstSearch);
            groupBox1.Location = new Point(920, 8);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(336, 624);
            groupBox1.TabIndex = 11;
            groupBox1.TabStop = false;
            groupBox1.Text = "Add Product";
            // 
            // txtTotalCost
            // 
            txtTotalCost.Location = new Point(192, 168);
            txtTotalCost.Name = "txtTotalCost";
            txtTotalCost.ReadOnly = true;
            txtTotalCost.Size = new Size(100, 23);
            txtTotalCost.TabIndex = 14;
            txtTotalCost.TabStop = false;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Enabled = false;
            label6.Location = new Point(192, 152);
            label6.Name = "label6";
            label6.Size = new Size(33, 15);
            label6.TabIndex = 13;
            label6.Text = "Total";
            // 
            // lstSearch
            // 
            lstSearch.FormattingEnabled = true;
            lstSearch.Location = new Point(16, 128);
            lstSearch.Name = "lstSearch";
            lstSearch.Size = new Size(312, 94);
            lstSearch.TabIndex = 11;
            lstSearch.Visible = false;
            lstSearch.Click += lstSearch_Click;
            // 
            // dgvPurchaseItems
            // 
            dgvPurchaseItems.AllowUserToAddRows = false;
            dgvPurchaseItems.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPurchaseItems.EditMode = DataGridViewEditMode.EditProgrammatically;
            dgvPurchaseItems.Location = new Point(8, 8);
            dgvPurchaseItems.Name = "dgvPurchaseItems";
            dgvPurchaseItems.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPurchaseItems.Size = new Size(904, 624);
            dgvPurchaseItems.TabIndex = 12;
            // 
            // btnSubmitPurchase
            // 
            btnSubmitPurchase.Location = new Point(824, 640);
            btnSubmitPurchase.Name = "btnSubmitPurchase";
            btnSubmitPurchase.Size = new Size(91, 32);
            btnSubmitPurchase.TabIndex = 13;
            btnSubmitPurchase.Text = "Submit";
            btnSubmitPurchase.UseVisualStyleBackColor = true;
            btnSubmitPurchase.Click += btnSubmitPurchase_Click;
            // 
            // lblRunningTotal
            // 
            lblRunningTotal.Anchor = AnchorStyles.Right;
            lblRunningTotal.Location = new Point(728, 648);
            lblRunningTotal.Name = "lblRunningTotal";
            lblRunningTotal.Size = new Size(96, 16);
            lblRunningTotal.TabIndex = 14;
            lblRunningTotal.Text = "0";
            lblRunningTotal.TextAlign = ContentAlignment.MiddleRight;
            // 
            // frmPurchases
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1264, 681);
            Controls.Add(lblRunningTotal);
            Controls.Add(btnSubmitPurchase);
            Controls.Add(dgvPurchaseItems);
            Controls.Add(groupBox1);
            Name = "frmPurchases";
            Text = "Purchases";
            Load += frmPurchases_Load;
            Enter += HighlightNumericText;
            ((System.ComponentModel.ISupportInitialize)numQty).EndInit();
            ((System.ComponentModel.ISupportInitialize)numUnitCost).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPurchaseItems).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TextBox txtSupplier;
        private Label label1;
        private DateTimePicker dtpDate;
        private Label label2;
        private TextBox txtProductSearch;
        private Label label3;
        private NumericUpDown numQty;
        private Label label4;
        private NumericUpDown numUnitCost;
        private Label label5;
        private Button btnAddItem;
        private GroupBox groupBox1;
        private DataGridView dgvPurchaseItems;
        private Button btnSubmitPurchase;
        private ListBox lstSearch;
        private Label lblRunningTotal;
        private TextBox txtTotalCost;
        private Label label6;
    }
}