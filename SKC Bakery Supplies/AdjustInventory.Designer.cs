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
            lblItem.AutoSize = true;
            lblItem.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblItem.Location = new Point(12, 12);
            lblItem.Name = "lblItem";
            lblItem.Size = new Size(38, 15);
            lblItem.TabIndex = 0;
            lblItem.Text = "Item";
            //
            // lblSystemQty
            //
            lblSystemQty.AutoSize = true;
            lblSystemQty.Location = new Point(12, 40);
            lblSystemQty.Name = "lblSystemQty";
            lblSystemQty.Size = new Size(90, 15);
            lblSystemQty.TabIndex = 1;
            lblSystemQty.Text = "System Qty: 0";
            //
            // lblActualQty
            //
            lblActualQty.AutoSize = true;
            lblActualQty.Location = new Point(12, 76);
            lblActualQty.Name = "lblActualQty";
            lblActualQty.Size = new Size(97, 15);
            lblActualQty.TabIndex = 2;
            lblActualQty.Text = "Actual Counted Qty";
            //
            // numActualQty
            //
            numActualQty.Location = new Point(12, 94);
            numActualQty.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            numActualQty.Name = "numActualQty";
            numActualQty.Size = new Size(120, 23);
            numActualQty.TabIndex = 3;
            //
            // lblUnitCost
            //
            lblUnitCost.AutoSize = true;
            lblUnitCost.Location = new Point(148, 76);
            lblUnitCost.MaximumSize = new Size(260, 0);
            lblUnitCost.Name = "lblUnitCost";
            lblUnitCost.Size = new Size(250, 30);
            lblUnitCost.TabIndex = 4;
            lblUnitCost.Text = "Unit Cost (only used if raising stock; leave 0 to use last purchase cost)";
            //
            // numUnitCost
            //
            numUnitCost.DecimalPlaces = 2;
            numUnitCost.Location = new Point(148, 110);
            numUnitCost.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            numUnitCost.Name = "numUnitCost";
            numUnitCost.Size = new Size(120, 23);
            numUnitCost.TabIndex = 5;
            //
            // lblReason
            //
            lblReason.AutoSize = true;
            lblReason.Location = new Point(12, 140);
            lblReason.Name = "lblReason";
            lblReason.Size = new Size(49, 15);
            lblReason.TabIndex = 6;
            lblReason.Text = "Reason";
            //
            // txtReason
            //
            txtReason.Location = new Point(12, 158);
            txtReason.Name = "txtReason";
            txtReason.Size = new Size(360, 23);
            txtReason.TabIndex = 7;
            //
            // btnSave
            //
            btnSave.Location = new Point(216, 194);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(75, 23);
            btnSave.TabIndex = 8;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            //
            // btnCancel
            //
            btnCancel.Location = new Point(297, 194);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 9;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            //
            // frmAdjustInventory
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(384, 229);
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
            Name = "frmAdjustInventory";
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
