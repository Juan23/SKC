namespace SKC_Bakery_Supplies
{
    partial class frmAdjustInventory
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
            lblItem = new Label();
            lblSystemQty = new Label();
            lblActualQty = new Label();
            numActualQty = new NumericUpDown();
            lblUnitCost = new Label();
            numUnitCost = new NumericUpDown();
            lblReason = new Label();
            txtReason = new TextBox();
            btnSave = new Button();
            btnCancel = new Button();
            ((System.ComponentModel.ISupportInitialize)numActualQty).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numUnitCost).BeginInit();
            SuspendLayout();
            //
            // lblItem
            //
            lblItem.AutoEllipsis = true;
            lblItem.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblItem.ForeColor = Color.FromArgb(45, 52, 64);
            lblItem.Location = new Point(20, 15);
            lblItem.Name = "lblItem";
            lblItem.Size = new Size(380, 25);
            lblItem.TabIndex = 0;
            lblItem.Text = "Item";
            //
            // lblSystemQty
            //
            lblSystemQty.AutoSize = true;
            lblSystemQty.Font = new Font("Segoe UI", 10F);
            lblSystemQty.Location = new Point(20, 50);
            lblSystemQty.Name = "lblSystemQty";
            lblSystemQty.Size = new Size(100, 19);
            lblSystemQty.TabIndex = 1;
            lblSystemQty.Text = "System Qty: 0";
            //
            // lblActualQty
            //
            lblActualQty.AutoSize = true;
            lblActualQty.Font = new Font("Segoe UI", 10F);
            lblActualQty.Location = new Point(20, 85);
            lblActualQty.Name = "lblActualQty";
            lblActualQty.Size = new Size(135, 19);
            lblActualQty.TabIndex = 2;
            lblActualQty.Text = "Actual Counted Qty";
            //
            // numActualQty
            //
            numActualQty.Font = new Font("Segoe UI", 10F);
            numActualQty.Location = new Point(20, 107);
            numActualQty.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            numActualQty.Name = "numActualQty";
            numActualQty.Size = new Size(130, 25);
            numActualQty.TabIndex = 3;
            //
            // lblUnitCost
            //
            lblUnitCost.AutoSize = true;
            lblUnitCost.Font = new Font("Segoe UI", 10F);
            lblUnitCost.Location = new Point(20, 147);
            lblUnitCost.MaximumSize = new Size(380, 0);
            lblUnitCost.Name = "lblUnitCost";
            lblUnitCost.Size = new Size(370, 38);
            lblUnitCost.TabIndex = 4;
            lblUnitCost.Text = "Unit Cost (only used if raising stock; leave 0 to use last purchase cost)";
            //
            // numUnitCost
            //
            numUnitCost.DecimalPlaces = 2;
            numUnitCost.Font = new Font("Segoe UI", 10F);
            numUnitCost.Location = new Point(20, 192);
            numUnitCost.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            numUnitCost.Name = "numUnitCost";
            numUnitCost.Size = new Size(130, 25);
            numUnitCost.TabIndex = 5;
            //
            // lblReason
            //
            lblReason.AutoSize = true;
            lblReason.Font = new Font("Segoe UI", 10F);
            lblReason.Location = new Point(20, 232);
            lblReason.Name = "lblReason";
            lblReason.Size = new Size(56, 19);
            lblReason.TabIndex = 6;
            lblReason.Text = "Reason";
            //
            // txtReason
            //
            txtReason.Font = new Font("Segoe UI", 10F);
            txtReason.Location = new Point(20, 254);
            txtReason.Name = "txtReason";
            txtReason.Size = new Size(380, 25);
            txtReason.TabIndex = 7;
            //
            // btnSave
            //
            btnSave.BackColor = Color.SeaGreen;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Segoe UI", 10F);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(190, 296);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(100, 32);
            btnSave.TabIndex = 8;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            //
            // btnCancel
            //
            btnCancel.BackColor = Color.White;
            btnCancel.FlatAppearance.BorderColor = Color.Silver;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Font = new Font("Segoe UI", 10F);
            btnCancel.Location = new Point(300, 296);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(100, 32);
            btnCancel.TabIndex = 9;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += btnCancel_Click;
            //
            // frmAdjustInventory
            //
            AcceptButton = btnSave;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.WhiteSmoke;
            CancelButton = btnCancel;
            ClientSize = new Size(420, 348);
            Controls.Add(lblItem);
            Controls.Add(lblSystemQty);
            Controls.Add(lblActualQty);
            Controls.Add(numActualQty);
            Controls.Add(lblUnitCost);
            Controls.Add(numUnitCost);
            Controls.Add(lblReason);
            Controls.Add(txtReason);
            Controls.Add(btnSave);
            Controls.Add(btnCancel);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmAdjustInventory";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Adjust Stock";
            ((System.ComponentModel.ISupportInitialize)numActualQty).EndInit();
            ((System.ComponentModel.ISupportInitialize)numUnitCost).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblItem;
        private Label lblSystemQty;
        private Label lblActualQty;
        private NumericUpDown numActualQty;
        private Label lblUnitCost;
        private NumericUpDown numUnitCost;
        private Label lblReason;
        private TextBox txtReason;
        private Button btnSave;
        private Button btnCancel;
    }
}
