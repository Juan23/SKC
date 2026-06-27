namespace SKC_Bakery_Supplies
{
    partial class frmAddMasterItem
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            txtSKU = new TextBox();
            txtBrand = new TextBox();
            txtBaseName = new TextBox();
            txtUOM = new TextBox();
            numMultiplier = new NumericUpDown();
            btnSave = new Button();
            numPrice = new NumericUpDown();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            ((System.ComponentModel.ISupportInitialize)numMultiplier).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numPrice).BeginInit();
            SuspendLayout();
            // 
            // txtSKU
            // 
            txtSKU.Location = new Point(32, 32);
            txtSKU.Name = "txtSKU";
            txtSKU.ReadOnly = true;
            txtSKU.Size = new Size(100, 23);
            txtSKU.TabIndex = 0;
            // 
            // txtBrand
            // 
            txtBrand.Location = new Point(136, 32);
            txtBrand.Name = "txtBrand";
            txtBrand.Size = new Size(100, 23);
            txtBrand.TabIndex = 1;
            txtBrand.TextChanged += txtBrand_TextChanged;
            // 
            // txtBaseName
            // 
            txtBaseName.Location = new Point(240, 32);
            txtBaseName.Name = "txtBaseName";
            txtBaseName.Size = new Size(100, 23);
            txtBaseName.TabIndex = 2;
            txtBaseName.TextChanged += txtBaseName_TextChanged;
            // 
            // txtUOM
            // 
            txtUOM.AcceptsReturn = true;
            txtUOM.Location = new Point(344, 32);
            txtUOM.Name = "txtUOM";
            txtUOM.Size = new Size(100, 23);
            txtUOM.TabIndex = 3;
            txtUOM.TextChanged += txtUOM_TextChanged;
            // 
            // numMultiplier
            // 
            numMultiplier.DecimalPlaces = 2;
            numMultiplier.Location = new Point(448, 32);
            numMultiplier.Name = "numMultiplier";
            numMultiplier.Size = new Size(64, 23);
            numMultiplier.TabIndex = 4;
            numMultiplier.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // btnSave
            // 
            btnSave.Location = new Point(528, 64);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(75, 23);
            btnSave.TabIndex = 5;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // numPrice
            // 
            numPrice.Location = new Point(520, 32);
            numPrice.Name = "numPrice";
            numPrice.Size = new Size(80, 23);
            numPrice.TabIndex = 6;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(32, 16);
            label1.Name = "label1";
            label1.Size = new Size(28, 15);
            label1.TabIndex = 7;
            label1.Text = "SKU";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(136, 16);
            label2.Name = "label2";
            label2.Size = new Size(38, 15);
            label2.TabIndex = 8;
            label2.Text = "Brand";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(240, 16);
            label3.Name = "label3";
            label3.Size = new Size(66, 15);
            label3.TabIndex = 9;
            label3.Text = "Base Name";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(344, 16);
            label4.Name = "label4";
            label4.Size = new Size(29, 15);
            label4.TabIndex = 10;
            label4.Text = "Unit";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(448, 16);
            label5.Name = "label5";
            label5.Size = new Size(58, 15);
            label5.TabIndex = 11;
            label5.Text = "Multiplier";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(520, 16);
            label6.Name = "label6";
            label6.Size = new Size(71, 15);
            label6.TabIndex = 12;
            label6.Text = "Selling Price";
            // 
            // frmAddMasterItem
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(626, 110);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(numPrice);
            Controls.Add(btnSave);
            Controls.Add(numMultiplier);
            Controls.Add(txtUOM);
            Controls.Add(txtBaseName);
            Controls.Add(txtBrand);
            Controls.Add(txtSKU);
            Name = "frmAddMasterItem";
            Text = "Add Master Item";
            ((System.ComponentModel.ISupportInitialize)numMultiplier).EndInit();
            ((System.ComponentModel.ISupportInitialize)numPrice).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtSKU;
        private TextBox txtBrand;
        private TextBox txtBaseName;
        private TextBox txtUOM;
        private NumericUpDown numMultiplier;
        private Button btnSave;
        private NumericUpDown numPrice;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
    }
}
