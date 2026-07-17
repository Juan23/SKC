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
            lblHeader = new Label();
            lblStart = new Label();
            dtpStart = new DateTimePicker();
            lblEnd = new Label();
            dtpEnd = new DateTimePicker();
            btnLoad = new Button();
            dgvAdjustments = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dgvAdjustments).BeginInit();
            SuspendLayout();
            //
            // lblHeader
            //
            lblHeader.AutoSize = true;
            lblHeader.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblHeader.Location = new Point(20, 15);
            lblHeader.Name = "lblHeader";
            lblHeader.Size = new Size(240, 32);
            lblHeader.TabIndex = 0;
            lblHeader.Text = "Adjustment History";
            //
            // lblStart
            //
            lblStart.AutoSize = true;
            lblStart.Font = new Font("Segoe UI", 11F);
            lblStart.Location = new Point(20, 60);
            lblStart.Name = "lblStart";
            lblStart.Size = new Size(76, 20);
            lblStart.TabIndex = 1;
            lblStart.Text = "Start Date";
            //
            // dtpStart
            //
            dtpStart.Font = new Font("Segoe UI", 11F);
            dtpStart.Location = new Point(20, 82);
            dtpStart.Name = "dtpStart";
            dtpStart.Size = new Size(180, 28);
            dtpStart.TabIndex = 2;
            //
            // lblEnd
            //
            lblEnd.AutoSize = true;
            lblEnd.Font = new Font("Segoe UI", 11F);
            lblEnd.Location = new Point(220, 60);
            lblEnd.Name = "lblEnd";
            lblEnd.Size = new Size(70, 20);
            lblEnd.TabIndex = 3;
            lblEnd.Text = "End Date";
            //
            // dtpEnd
            //
            dtpEnd.Font = new Font("Segoe UI", 11F);
            dtpEnd.Location = new Point(220, 82);
            dtpEnd.Name = "dtpEnd";
            dtpEnd.Size = new Size(180, 28);
            dtpEnd.TabIndex = 4;
            //
            // btnLoad
            //
            btnLoad.Font = new Font("Segoe UI", 10F);
            btnLoad.Location = new Point(420, 80);
            btnLoad.Name = "btnLoad";
            btnLoad.Size = new Size(120, 32);
            btnLoad.TabIndex = 5;
            btnLoad.Text = "Search";
            btnLoad.UseVisualStyleBackColor = true;
            btnLoad.Click += btnLoad_Click;
            //
            // dgvAdjustments
            //
            dgvAdjustments.AllowUserToAddRows = false;
            dgvAdjustments.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvAdjustments.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvAdjustments.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvAdjustments.Location = new Point(20, 125);
            dgvAdjustments.Name = "dgvAdjustments";
            dgvAdjustments.ReadOnly = true;
            dgvAdjustments.RowHeadersVisible = false;
            dgvAdjustments.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAdjustments.Size = new Size(1240, 495);
            dgvAdjustments.TabIndex = 6;
            //
            // frmAdjustmentHistory
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1280, 640);
            Controls.Add(dgvAdjustments);
            Controls.Add(btnLoad);
            Controls.Add(dtpEnd);
            Controls.Add(lblEnd);
            Controls.Add(dtpStart);
            Controls.Add(lblStart);
            Controls.Add(lblHeader);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "frmAdjustmentHistory";
            StartPosition = FormStartPosition.CenterScreen;
            WindowState = FormWindowState.Maximized;
            Text = "Adjustment History";
            Load += frmAdjustmentHistory_Load;
            ((System.ComponentModel.ISupportInitialize)dgvAdjustments).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblHeader;
        private Label lblStart;
        private DateTimePicker dtpStart;
        private Label lblEnd;
        private DateTimePicker dtpEnd;
        private Button btnLoad;
        private DataGridView dgvAdjustments;
    }
}
