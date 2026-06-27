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
            txtSearch = new TextBox();
            dgvProducts = new DataGridView();
            btnEdit = new Button();
            btnDelete = new Button();
            grpQuickAddProduct = new GroupBox();
            btnSaveNew = new Button();
            Price = new Label();
            numNewPrice = new NumericUpDown();
            label5 = new Label();
            numNewMultiplier = new NumericUpDown();
            label4 = new Label();
            txtNewUOM = new TextBox();
            label3 = new Label();
            txtNewBaseName = new TextBox();
            label2 = new Label();
            txtNewBrand = new TextBox();
            label1 = new Label();
            txtNewSKU = new TextBox();
            ((System.ComponentModel.ISupportInitialize)dgvProducts).BeginInit();
            grpQuickAddProduct.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numNewPrice).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numNewMultiplier).BeginInit();
            SuspendLayout();
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(8, 8);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(784, 23);
            txtSearch.TabIndex = 0;
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // dgvProducts
            // 
            dgvProducts.AllowUserToAddRows = false;
            dgvProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCellsExceptHeader;
            dgvProducts.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
            dgvProducts.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvProducts.Location = new Point(8, 40);
            dgvProducts.Name = "dgvProducts";
            dgvProducts.ReadOnly = true;
            dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProducts.Size = new Size(784, 376);
            dgvProducts.TabIndex = 1;
            // 
            // btnEdit
            // 
            btnEdit.Location = new Point(640, 424);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(75, 23);
            btnEdit.TabIndex = 2;
            btnEdit.Text = "Edit";
            btnEdit.UseVisualStyleBackColor = true;
            btnEdit.Click += btnEdit_Click;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(720, 424);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(75, 23);
            btnDelete.TabIndex = 3;
            btnDelete.Text = "Delete";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // grpQuickAddProduct
            // 
            grpQuickAddProduct.Controls.Add(btnSaveNew);
            grpQuickAddProduct.Controls.Add(Price);
            grpQuickAddProduct.Controls.Add(numNewPrice);
            grpQuickAddProduct.Controls.Add(label5);
            grpQuickAddProduct.Controls.Add(numNewMultiplier);
            grpQuickAddProduct.Controls.Add(label4);
            grpQuickAddProduct.Controls.Add(txtNewUOM);
            grpQuickAddProduct.Controls.Add(label3);
            grpQuickAddProduct.Controls.Add(txtNewBaseName);
            grpQuickAddProduct.Controls.Add(label2);
            grpQuickAddProduct.Controls.Add(txtNewBrand);
            grpQuickAddProduct.Controls.Add(label1);
            grpQuickAddProduct.Controls.Add(txtNewSKU);
            grpQuickAddProduct.Location = new Point(800, 8);
            grpQuickAddProduct.Name = "grpQuickAddProduct";
            grpQuickAddProduct.Size = new Size(240, 408);
            grpQuickAddProduct.TabIndex = 4;
            grpQuickAddProduct.TabStop = false;
            grpQuickAddProduct.Text = "Quick Add Product";
            // 
            // btnSaveNew
            // 
            btnSaveNew.Location = new Point(160, 176);
            btnSaveNew.Name = "btnSaveNew";
            btnSaveNew.Size = new Size(75, 23);
            btnSaveNew.TabIndex = 12;
            btnSaveNew.Text = "Add Item";
            btnSaveNew.UseVisualStyleBackColor = true;
            btnSaveNew.Click += btnSaveNew_Click;
            // 
            // Price
            // 
            Price.AutoSize = true;
            Price.Location = new Point(168, 128);
            Price.Name = "Price";
            Price.Size = new Size(33, 15);
            Price.TabIndex = 11;
            Price.Text = "Price";
            // 
            // numNewPrice
            // 
            numNewPrice.Location = new Point(168, 144);
            numNewPrice.Name = "numNewPrice";
            numNewPrice.Size = new Size(64, 23);
            numNewPrice.TabIndex = 10;
            numNewPrice.Enter += HighlightNumericText;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(96, 128);
            label5.Name = "label5";
            label5.Size = new Size(58, 15);
            label5.TabIndex = 9;
            label5.Text = "Multiplier";
            // 
            // numNewMultiplier
            // 
            numNewMultiplier.DecimalPlaces = 2;
            numNewMultiplier.Location = new Point(96, 144);
            numNewMultiplier.Name = "numNewMultiplier";
            numNewMultiplier.Size = new Size(64, 23);
            numNewMultiplier.TabIndex = 8;
            numNewMultiplier.Enter += HighlightNumericText;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(8, 128);
            label4.Name = "label4";
            label4.Size = new Size(35, 15);
            label4.TabIndex = 7;
            label4.Text = "UOM";
            // 
            // txtNewUOM
            // 
            txtNewUOM.Location = new Point(8, 144);
            txtNewUOM.Name = "txtNewUOM";
            txtNewUOM.Size = new Size(80, 23);
            txtNewUOM.TabIndex = 6;
            txtNewUOM.TextChanged += txtNewUOM_TextChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(8, 80);
            label3.Name = "label3";
            label3.Size = new Size(66, 15);
            label3.TabIndex = 5;
            label3.Text = "Base Name";
            // 
            // txtNewBaseName
            // 
            txtNewBaseName.Location = new Point(8, 96);
            txtNewBaseName.Name = "txtNewBaseName";
            txtNewBaseName.Size = new Size(224, 23);
            txtNewBaseName.TabIndex = 4;
            txtNewBaseName.TextChanged += txtNewBaseName_TextChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(88, 32);
            label2.Name = "label2";
            label2.Size = new Size(38, 15);
            label2.TabIndex = 3;
            label2.Text = "Brand";
            // 
            // txtNewBrand
            // 
            txtNewBrand.Location = new Point(88, 48);
            txtNewBrand.Name = "txtNewBrand";
            txtNewBrand.Size = new Size(144, 23);
            txtNewBrand.TabIndex = 2;
            txtNewBrand.TextChanged += txtNewBrand_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(8, 32);
            label1.Name = "label1";
            label1.Size = new Size(28, 15);
            label1.TabIndex = 1;
            label1.Text = "SKU";
            // 
            // txtNewSKU
            // 
            txtNewSKU.Location = new Point(8, 48);
            txtNewSKU.Name = "txtNewSKU";
            txtNewSKU.Size = new Size(72, 23);
            txtNewSKU.TabIndex = 0;
            // 
            // frmViewProducts
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1047, 450);
            Controls.Add(grpQuickAddProduct);
            Controls.Add(btnDelete);
            Controls.Add(btnEdit);
            Controls.Add(dgvProducts);
            Controls.Add(txtSearch);
            Name = "frmViewProducts";
            Text = "ViewProducts";
            ((System.ComponentModel.ISupportInitialize)dgvProducts).EndInit();
            grpQuickAddProduct.ResumeLayout(false);
            grpQuickAddProduct.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numNewPrice).EndInit();
            ((System.ComponentModel.ISupportInitialize)numNewMultiplier).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtSearch;
        private DataGridView dgvProducts;
        private Button btnEdit;
        private Button btnDelete;
        private GroupBox grpQuickAddProduct;
        private Label label1;
        private TextBox txtNewSKU;
        private Button btnSaveNew;
        private Label Price;
        private NumericUpDown numNewPrice;
        private Label label5;
        private NumericUpDown numNewMultiplier;
        private Label label4;
        private TextBox txtNewUOM;
        private Label label3;
        private TextBox txtNewBaseName;
        private Label label2;
        private TextBox txtNewBrand;
    }
}