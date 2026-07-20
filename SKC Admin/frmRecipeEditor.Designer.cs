namespace SKC_Admin
{
    partial class frmRecipeEditor
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
            lblName = new System.Windows.Forms.Label();
            txtName = new System.Windows.Forms.TextBox();
            lblKind = new System.Windows.Forms.Label();
            cmbKind = new System.Windows.Forms.ComboBox();
            lblOutput = new System.Windows.Forms.Label();
            cmbOutputSku = new System.Windows.Forms.ComboBox();
            lblOutputQty = new System.Windows.Forms.Label();
            numOutputQty = new System.Windows.Forms.NumericUpDown();
            lblLinesHeader = new System.Windows.Forms.Label();
            lblInput = new System.Windows.Forms.Label();
            cmbInputSku = new System.Windows.Forms.ComboBox();
            lblLineQty = new System.Windows.Forms.Label();
            numLineQty = new System.Windows.Forms.NumericUpDown();
            btnAddLine = new System.Windows.Forms.Button();
            btnRemoveLine = new System.Windows.Forms.Button();
            dgvLines = new System.Windows.Forms.DataGridView();
            btnSave = new System.Windows.Forms.Button();
            btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)numOutputQty).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numLineQty).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvLines).BeginInit();
            SuspendLayout();
            //
            // lblName
            //
            lblName.AutoSize = true;
            lblName.Location = new System.Drawing.Point(15, 18);
            lblName.Name = "lblName";
            lblName.Size = new System.Drawing.Size(42, 15);
            lblName.TabIndex = 0;
            lblName.Text = "Name";
            //
            // txtName
            //
            txtName.Location = new System.Drawing.Point(130, 15);
            txtName.Name = "txtName";
            txtName.Size = new System.Drawing.Size(340, 23);
            txtName.TabIndex = 1;
            //
            // lblKind
            //
            lblKind.AutoSize = true;
            lblKind.Location = new System.Drawing.Point(15, 50);
            lblKind.Name = "lblKind";
            lblKind.Size = new System.Drawing.Size(32, 15);
            lblKind.TabIndex = 2;
            lblKind.Text = "Kind";
            //
            // cmbKind
            //
            cmbKind.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbKind.Location = new System.Drawing.Point(130, 47);
            cmbKind.Name = "cmbKind";
            cmbKind.Size = new System.Drawing.Size(150, 23);
            cmbKind.TabIndex = 3;
            //
            // lblOutput
            //
            lblOutput.AutoSize = true;
            lblOutput.Location = new System.Drawing.Point(15, 82);
            lblOutput.Name = "lblOutput";
            lblOutput.Size = new System.Drawing.Size(88, 15);
            lblOutput.TabIndex = 4;
            lblOutput.Text = "Output Product";
            //
            // cmbOutputSku
            //
            cmbOutputSku.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbOutputSku.Location = new System.Drawing.Point(130, 79);
            cmbOutputSku.Name = "cmbOutputSku";
            cmbOutputSku.Size = new System.Drawing.Size(340, 23);
            cmbOutputSku.TabIndex = 5;
            //
            // lblOutputQty
            //
            lblOutputQty.AutoSize = true;
            lblOutputQty.Location = new System.Drawing.Point(15, 114);
            lblOutputQty.Name = "lblOutputQty";
            lblOutputQty.Size = new System.Drawing.Size(109, 15);
            lblOutputQty.TabIndex = 6;
            lblOutputQty.Text = "Output Qty / Batch";
            //
            // numOutputQty
            //
            numOutputQty.Location = new System.Drawing.Point(130, 111);
            numOutputQty.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            numOutputQty.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numOutputQty.Name = "numOutputQty";
            numOutputQty.Size = new System.Drawing.Size(120, 23);
            numOutputQty.TabIndex = 7;
            numOutputQty.Value = new decimal(new int[] { 1, 0, 0, 0 });
            //
            // lblLinesHeader
            //
            lblLinesHeader.AutoSize = true;
            lblLinesHeader.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblLinesHeader.Location = new System.Drawing.Point(15, 150);
            lblLinesHeader.Name = "lblLinesHeader";
            lblLinesHeader.Size = new System.Drawing.Size(150, 15);
            lblLinesHeader.TabIndex = 8;
            lblLinesHeader.Text = "Input Lines (per batch)";
            //
            // lblInput
            //
            lblInput.AutoSize = true;
            lblInput.Location = new System.Drawing.Point(15, 178);
            lblInput.Name = "lblInput";
            lblInput.Size = new System.Drawing.Size(33, 15);
            lblInput.TabIndex = 9;
            lblInput.Text = "Input";
            //
            // cmbInputSku
            //
            cmbInputSku.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbInputSku.Location = new System.Drawing.Point(60, 175);
            cmbInputSku.Name = "cmbInputSku";
            cmbInputSku.Size = new System.Drawing.Size(280, 23);
            cmbInputSku.TabIndex = 10;
            //
            // lblLineQty
            //
            lblLineQty.AutoSize = true;
            lblLineQty.Location = new System.Drawing.Point(350, 178);
            lblLineQty.Name = "lblLineQty";
            lblLineQty.Size = new System.Drawing.Size(26, 15);
            lblLineQty.TabIndex = 11;
            lblLineQty.Text = "Qty";
            //
            // numLineQty
            //
            numLineQty.Location = new System.Drawing.Point(382, 175);
            numLineQty.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            numLineQty.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numLineQty.Name = "numLineQty";
            numLineQty.Size = new System.Drawing.Size(88, 23);
            numLineQty.TabIndex = 12;
            numLineQty.Value = new decimal(new int[] { 1, 0, 0, 0 });
            //
            // btnAddLine
            //
            btnAddLine.BackColor = System.Drawing.Color.White;
            btnAddLine.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            btnAddLine.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnAddLine.Location = new System.Drawing.Point(15, 205);
            btnAddLine.Name = "btnAddLine";
            btnAddLine.Size = new System.Drawing.Size(95, 27);
            btnAddLine.TabIndex = 13;
            btnAddLine.Text = "Add Line";
            btnAddLine.UseVisualStyleBackColor = false;
            btnAddLine.Click += btnAddLine_Click;
            //
            // btnRemoveLine
            //
            btnRemoveLine.BackColor = System.Drawing.Color.Firebrick;
            btnRemoveLine.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnRemoveLine.ForeColor = System.Drawing.Color.White;
            btnRemoveLine.Location = new System.Drawing.Point(116, 205);
            btnRemoveLine.Name = "btnRemoveLine";
            btnRemoveLine.Size = new System.Drawing.Size(120, 27);
            btnRemoveLine.TabIndex = 14;
            btnRemoveLine.Text = "Remove Selected";
            btnRemoveLine.UseVisualStyleBackColor = false;
            btnRemoveLine.Click += btnRemoveLine_Click;
            //
            // dgvLines
            //
            dgvLines.AllowUserToAddRows = false;
            dgvLines.AllowUserToDeleteRows = false;
            dgvLines.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dgvLines.BackgroundColor = System.Drawing.Color.White;
            dgvLines.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvLines.Location = new System.Drawing.Point(15, 240);
            dgvLines.Name = "dgvLines";
            dgvLines.ReadOnly = true;
            dgvLines.RowHeadersVisible = false;
            dgvLines.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvLines.Size = new System.Drawing.Size(455, 190);
            dgvLines.TabIndex = 15;
            //
            // btnSave
            //
            btnSave.BackColor = System.Drawing.Color.SeaGreen;
            btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnSave.ForeColor = System.Drawing.Color.White;
            btnSave.Location = new System.Drawing.Point(260, 440);
            btnSave.Name = "btnSave";
            btnSave.Size = new System.Drawing.Size(100, 32);
            btnSave.TabIndex = 16;
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
            btnCancel.Location = new System.Drawing.Point(370, 440);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(100, 32);
            btnCancel.TabIndex = 17;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = false;
            //
            // frmRecipeEditor
            //
            AcceptButton = btnSave;
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.WhiteSmoke;
            CancelButton = btnCancel;
            ClientSize = new System.Drawing.Size(486, 485);
            Controls.Add(lblName);
            Controls.Add(txtName);
            Controls.Add(lblKind);
            Controls.Add(cmbKind);
            Controls.Add(lblOutput);
            Controls.Add(cmbOutputSku);
            Controls.Add(lblOutputQty);
            Controls.Add(numOutputQty);
            Controls.Add(lblLinesHeader);
            Controls.Add(lblInput);
            Controls.Add(cmbInputSku);
            Controls.Add(lblLineQty);
            Controls.Add(numLineQty);
            Controls.Add(btnAddLine);
            Controls.Add(btnRemoveLine);
            Controls.Add(dgvLines);
            Controls.Add(btnSave);
            Controls.Add(btnCancel);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmRecipeEditor";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Recipe";
            ((System.ComponentModel.ISupportInitialize)numOutputQty).EndInit();
            ((System.ComponentModel.ISupportInitialize)numLineQty).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvLines).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblKind;
        private System.Windows.Forms.ComboBox cmbKind;
        private System.Windows.Forms.Label lblOutput;
        private System.Windows.Forms.ComboBox cmbOutputSku;
        private System.Windows.Forms.Label lblOutputQty;
        private System.Windows.Forms.NumericUpDown numOutputQty;
        private System.Windows.Forms.Label lblLinesHeader;
        private System.Windows.Forms.Label lblInput;
        private System.Windows.Forms.ComboBox cmbInputSku;
        private System.Windows.Forms.Label lblLineQty;
        private System.Windows.Forms.NumericUpDown numLineQty;
        private System.Windows.Forms.Button btnAddLine;
        private System.Windows.Forms.Button btnRemoveLine;
        private System.Windows.Forms.DataGridView dgvLines;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
    }
}
