namespace SKC_Branch
{
    partial class frmProduction
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
            lblKind = new System.Windows.Forms.Label();
            cmbKind = new System.Windows.Forms.ComboBox();
            lblRecipe = new System.Windows.Forms.Label();
            cmbRecipe = new System.Windows.Forms.ComboBox();
            lblMultiplier = new System.Windows.Forms.Label();
            numMultiplier = new System.Windows.Forms.NumericUpDown();
            lblOutputQty = new System.Windows.Forms.Label();
            numOutputQty = new System.Windows.Forms.NumericUpDown();
            lblStaff = new System.Windows.Forms.Label();
            txtStaff = new System.Windows.Forms.TextBox();
            lblPreviewHeader = new System.Windows.Forms.Label();
            dgvPreview = new System.Windows.Forms.DataGridView();
            btnHistory = new System.Windows.Forms.Button();
            btnSubmit = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)numMultiplier).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numOutputQty).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvPreview).BeginInit();
            SuspendLayout();
            //
            // lblKind
            //
            lblKind.AutoSize = true;
            lblKind.Location = new System.Drawing.Point(15, 18);
            lblKind.Name = "lblKind";
            lblKind.Size = new System.Drawing.Size(32, 15);
            lblKind.TabIndex = 0;
            lblKind.Text = "Kind";
            //
            // cmbKind
            //
            cmbKind.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbKind.Location = new System.Drawing.Point(140, 15);
            cmbKind.Name = "cmbKind";
            cmbKind.Size = new System.Drawing.Size(180, 23);
            cmbKind.TabIndex = 1;
            cmbKind.SelectedIndexChanged += cmbKind_SelectedIndexChanged;
            //
            // lblRecipe
            //
            lblRecipe.AutoSize = true;
            lblRecipe.Location = new System.Drawing.Point(15, 52);
            lblRecipe.Name = "lblRecipe";
            lblRecipe.Size = new System.Drawing.Size(46, 15);
            lblRecipe.TabIndex = 2;
            lblRecipe.Text = "Recipe";
            //
            // cmbRecipe
            //
            cmbRecipe.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbRecipe.Location = new System.Drawing.Point(140, 49);
            cmbRecipe.Name = "cmbRecipe";
            cmbRecipe.Size = new System.Drawing.Size(380, 23);
            cmbRecipe.TabIndex = 3;
            cmbRecipe.SelectedIndexChanged += cmbRecipe_SelectedIndexChanged;
            //
            // lblMultiplier
            //
            lblMultiplier.AutoSize = true;
            lblMultiplier.Location = new System.Drawing.Point(15, 86);
            lblMultiplier.Name = "lblMultiplier";
            lblMultiplier.Size = new System.Drawing.Size(93, 15);
            lblMultiplier.TabIndex = 4;
            lblMultiplier.Text = "Batch Multiplier";
            //
            // numMultiplier
            //
            numMultiplier.DecimalPlaces = 2;
            numMultiplier.Location = new System.Drawing.Point(140, 83);
            numMultiplier.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            numMultiplier.Minimum = new decimal(new int[] { 1, 0, 0, 65536 });
            numMultiplier.Name = "numMultiplier";
            numMultiplier.Size = new System.Drawing.Size(120, 23);
            numMultiplier.TabIndex = 5;
            numMultiplier.Value = new decimal(new int[] { 1, 0, 0, 0 });
            numMultiplier.ValueChanged += numMultiplier_ValueChanged;
            //
            // lblOutputQty
            //
            lblOutputQty.AutoSize = true;
            lblOutputQty.Location = new System.Drawing.Point(280, 86);
            lblOutputQty.Name = "lblOutputQty";
            lblOutputQty.Size = new System.Drawing.Size(105, 15);
            lblOutputQty.TabIndex = 6;
            lblOutputQty.Text = "Actual Output Qty";
            //
            // numOutputQty
            //
            numOutputQty.Location = new System.Drawing.Point(395, 83);
            numOutputQty.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            numOutputQty.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
            numOutputQty.Name = "numOutputQty";
            numOutputQty.Size = new System.Drawing.Size(120, 23);
            numOutputQty.TabIndex = 7;
            //
            // lblStaff
            //
            lblStaff.AutoSize = true;
            lblStaff.Location = new System.Drawing.Point(15, 120);
            lblStaff.Name = "lblStaff";
            lblStaff.Size = new System.Drawing.Size(97, 15);
            lblStaff.TabIndex = 8;
            lblStaff.Text = "Baked/Decorated By";
            //
            // txtStaff
            //
            txtStaff.Location = new System.Drawing.Point(140, 117);
            txtStaff.Name = "txtStaff";
            txtStaff.Size = new System.Drawing.Size(250, 23);
            txtStaff.TabIndex = 9;
            //
            // lblPreviewHeader
            //
            lblPreviewHeader.AutoSize = true;
            lblPreviewHeader.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblPreviewHeader.Location = new System.Drawing.Point(15, 155);
            lblPreviewHeader.Name = "lblPreviewHeader";
            lblPreviewHeader.Size = new System.Drawing.Size(170, 15);
            lblPreviewHeader.TabIndex = 10;
            lblPreviewHeader.Text = "Will Consume From Your Stock";
            //
            // dgvPreview
            //
            dgvPreview.AllowUserToAddRows = false;
            dgvPreview.AllowUserToDeleteRows = false;
            dgvPreview.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dgvPreview.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPreview.Location = new System.Drawing.Point(15, 175);
            dgvPreview.Name = "dgvPreview";
            dgvPreview.ReadOnly = true;
            dgvPreview.RowHeadersVisible = false;
            dgvPreview.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvPreview.Size = new System.Drawing.Size(505, 220);
            dgvPreview.TabIndex = 11;
            //
            // btnHistory
            //
            btnHistory.Location = new System.Drawing.Point(15, 410);
            btnHistory.Name = "btnHistory";
            btnHistory.Size = new System.Drawing.Size(140, 30);
            btnHistory.TabIndex = 12;
            btnHistory.Text = "View History...";
            btnHistory.UseVisualStyleBackColor = true;
            btnHistory.Click += btnHistory_Click;
            //
            // btnSubmit
            //
            btnSubmit.Location = new System.Drawing.Point(400, 410);
            btnSubmit.Name = "btnSubmit";
            btnSubmit.Size = new System.Drawing.Size(120, 30);
            btnSubmit.TabIndex = 13;
            btnSubmit.Text = "Submit";
            btnSubmit.UseVisualStyleBackColor = true;
            btnSubmit.Click += btnSubmit_Click;
            //
            // frmProduction
            //
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(536, 455);
            Controls.Add(lblKind);
            Controls.Add(cmbKind);
            Controls.Add(lblRecipe);
            Controls.Add(cmbRecipe);
            Controls.Add(lblMultiplier);
            Controls.Add(numMultiplier);
            Controls.Add(lblOutputQty);
            Controls.Add(numOutputQty);
            Controls.Add(lblStaff);
            Controls.Add(txtStaff);
            Controls.Add(lblPreviewHeader);
            Controls.Add(dgvPreview);
            Controls.Add(btnHistory);
            Controls.Add(btnSubmit);
            MinimumSize = new System.Drawing.Size(552, 494);
            Name = "frmProduction";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Record Baking / Decorating";
            Load += frmProduction_Load;
            ((System.ComponentModel.ISupportInitialize)numMultiplier).EndInit();
            ((System.ComponentModel.ISupportInitialize)numOutputQty).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvPreview).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblKind;
        private System.Windows.Forms.ComboBox cmbKind;
        private System.Windows.Forms.Label lblRecipe;
        private System.Windows.Forms.ComboBox cmbRecipe;
        private System.Windows.Forms.Label lblMultiplier;
        private System.Windows.Forms.NumericUpDown numMultiplier;
        private System.Windows.Forms.Label lblOutputQty;
        private System.Windows.Forms.NumericUpDown numOutputQty;
        private System.Windows.Forms.Label lblStaff;
        private System.Windows.Forms.TextBox txtStaff;
        private System.Windows.Forms.Label lblPreviewHeader;
        private System.Windows.Forms.DataGridView dgvPreview;
        private System.Windows.Forms.Button btnHistory;
        private System.Windows.Forms.Button btnSubmit;
    }
}
