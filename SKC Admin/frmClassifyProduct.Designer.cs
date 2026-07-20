namespace SKC_Admin
{
    partial class frmClassifyProduct
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            lblItem = new System.Windows.Forms.Label();
            lblCategory = new System.Windows.Forms.Label();
            cmbCategory = new System.Windows.Forms.ComboBox();
            lblUom = new System.Windows.Forms.Label();
            txtUom = new System.Windows.Forms.TextBox();
            lblPackMultiplier = new System.Windows.Forms.Label();
            numPackMultiplier = new System.Windows.Forms.NumericUpDown();
            btnSave = new System.Windows.Forms.Button();
            btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)numPackMultiplier).BeginInit();
            SuspendLayout();
            //
            // lblItem
            //
            lblItem.AutoSize = true;
            lblItem.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            lblItem.ForeColor = System.Drawing.Color.FromArgb(45, 52, 64);
            lblItem.Location = new System.Drawing.Point(20, 15);
            lblItem.Name = "lblItem";
            lblItem.Size = new System.Drawing.Size(60, 23);
            lblItem.TabIndex = 0;
            lblItem.Text = "Item";
            //
            // lblCategory
            //
            lblCategory.AutoSize = true;
            lblCategory.Location = new System.Drawing.Point(20, 60);
            lblCategory.Name = "lblCategory";
            lblCategory.Size = new System.Drawing.Size(58, 15);
            lblCategory.TabIndex = 1;
            lblCategory.Text = "Category";
            //
            // cmbCategory
            //
            cmbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbCategory.Location = new System.Drawing.Point(150, 57);
            cmbCategory.Name = "cmbCategory";
            cmbCategory.Size = new System.Drawing.Size(210, 23);
            cmbCategory.TabIndex = 2;
            //
            // lblUom
            //
            lblUom.AutoSize = true;
            lblUom.Location = new System.Drawing.Point(20, 100);
            lblUom.Name = "lblUom";
            lblUom.Size = new System.Drawing.Size(105, 15);
            lblUom.TabIndex = 3;
            lblUom.Text = "Purchase Unit";
            //
            // txtUom
            //
            txtUom.Location = new System.Drawing.Point(150, 97);
            txtUom.Name = "txtUom";
            txtUom.PlaceholderText = "e.g. Sack (25kg)";
            txtUom.Size = new System.Drawing.Size(210, 23);
            txtUom.TabIndex = 4;
            //
            // lblPackMultiplier
            //
            lblPackMultiplier.AutoSize = true;
            lblPackMultiplier.Location = new System.Drawing.Point(20, 140);
            lblPackMultiplier.Name = "lblPackMultiplier";
            lblPackMultiplier.Size = new System.Drawing.Size(114, 15);
            lblPackMultiplier.TabIndex = 5;
            lblPackMultiplier.Text = "Base Units Per Pack";
            //
            // numPackMultiplier
            //
            numPackMultiplier.DecimalPlaces = 4;
            numPackMultiplier.Location = new System.Drawing.Point(150, 137);
            numPackMultiplier.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            numPackMultiplier.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numPackMultiplier.Name = "numPackMultiplier";
            numPackMultiplier.Size = new System.Drawing.Size(210, 23);
            numPackMultiplier.TabIndex = 6;
            numPackMultiplier.Value = new decimal(new int[] { 1, 0, 0, 0 });
            //
            // btnSave
            //
            btnSave.BackColor = System.Drawing.Color.SeaGreen;
            btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnSave.ForeColor = System.Drawing.Color.White;
            btnSave.Location = new System.Drawing.Point(150, 182);
            btnSave.Name = "btnSave";
            btnSave.Size = new System.Drawing.Size(100, 32);
            btnSave.TabIndex = 7;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            //
            // btnCancel
            //
            btnCancel.BackColor = System.Drawing.Color.White;
            btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnCancel.Location = new System.Drawing.Point(260, 182);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(100, 32);
            btnCancel.TabIndex = 8;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = false;
            //
            // frmClassifyProduct
            //
            AcceptButton = btnSave;
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.WhiteSmoke;
            CancelButton = btnCancel;
            ClientSize = new System.Drawing.Size(380, 235);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(numPackMultiplier);
            Controls.Add(lblPackMultiplier);
            Controls.Add(txtUom);
            Controls.Add(lblUom);
            Controls.Add(cmbCategory);
            Controls.Add(lblCategory);
            Controls.Add(lblItem);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmClassifyProduct";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Set Category / Unit of Measure";
            ((System.ComponentModel.ISupportInitialize)numPackMultiplier).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblItem;
        private System.Windows.Forms.Label lblCategory;
        private System.Windows.Forms.ComboBox cmbCategory;
        private System.Windows.Forms.Label lblUom;
        private System.Windows.Forms.TextBox txtUom;
        private System.Windows.Forms.Label lblPackMultiplier;
        private System.Windows.Forms.NumericUpDown numPackMultiplier;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
    }
}
