namespace SKC_Bakery_Supplies
{
    partial class frmViewProducts
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
            txtSearch = new TextBox();
            dgvProducts = new DataGridView();
            btnEdit = new Button();
            btnDelete = new Button();
            btnAdjustStock = new Button();
            grpQuickAddProduct = new GroupBox();
            btnSaveNew = new Button();
            Price = new Label();
            numNewPrice = new NumericUpDown();
            label3 = new Label();
            txtNewBaseName = new TextBox();
            label2 = new Label();
            txtNewBrand = new TextBox();
            label1 = new Label();
            txtNewSKU = new TextBox();
            btnPrintInventory = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvProducts).BeginInit();
            grpQuickAddProduct.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numNewPrice).BeginInit();
            SuspendLayout();
            //
            // lblHeader
            //
            lblHeader.AutoSize = true;
            lblHeader.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblHeader.Location = new Point(20, 15);
            lblHeader.Name = "lblHeader";
            lblHeader.Size = new Size(120, 32);
            lblHeader.TabIndex = 0;
            lblHeader.Text = "Products";
            //
            // txtSearch
            //
            txtSearch.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtSearch.Font = new Font("Segoe UI", 11F);
            txtSearch.Location = new Point(20, 60);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "Search products...";
            txtSearch.Size = new Size(900, 28);
            txtSearch.TabIndex = 1;
            txtSearch.TextChanged += txtSearch_TextChanged;
            //
            // dgvProducts
            //
            dgvProducts.AllowUserToAddRows = false;
            dgvProducts.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProducts.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
            dgvProducts.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvProducts.Location = new Point(20, 100);
            dgvProducts.Name = "dgvProducts";
            dgvProducts.ReadOnly = true;
            dgvProducts.RowHeadersVisible = false;
            dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProducts.Size = new Size(900, 470);
            dgvProducts.TabIndex = 2;
            //
            // btnAdjustStock
            //
            btnAdjustStock.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnAdjustStock.Font = new Font("Segoe UI", 10F);
            btnAdjustStock.Location = new Point(20, 588);
            btnAdjustStock.Name = "btnAdjustStock";
            btnAdjustStock.Size = new Size(120, 40);
            btnAdjustStock.TabIndex = 3;
            btnAdjustStock.Text = "Adjust";
            btnAdjustStock.UseVisualStyleBackColor = true;
            btnAdjustStock.Click += btnAdjustStock_Click;
            //
            // btnEdit
            //
            btnEdit.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnEdit.Font = new Font("Segoe UI", 10F);
            btnEdit.Location = new Point(150, 588);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(120, 40);
            btnEdit.TabIndex = 4;
            btnEdit.Text = "Edit";
            btnEdit.UseVisualStyleBackColor = true;
            btnEdit.Click += btnEdit_Click;
            //
            // btnDelete
            //
            btnDelete.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnDelete.Font = new Font("Segoe UI", 10F);
            btnDelete.Location = new Point(280, 588);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(120, 40);
            btnDelete.TabIndex = 5;
            btnDelete.Text = "Delete";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            //
            // btnPrintInventory
            //
            btnPrintInventory.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnPrintInventory.Font = new Font("Segoe UI", 10F);
            btnPrintInventory.Location = new Point(410, 588);
            btnPrintInventory.Name = "btnPrintInventory";
            btnPrintInventory.Size = new Size(160, 40);
            btnPrintInventory.TabIndex = 6;
            btnPrintInventory.Text = "Print Inventory";
            btnPrintInventory.UseVisualStyleBackColor = true;
            btnPrintInventory.Click += btnPrintInventory_Click;
            //
            // grpQuickAddProduct
            //
            grpQuickAddProduct.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            grpQuickAddProduct.Controls.Add(btnSaveNew);
            grpQuickAddProduct.Controls.Add(Price);
            grpQuickAddProduct.Controls.Add(numNewPrice);
            grpQuickAddProduct.Controls.Add(label3);
            grpQuickAddProduct.Controls.Add(txtNewBaseName);
            grpQuickAddProduct.Controls.Add(label2);
            grpQuickAddProduct.Controls.Add(txtNewBrand);
            grpQuickAddProduct.Controls.Add(label1);
            grpQuickAddProduct.Controls.Add(txtNewSKU);
            grpQuickAddProduct.Font = new Font("Segoe UI", 10F);
            grpQuickAddProduct.Location = new Point(940, 60);
            grpQuickAddProduct.Name = "grpQuickAddProduct";
            grpQuickAddProduct.Size = new Size(320, 340);
            grpQuickAddProduct.TabIndex = 7;
            grpQuickAddProduct.TabStop = false;
            grpQuickAddProduct.Text = "Quick Add Product";
            //
            // btnSaveNew
            //
            btnSaveNew.Location = new Point(16, 220);
            btnSaveNew.Name = "btnSaveNew";
            btnSaveNew.Size = new Size(284, 40);
            btnSaveNew.TabIndex = 4;
            btnSaveNew.Text = "Add Item";
            btnSaveNew.UseVisualStyleBackColor = true;
            btnSaveNew.Click += btnSaveNew_Click;
            //
            // Price
            //
            Price.AutoSize = true;
            Price.Location = new Point(16, 150);
            Price.Name = "Price";
            Price.Size = new Size(42, 20);
            Price.TabIndex = 3;
            Price.Text = "Price";
            //
            // numNewPrice
            //
            numNewPrice.Location = new Point(16, 170);
            numNewPrice.Name = "numNewPrice";
            numNewPrice.Size = new Size(140, 28);
            numNewPrice.TabIndex = 3;
            numNewPrice.Enter += HighlightNumericText;
            //
            // label3
            //
            label3.AutoSize = true;
            label3.Location = new Point(16, 90);
            label3.Name = "label3";
            label3.Size = new Size(84, 20);
            label3.TabIndex = 2;
            label3.Text = "Base Name";
            //
            // txtNewBaseName
            //
            txtNewBaseName.Location = new Point(16, 110);
            txtNewBaseName.Name = "txtNewBaseName";
            txtNewBaseName.Size = new Size(284, 28);
            txtNewBaseName.TabIndex = 2;
            txtNewBaseName.TextChanged += txtNewBaseName_TextChanged;
            //
            // label2
            //
            label2.AutoSize = true;
            label2.Location = new Point(170, 30);
            label2.Name = "label2";
            label2.Size = new Size(48, 20);
            label2.TabIndex = 1;
            label2.Text = "Brand";
            //
            // txtNewBrand
            //
            txtNewBrand.Location = new Point(170, 50);
            txtNewBrand.Name = "txtNewBrand";
            txtNewBrand.Size = new Size(130, 28);
            txtNewBrand.TabIndex = 1;
            txtNewBrand.TextChanged += txtNewBrand_TextChanged;
            //
            // label1
            //
            label1.AutoSize = true;
            label1.Location = new Point(16, 30);
            label1.Name = "label1";
            label1.Size = new Size(36, 20);
            label1.TabIndex = 0;
            label1.Text = "SKU";
            //
            // txtNewSKU
            //
            txtNewSKU.Location = new Point(16, 50);
            txtNewSKU.Name = "txtNewSKU";
            txtNewSKU.Size = new Size(140, 28);
            txtNewSKU.TabIndex = 0;
            //
            // frmViewProducts
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1280, 640);
            Controls.Add(btnPrintInventory);
            Controls.Add(grpQuickAddProduct);
            Controls.Add(btnDelete);
            Controls.Add(btnEdit);
            Controls.Add(btnAdjustStock);
            Controls.Add(dgvProducts);
            Controls.Add(txtSearch);
            Controls.Add(lblHeader);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "frmViewProducts";
            StartPosition = FormStartPosition.CenterScreen;
            WindowState = FormWindowState.Maximized;
            Text = "ViewProducts";
            ((System.ComponentModel.ISupportInitialize)dgvProducts).EndInit();
            grpQuickAddProduct.ResumeLayout(false);
            grpQuickAddProduct.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numNewPrice).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblHeader;
        private TextBox txtSearch;
        private DataGridView dgvProducts;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnAdjustStock;
        private GroupBox grpQuickAddProduct;
        private Label label1;
        private TextBox txtNewSKU;
        private Button btnSaveNew;
        private Label Price;
        private NumericUpDown numNewPrice;
        private Label label3;
        private TextBox txtNewBaseName;
        private Label label2;
        private TextBox txtNewBrand;
        private Button btnPrintInventory;
    }
}