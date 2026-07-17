namespace SKC_Bakery_Supplies
{
    partial class frmViewDeliveries
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
            dgvTickets = new DataGridView();
            groupBox1 = new GroupBox();
            btnPrintDaily = new Button();
            btnDelete = new Button();
            btnEdit = new Button();
            btnSearch = new Button();
            label2 = new Label();
            dtpEnd = new DateTimePicker();
            label1 = new Label();
            dtpStart = new DateTimePicker();
            ((System.ComponentModel.ISupportInitialize)dgvTickets).BeginInit();
            groupBox1.SuspendLayout();
            SuspendLayout();
            //
            // lblHeader
            //
            lblHeader.AutoSize = true;
            lblHeader.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblHeader.Location = new Point(20, 15);
            lblHeader.Name = "lblHeader";
            lblHeader.Size = new Size(150, 32);
            lblHeader.TabIndex = 0;
            lblHeader.Text = "Deliveries";
            //
            // dgvTickets
            //
            dgvTickets.AllowUserToAddRows = false;
            dgvTickets.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvTickets.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvTickets.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvTickets.Location = new Point(20, 70);
            dgvTickets.Name = "dgvTickets";
            dgvTickets.ReadOnly = true;
            dgvTickets.RowHeadersVisible = false;
            dgvTickets.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTickets.Size = new Size(920, 550);
            dgvTickets.TabIndex = 1;
            dgvTickets.CellDoubleClick += dgvTickets_CellDoubleClick;
            //
            // groupBox1
            //
            groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            groupBox1.Controls.Add(btnPrintDaily);
            groupBox1.Controls.Add(btnDelete);
            groupBox1.Controls.Add(btnEdit);
            groupBox1.Controls.Add(btnSearch);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(dtpEnd);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(dtpStart);
            groupBox1.Font = new Font("Segoe UI", 10F);
            groupBox1.Location = new Point(960, 70);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(300, 340);
            groupBox1.TabIndex = 2;
            groupBox1.TabStop = false;
            groupBox1.Text = "Settings";
            //
            // btnPrintDaily
            //
            btnPrintDaily.Location = new Point(16, 290);
            btnPrintDaily.Name = "btnPrintDaily";
            btnPrintDaily.Size = new Size(260, 36);
            btnPrintDaily.TabIndex = 5;
            btnPrintDaily.Text = "Print Day Report";
            btnPrintDaily.UseVisualStyleBackColor = true;
            btnPrintDaily.Click += btnPrintDaily_Click;
            //
            // btnDelete
            //
            btnDelete.Location = new Point(16, 245);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(260, 36);
            btnDelete.TabIndex = 4;
            btnDelete.Text = "Delete";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            //
            // btnSearch
            //
            btnSearch.Location = new Point(16, 155);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(260, 36);
            btnSearch.TabIndex = 2;
            btnSearch.Text = "Search";
            btnSearch.UseVisualStyleBackColor = true;
            btnSearch.Click += btnSearch_Click;
            //
            // btnEdit
            //
            btnEdit.Enabled = false;
            btnEdit.Location = new Point(16, 200);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(260, 36);
            btnEdit.TabIndex = 3;
            btnEdit.Text = "Edit Ticket";
            btnEdit.UseVisualStyleBackColor = true;
            btnEdit.Click += btnEdit_Click;
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
            dtpEnd.Value = new DateTime(2026, 7, 7, 18, 8, 23, 0);
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
            // frmViewDeliveries
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1280, 640);
            Controls.Add(dgvTickets);
            Controls.Add(groupBox1);
            Controls.Add(lblHeader);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "frmViewDeliveries";
            StartPosition = FormStartPosition.CenterScreen;
            WindowState = FormWindowState.Maximized;
            Text = "ViewDeliveries";
            Load += frmViewDeliveries_Load;
            ((System.ComponentModel.ISupportInitialize)dgvTickets).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private Label lblHeader;
        private DataGridView dgvTickets;
        private GroupBox groupBox1;
        private Button btnDelete;
        private Button btnEdit;
        private Button btnSearch;
        private Label label2;
        private DateTimePicker dtpEnd;
        private Label label1;
        private DateTimePicker dtpStart;
        private Button btnPrintDaily;
    }
}