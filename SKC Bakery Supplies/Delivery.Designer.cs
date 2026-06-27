namespace SKC_Bakery_Supplies
{
    partial class frmDelivery
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
            label1 = new Label();
            cmbBranch = new ComboBox();
            txtProductSearch = new TextBox();
            label2 = new Label();
            numQty = new NumericUpDown();
            label3 = new Label();
            btnAddItem = new Button();
            lstSearch = new ListBox();
            dgvDeliveryItems = new DataGridView();
            btnSubmitDelivery = new Button();
            ((System.ComponentModel.ISupportInitialize)numQty).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvDeliveryItems).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(8, 8);
            label1.Name = "label1";
            label1.Size = new Size(107, 15);
            label1.TabIndex = 0;
            label1.Text = "Destination Branch";
            // 
            // cmbBranch
            // 
            cmbBranch.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbBranch.FormattingEnabled = true;
            cmbBranch.Location = new Point(8, 24);
            cmbBranch.Name = "cmbBranch";
            cmbBranch.Size = new Size(121, 23);
            cmbBranch.TabIndex = 1;
            // 
            // txtProductSearch
            // 
            txtProductSearch.Location = new Point(136, 24);
            txtProductSearch.Name = "txtProductSearch";
            txtProductSearch.Size = new Size(272, 23);
            txtProductSearch.TabIndex = 2;
            txtProductSearch.TextChanged += txtProductSearch_TextChanged;
            txtProductSearch.KeyDown += txtProductSearch_KeyDown;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(136, 8);
            label2.Name = "label2";
            label2.Size = new Size(87, 15);
            label2.TabIndex = 3;
            label2.Text = "Search Product";
            // 
            // numQty
            // 
            numQty.Location = new Point(416, 24);
            numQty.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numQty.Name = "numQty";
            numQty.Size = new Size(64, 23);
            numQty.TabIndex = 4;
            numQty.Value = new decimal(new int[] { 1, 0, 0, 0 });
            numQty.Click += HighlightNumericText;
            numQty.Enter += HighlightNumericText;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(416, 8);
            label3.Name = "label3";
            label3.Size = new Size(53, 15);
            label3.TabIndex = 5;
            label3.Text = "Quantity";
            // 
            // btnAddItem
            // 
            btnAddItem.Location = new Point(488, 24);
            btnAddItem.Name = "btnAddItem";
            btnAddItem.Size = new Size(75, 23);
            btnAddItem.TabIndex = 6;
            btnAddItem.Text = "Add Item";
            btnAddItem.UseVisualStyleBackColor = true;
            btnAddItem.Click += btnAddItem_Click;
            // 
            // lstSearch
            // 
            lstSearch.FormattingEnabled = true;
            lstSearch.Location = new Point(136, 48);
            lstSearch.Name = "lstSearch";
            lstSearch.Size = new Size(272, 94);
            lstSearch.TabIndex = 7;
            lstSearch.Visible = false;
            lstSearch.Click += lstSearch_Click;
            // 
            // dgvDeliveryItems
            // 
            dgvDeliveryItems.AllowUserToAddRows = false;
            dgvDeliveryItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDeliveryItems.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvDeliveryItems.Location = new Point(136, 48);
            dgvDeliveryItems.Name = "dgvDeliveryItems";
            dgvDeliveryItems.ReadOnly = true;
            dgvDeliveryItems.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDeliveryItems.Size = new Size(424, 400);
            dgvDeliveryItems.TabIndex = 8;
            // 
            // btnSubmitDelivery
            // 
            btnSubmitDelivery.Location = new Point(712, 416);
            btnSubmitDelivery.Name = "btnSubmitDelivery";
            btnSubmitDelivery.Size = new Size(75, 23);
            btnSubmitDelivery.TabIndex = 9;
            btnSubmitDelivery.Text = "Submit Delivery";
            btnSubmitDelivery.UseVisualStyleBackColor = true;
            btnSubmitDelivery.Click += btnSubmitDelivery_Click;
            // 
            // frmDelivery
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnSubmitDelivery);
            Controls.Add(lstSearch);
            Controls.Add(btnAddItem);
            Controls.Add(label3);
            Controls.Add(numQty);
            Controls.Add(label2);
            Controls.Add(txtProductSearch);
            Controls.Add(cmbBranch);
            Controls.Add(label1);
            Controls.Add(dgvDeliveryItems);
            Name = "frmDelivery";
            Text = "Delivery";
            Load += frmDelivery_Load;
            ((System.ComponentModel.ISupportInitialize)numQty).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvDeliveryItems).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private ComboBox cmbBranch;
        private TextBox txtProductSearch;
        private Label label2;
        private NumericUpDown numQty;
        private Label label3;
        private Button btnAddItem;
        private ListBox lstSearch;
        private DataGridView dgvDeliveryItems;
        private Button btnSubmitDelivery;
    }
}