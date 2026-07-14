namespace SKC_Bakery_Supplies
{
    partial class frmAdjustmentHistory
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
            lblStart = new Label();
            dtpStart = new DateTimePicker();
            lblEnd = new Label();
            dtpEnd = new DateTimePicker();
            btnLoad = new Button();
            dgvAdjustments = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dgvAdjustments).BeginInit();
            SuspendLayout();
            //
            // lblStart
            //
            lblStart.AutoSize = true;
            lblStart.Location = new Point(8, 8);
            lblStart.Name = "lblStart";
            lblStart.Size = new Size(58, 15);
            lblStart.TabIndex = 0;
            lblStart.Text = "Start Date";
            //
            // dtpStart
            //
            dtpStart.Location = new Point(8, 24);
            dtpStart.Name = "dtpStart";
            dtpStart.Size = new Size(160, 23);
            dtpStart.TabIndex = 1;
            //
            // lblEnd
            //
            lblEnd.AutoSize = true;
            lblEnd.Location = new Point(180, 8);
            lblEnd.Name = "lblEnd";
            lblEnd.Size = new Size(54, 15);
            lblEnd.TabIndex = 2;
            lblEnd.Text = "End Date";
            //
            // dtpEnd
            //
            dtpEnd.Location = new Point(180, 24);
            dtpEnd.Name = "dtpEnd";
            dtpEnd.Size = new Size(160, 23);
            dtpEnd.TabIndex = 3;
            //
            // btnLoad
            //
            btnLoad.Location = new Point(352, 24);
            btnLoad.Name = "btnLoad";
            btnLoad.Size = new Size(75, 23);
            btnLoad.TabIndex = 4;
            btnLoad.Text = "Search";
            btnLoad.UseVisualStyleBackColor = true;
            btnLoad.Click += btnLoad_Click;
            //
            // dgvAdjustments
            //
            dgvAdjustments.AllowUserToAddRows = false;
            dgvAdjustments.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvAdjustments.Location = new Point(8, 56);
            dgvAdjustments.Name = "dgvAdjustments";
            dgvAdjustments.ReadOnly = true;
            dgvAdjustments.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAdjustments.Size = new Size(760, 416);
            dgvAdjustments.TabIndex = 5;
            //
            // frmAdjustmentHistory
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 481);
            Controls.Add(dgvAdjustments);
            Controls.Add(btnLoad);
            Controls.Add(dtpEnd);
            Controls.Add(lblEnd);
            Controls.Add(dtpStart);
            Controls.Add(lblStart);
            Name = "frmAdjustmentHistory";
            Text = "Adjustment History";
            Load += frmAdjustmentHistory_Load;
            ((System.ComponentModel.ISupportInitialize)dgvAdjustments).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblStart;
        private DateTimePicker dtpStart;
        private Label lblEnd;
        private DateTimePicker dtpEnd;
        private Button btnLoad;
        private DataGridView dgvAdjustments;
    }
}
