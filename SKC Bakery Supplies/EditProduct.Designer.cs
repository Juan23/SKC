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
            lblHeader = new Label();
            label6 = new Label();
            label3 = new Label();
            label2 = new Label();
            label7 = new Label();
            numPrice = new NumericUpDown();
            btnSave = new Button();
            txtBaseName = new TextBox();
            txtBrand = new TextBox();
            txtSKU = new TextBox();
            ((System.ComponentModel.ISupportInitialize)numPrice).BeginInit();
            SuspendLayout();
            //
            // lblHeader
            //
            lblHeader.AutoSize = true;
            lblHeader.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblHeader.ForeColor = Color.FromArgb(45, 52, 64);
            lblHeader.Location = new Point(20, 15);
            lblHeader.Name = "lblHeader";
            lblHeader.Size = new Size(115, 21);
            lblHeader.TabIndex = 0;
            lblHeader.Text = "Edit Product";
            //
            // label7
            //
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 10F);
            label7.Location = new Point(20, 58);
            label7.Name = "label7";
            label7.Size = new Size(33, 19);
            label7.TabIndex = 1;
            label7.Text = "SKU";
            //
            // txtSKU
            //
            txtSKU.Font = new Font("Segoe UI", 10F);
            txtSKU.Location = new Point(150, 55);
            txtSKU.Name = "txtSKU";
            txtSKU.ReadOnly = true;
            txtSKU.Size = new Size(220, 25);
            txtSKU.TabIndex = 2;
            //
            // label2
            //
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 10F);
            label2.Location = new Point(20, 96);
            label2.Name = "label2";
            label2.Size = new Size(45, 19);
            label2.TabIndex = 3;
            label2.Text = "Brand";
            //
            // txtBrand
            //
            txtBrand.Font = new Font("Segoe UI", 10F);
            txtBrand.Location = new Point(150, 93);
            txtBrand.Name = "txtBrand";
            txtBrand.Size = new Size(220, 25);
            txtBrand.TabIndex = 4;
            //
            // label3
            //
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 10F);
            label3.Location = new Point(20, 134);
            label3.Name = "label3";
            label3.Size = new Size(78, 19);
            label3.TabIndex = 5;
            label3.Text = "Base Name";
            //
            // txtBaseName
            //
            txtBaseName.Font = new Font("Segoe UI", 10F);
            txtBaseName.Location = new Point(150, 131);
            txtBaseName.Name = "txtBaseName";
            txtBaseName.Size = new Size(220, 25);
            txtBaseName.TabIndex = 6;
            //
            // label6
            //
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 10F);
            label6.Location = new Point(20, 172);
            label6.Name = "label6";
            label6.Size = new Size(85, 19);
            label6.TabIndex = 7;
            label6.Text = "Selling Price";
            //
            // numPrice
            //
            numPrice.Font = new Font("Segoe UI", 10F);
            numPrice.Location = new Point(150, 169);
            numPrice.Name = "numPrice";
            numPrice.Size = new Size(120, 25);
            numPrice.TabIndex = 8;
            //
            // btnSave
            //
            btnSave.BackColor = Color.SeaGreen;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Segoe UI", 10F);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(270, 218);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(100, 32);
            btnSave.TabIndex = 9;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            //
            // frmEditProduct
            //
            AcceptButton = btnSave;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(390, 270);
            Controls.Add(lblHeader);
            Controls.Add(label6);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label7);
            Controls.Add(numPrice);
            Controls.Add(btnSave);
            Controls.Add(txtBaseName);
            Controls.Add(txtBrand);
            Controls.Add(txtSKU);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmEditProduct";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Edit Product";
            ((System.ComponentModel.ISupportInitialize)numPrice).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblHeader;
        private Label label6;
        private Label label3;
        private Label label2;
        private Label label7;
        private NumericUpDown numPrice;
        private Button btnSave;
        private TextBox txtBaseName;
        private TextBox txtBrand;
        private TextBox txtSKU;
    }
}
