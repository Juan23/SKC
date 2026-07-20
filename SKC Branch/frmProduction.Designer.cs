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
            lblKind.Font = new System.Drawing.Font("Segoe UI", 10F);
            lblKind.Location = new System.Drawing.Point(20, 24);
            lblKind.Name = "lblKind";
            lblKind.Size = new System.Drawing.Size(40, 23);
            lblKind.TabIndex = 0;
            lblKind.Text = "Kind";
            //
            // cmbKind
            //
            cmbKind.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbKind.Font = new System.Drawing.Font("Segoe UI", 11F);
            cmbKind.Location = new System.Drawing.Point(170, 20);
            cmbKind.Name = "cmbKind";
            cmbKind.Size = new System.Drawing.Size(220, 28);
            cmbKind.TabIndex = 1;
            cmbKind.SelectedIndexChanged += cmbKind_SelectedIndexChanged;
            //
            // lblRecipe
            //
            lblRecipe.AutoSize = true;
            lblRecipe.Font = new System.Drawing.Font("Segoe UI", 10F);
            lblRecipe.Location = new System.Drawing.Point(20, 64);
            lblRecipe.Name = "lblRecipe";
            lblRecipe.Size = new System.Drawing.Size(56, 23);
            lblRecipe.TabIndex = 2;
            lblRecipe.Text = "Recipe";
            //
            // cmbRecipe
            //
            cmbRecipe.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbRecipe.Font = new System.Drawing.Font("Segoe UI", 11F);
            cmbRecipe.Location = new System.Drawing.Point(170, 60);
            cmbRecipe.Name = "cmbRecipe";
            cmbRecipe.Size = new System.Drawing.Size(560, 28);
            cmbRecipe.TabIndex = 3;
            cmbRecipe.SelectedIndexChanged += cmbRecipe_SelectedIndexChanged;
            //
            // lblMultiplier
            //
            lblMultiplier.AutoSize = true;
            lblMultiplier.Font = new System.Drawing.Font("Segoe UI", 10F);
            lblMultiplier.Location = new System.Drawing.Point(20, 104);
            lblMultiplier.Name = "lblMultiplier";
            lblMultiplier.Size = new System.Drawing.Size(115, 23);
            lblMultiplier.TabIndex = 4;
            lblMultiplier.Text = "Batch Multiplier";
            //
            // numMultiplier
            //
            numMultiplier.DecimalPlaces = 2;
            numMultiplier.Font = new System.Drawing.Font("Segoe UI", 11F);
            numMultiplier.Location = new System.Drawing.Point(170, 100);
            numMultiplier.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            numMultiplier.Minimum = new decimal(new int[] { 1, 0, 0, 65536 });
            numMultiplier.Name = "numMultiplier";
            numMultiplier.Size = new System.Drawing.Size(120, 28);
            numMultiplier.TabIndex = 5;
            numMultiplier.Value = new decimal(new int[] { 1, 0, 0, 0 });
            numMultiplier.ValueChanged += numMultiplier_ValueChanged;
            //
            // lblOutputQty
            //
            lblOutputQty.AutoSize = true;
            lblOutputQty.Font = new System.Drawing.Font("Segoe UI", 10F);
            lblOutputQty.Location = new System.Drawing.Point(340, 104);
            lblOutputQty.Name = "lblOutputQty";
            lblOutputQty.Size = new System.Drawing.Size(130, 23);
            lblOutputQty.TabIndex = 6;
            lblOutputQty.Text = "Actual Output Qty";
            //
            // numOutputQty
            //
            numOutputQty.Font = new System.Drawing.Font("Segoe UI", 11F);
            numOutputQty.Location = new System.Drawing.Point(490, 100);
            numOutputQty.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            numOutputQty.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
            numOutputQty.Name = "numOutputQty";
            numOutputQty.Size = new System.Drawing.Size(120, 28);
            numOutputQty.TabIndex = 7;
            //
            // lblStaff
            //
            lblStaff.AutoSize = true;
            lblStaff.Font = new System.Drawing.Font("Segoe UI", 10F);
            lblStaff.Location = new System.Drawing.Point(20, 144);
            lblStaff.Name = "lblStaff";
            lblStaff.Size = new System.Drawing.Size(135, 23);
            lblStaff.TabIndex = 8;
            lblStaff.Text = "Baked/Decorated By";
            //
            // txtStaff
            //
            txtStaff.Font = new System.Drawing.Font("Segoe UI", 11F);
            txtStaff.Location = new System.Drawing.Point(170, 140);
            txtStaff.Name = "txtStaff";
            txtStaff.Size = new System.Drawing.Size(300, 28);
            txtStaff.TabIndex = 9;
            //
            // lblPreviewHeader
            //
            lblPreviewHeader.AutoSize = true;
            lblPreviewHeader.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            lblPreviewHeader.Location = new System.Drawing.Point(20, 185);
            lblPreviewHeader.Name = "lblPreviewHeader";
            lblPreviewHeader.Size = new System.Drawing.Size(250, 25);
            lblPreviewHeader.TabIndex = 10;
            lblPreviewHeader.Text = "Will Consume From Your Stock";
            //
            // dgvPreview
            //
            dgvPreview.AllowUserToAddRows = false;
            dgvPreview.AllowUserToDeleteRows = false;
            dgvPreview.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom
                | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            dgvPreview.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dgvPreview.BackgroundColor = System.Drawing.Color.White;
            dgvPreview.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPreview.Location = new System.Drawing.Point(20, 215);
            dgvPreview.Name = "dgvPreview";
            dgvPreview.ReadOnly = true;
            dgvPreview.RowHeadersVisible = false;
            dgvPreview.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvPreview.Size = new System.Drawing.Size(1240, 350);
            dgvPreview.TabIndex = 11;
            //
            // btnHistory
            //
            btnHistory.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            btnHistory.BackColor = System.Drawing.Color.White;
            btnHistory.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            btnHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnHistory.Font = new System.Drawing.Font("Segoe UI", 10F);
            btnHistory.Location = new System.Drawing.Point(20, 585);
            btnHistory.Name = "btnHistory";
            btnHistory.Size = new System.Drawing.Size(180, 40);
            btnHistory.TabIndex = 12;
            btnHistory.Text = "View History...";
            btnHistory.UseVisualStyleBackColor = false;
            btnHistory.Click += btnHistory_Click;
            //
            // btnSubmit
            //
            btnSubmit.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnSubmit.BackColor = System.Drawing.Color.SeaGreen;
            btnSubmit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnSubmit.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            btnSubmit.ForeColor = System.Drawing.Color.White;
            btnSubmit.Location = new System.Drawing.Point(1060, 585);
            btnSubmit.Name = "btnSubmit";
            btnSubmit.Size = new System.Drawing.Size(200, 40);
            btnSubmit.TabIndex = 13;
            btnSubmit.Text = "Submit";
            btnSubmit.UseVisualStyleBackColor = false;
            btnSubmit.Click += btnSubmit_Click;
            //
            // frmProduction
            //
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.WhiteSmoke;
            ClientSize = new System.Drawing.Size(1280, 640);
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
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "frmProduction";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            WindowState = System.Windows.Forms.FormWindowState.Maximized;
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
