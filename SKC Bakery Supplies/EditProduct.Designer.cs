namespace SKC_Bakery_Supplies
{
    partial class frmEditProduct
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
            label6 = new Label();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            label7 = new Label();
            numPrice = new NumericUpDown();
            btnSave = new Button();
            txtUOM = new TextBox();
            txtBaseName = new TextBox();
            txtBrand = new TextBox();
            txtSKU = new TextBox();
            txtMultiplier = new TextBox();
            ((System.ComponentModel.ISupportInitialize)numPrice).BeginInit();
            SuspendLayout();
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(496, 8);
            label6.Name = "label6";
            label6.Size = new Size(71, 15);
            label6.TabIndex = 25;
            label6.Text = "Selling Price";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(424, 8);
            label5.Name = "label5";
            label5.Size = new Size(58, 15);
            label5.TabIndex = 24;
            label5.Text = "Multiplier";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(360, 8);
            label4.Name = "label4";
            label4.Size = new Size(29, 15);
            label4.TabIndex = 23;
            label4.Text = "Unit";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(216, 8);
            label3.Name = "label3";
            label3.Size = new Size(66, 15);
            label3.TabIndex = 22;
            label3.Text = "Base Name";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(112, 8);
            label2.Name = "label2";
            label2.Size = new Size(38, 15);
            label2.TabIndex = 21;
            label2.Text = "Brand";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(8, 8);
            label7.Name = "label7";
            label7.Size = new Size(28, 15);
            label7.TabIndex = 20;
            label7.Text = "SKU";
            // 
            // numPrice
            // 
            numPrice.Location = new Point(496, 24);
            numPrice.Name = "numPrice";
            numPrice.Size = new Size(80, 23);
            numPrice.TabIndex = 19;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(504, 56);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(75, 23);
            btnSave.TabIndex = 18;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // txtUOM
            // 
            txtUOM.AcceptsReturn = true;
            txtUOM.Location = new Point(360, 24);
            txtUOM.Name = "txtUOM";
            txtUOM.ReadOnly = true;
            txtUOM.Size = new Size(60, 23);
            txtUOM.TabIndex = 16;
            // 
            // txtBaseName
            // 
            txtBaseName.Location = new Point(216, 24);
            txtBaseName.Name = "txtBaseName";
            txtBaseName.Size = new Size(136, 23);
            txtBaseName.TabIndex = 15;
            // 
            // txtBrand
            // 
            txtBrand.Location = new Point(112, 24);
            txtBrand.Name = "txtBrand";
            txtBrand.Size = new Size(100, 23);
            txtBrand.TabIndex = 14;
            // 
            // txtSKU
            // 
            txtSKU.Location = new Point(8, 24);
            txtSKU.Name = "txtSKU";
            txtSKU.ReadOnly = true;
            txtSKU.Size = new Size(100, 23);
            txtSKU.TabIndex = 13;
            // 
            // txtMultiplier
            // 
            txtMultiplier.Location = new Point(424, 24);
            txtMultiplier.Name = "txtMultiplier";
            txtMultiplier.ReadOnly = true;
            txtMultiplier.Size = new Size(64, 23);
            txtMultiplier.TabIndex = 26;
            // 
            // frmEditProduct
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(593, 94);
            Controls.Add(txtMultiplier);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label7);
            Controls.Add(numPrice);
            Controls.Add(btnSave);
            Controls.Add(txtUOM);
            Controls.Add(txtBaseName);
            Controls.Add(txtBrand);
            Controls.Add(txtSKU);
            Name = "frmEditProduct";
            Text = "Edit Product";
            ((System.ComponentModel.ISupportInitialize)numPrice).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label6;
        private Label label5;
        private Label label4;
        private Label label3;
        private Label label2;
        private Label label7;
        private NumericUpDown numPrice;
        private Button btnSave;
        private TextBox txtUOM;
        private TextBox txtBaseName;
        private TextBox txtBrand;
        private TextBox txtSKU;
        private TextBox txtMultiplier;
    }
}