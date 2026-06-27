namespace SKC_POS
{
    partial class frmProducts
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
            gridAllProducts = new DataGridView();
            txtProductName = new TextBox();
            txtProductPrice = new TextBox();
            btnAddProduct = new Button();
            btnUploadProductList = new Button();
            ((System.ComponentModel.ISupportInitialize)gridAllProducts).BeginInit();
            SuspendLayout();
            // 
            // gridAllProducts
            // 
            gridAllProducts.AllowUserToAddRows = false;
            gridAllProducts.AllowUserToDeleteRows = false;
            gridAllProducts.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            gridAllProducts.Location = new Point(8, 8);
            gridAllProducts.Name = "gridAllProducts";
            gridAllProducts.ReadOnly = true;
            gridAllProducts.Size = new Size(984, 664);
            gridAllProducts.TabIndex = 0;
            // 
            // txtProductName
            // 
            txtProductName.Location = new Point(1144, 16);
            txtProductName.Name = "txtProductName";
            txtProductName.Size = new Size(100, 23);
            txtProductName.TabIndex = 1;
            // 
            // txtProductPrice
            // 
            txtProductPrice.Location = new Point(1144, 48);
            txtProductPrice.Name = "txtProductPrice";
            txtProductPrice.Size = new Size(100, 23);
            txtProductPrice.TabIndex = 2;
            // 
            // btnAddProduct
            // 
            btnAddProduct.Location = new Point(1128, 88);
            btnAddProduct.Name = "btnAddProduct";
            btnAddProduct.Size = new Size(128, 23);
            btnAddProduct.TabIndex = 3;
            btnAddProduct.Text = "Add Product";
            btnAddProduct.UseVisualStyleBackColor = true;
            btnAddProduct.Click += btnAddProduct_Click;
            // 
            // btnUploadProductList
            // 
            btnUploadProductList.Location = new Point(1128, 376);
            btnUploadProductList.Name = "btnUploadProductList";
            btnUploadProductList.Size = new Size(128, 23);
            btnUploadProductList.TabIndex = 4;
            btnUploadProductList.Text = "Bulk Upload";
            btnUploadProductList.UseVisualStyleBackColor = true;
            btnUploadProductList.Click += btnUploadProductList_Click;
            // 
            // frmProducts
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1264, 681);
            Controls.Add(btnUploadProductList);
            Controls.Add(btnAddProduct);
            Controls.Add(txtProductPrice);
            Controls.Add(txtProductName);
            Controls.Add(gridAllProducts);
            Name = "frmProducts";
            Text = "Products";
            Load += frmProducts_Load;
            ((System.ComponentModel.ISupportInitialize)gridAllProducts).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView gridAllProducts;
        private TextBox txtProductName;
        private TextBox txtProductPrice;
        private Button btnAddProduct;
        private Button btnUploadProductList;
    }
}