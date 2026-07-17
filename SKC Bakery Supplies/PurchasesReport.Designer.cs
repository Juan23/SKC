namespace SKC_Bakery_Supplies
{
    partial class frmPurchasesReport
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
            groupBox1 = new GroupBox();
            btnFilter = new Button();
            btnPrint = new Button();
            label2 = new Label();
            dtpEnd = new DateTimePicker();
            label1 = new Label();
            dtpStart = new DateTimePicker();
            dgvTickets = new DataGridView();
            dgvDetails = new DataGridView();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvTickets).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvDetails).BeginInit();
            SuspendLayout();
            //
            // lblHeader
            //
            lblHeader.AutoSize = true;
            lblHeader.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblHeader.Location = new Point(20, 15);
            lblHeader.Name = "lblHeader";
            lblHeader.Size = new Size(220, 32);
            lblHeader.TabIndex = 0;
            lblHeader.Text = "Purchases Report";
            //
            // groupBox1
            //
            groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            groupBox1.Controls.Add(btnFilter);
            groupBox1.Controls.Add(btnPrint);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(dtpEnd);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(dtpStart);
            groupBox1.Font = new Font("Segoe UI", 10F);
            groupBox1.Location = new Point(960, 70);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(300, 260);
            groupBox1.TabIndex = 3;
            groupBox1.TabStop = false;
            groupBox1.Text = "Settings";
            //
            // btnFilter
            //
            btnFilter.Location = new Point(156, 160);
            btnFilter.Name = "btnFilter";
            btnFilter.Size = new Size(130, 36);
            btnFilter.TabIndex = 3;
            btnFilter.Text = "Search";
            btnFilter.UseVisualStyleBackColor = true;
            btnFilter.Click += btnFilter_Click;
            //
            // btnPrint
            //
            btnPrint.Location = new Point(16, 160);
            btnPrint.Name = "btnPrint";
            btnPrint.Size = new Size(130, 36);
            btnPrint.TabIndex = 2;
            btnPrint.Text = "Print";
            btnPrint.UseVisualStyleBackColor = true;
            btnPrint.Click += btnPrint_Click;
            //
            // label2
            //
            label2.AutoSize = true;
            label2.Location = new Point(16, 90);
            label2.Name = "label2";
            label2.Size = new Size(70, 20);
            label2.TabIndex = 1;
            label2.Text = "End Date";
            //
            // dtpEnd
            //
            dtpEnd.Location = new Point(16, 110);
            dtpEnd.Name = "dtpEnd";
            dtpEnd.Size = new Size(260, 28);
            dtpEnd.TabIndex = 1;
            //
            // label1
            //
            label1.AutoSize = true;
            label1.Location = new Point(16, 30);
            label1.Name = "label1";
            label1.Size = new Size(76, 20);
            label1.TabIndex = 0;
            label1.Text = "Start Date";
            //
            // dtpStart
            //
            dtpStart.Location = new Point(16, 50);
            dtpStart.Name = "dtpStart";
            dtpStart.Size = new Size(260, 28);
            dtpStart.TabIndex = 0;
            //
            // dgvTickets
            //
            dgvTickets.AllowUserToAddRows = false;
            dgvTickets.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            dgvTickets.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvTickets.Location = new Point(20, 70);
            dgvTickets.Name = "dgvTickets";
            dgvTickets.RowHeadersVisible = false;
            dgvTickets.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTickets.Size = new Size(380, 550);
            dgvTickets.TabIndex = 1;
            dgvTickets.SelectionChanged += dgvTickets_SelectionChanged;
            //
            // dgvDetails
            //
            dgvDetails.AllowUserToAddRows = false;
            dgvDetails.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvDetails.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDetails.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvDetails.Location = new Point(420, 70);
            dgvDetails.Name = "dgvDetails";
            dgvDetails.RowHeadersVisible = false;
            dgvDetails.Size = new Size(520, 550);
            dgvDetails.TabIndex = 2;
            //
            // frmPurchasesReport
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1280, 640);
            Controls.Add(dgvDetails);
            Controls.Add(dgvTickets);
            Controls.Add(groupBox1);
            Controls.Add(lblHeader);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "frmPurchasesReport";
            StartPosition = FormStartPosition.CenterScreen;
            WindowState = FormWindowState.Maximized;
            Text = "PurchasesReport";
            Load += frmPurchasesReport_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvTickets).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvDetails).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Label lblHeader;
        private GroupBox groupBox1;
        private Button btnFilter;
        private Button btnPrint;
        private Label label2;
        private DateTimePicker dtpEnd;
        private Label label1;
        private DateTimePicker dtpStart;
        private DataGridView dgvTickets;
        private DataGridView dgvDetails;
    }
}