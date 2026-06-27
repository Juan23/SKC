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
            dgvTickets = new DataGridView();
            groupBox1 = new GroupBox();
            btnDelete = new Button();
            btnSearch = new Button();
            label2 = new Label();
            dtpEnd = new DateTimePicker();
            label1 = new Label();
            dtpStart = new DateTimePicker();
            ((System.ComponentModel.ISupportInitialize)dgvTickets).BeginInit();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // dgvTickets
            // 
            dgvTickets.AllowUserToAddRows = false;
            dgvTickets.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvTickets.Location = new Point(8, 8);
            dgvTickets.Name = "dgvTickets";
            dgvTickets.ReadOnly = true;
            dgvTickets.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTickets.Size = new Size(416, 664);
            dgvTickets.TabIndex = 4;
            dgvTickets.CellDoubleClick += dgvTickets_CellDoubleClick;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btnDelete);
            groupBox1.Controls.Add(btnSearch);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(dtpEnd);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(dtpStart);
            groupBox1.Location = new Point(432, 8);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(216, 536);
            groupBox1.TabIndex = 3;
            groupBox1.TabStop = false;
            groupBox1.Text = "Settings";
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(136, 504);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(75, 23);
            btnDelete.TabIndex = 5;
            btnDelete.Text = "Delete";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // btnSearch
            // 
            btnSearch.Location = new Point(136, 480);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(75, 23);
            btnSearch.TabIndex = 4;
            btnSearch.Text = "Search";
            btnSearch.UseVisualStyleBackColor = true;
            btnSearch.Click += btnSearch_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(8, 88);
            label2.Name = "label2";
            label2.Size = new Size(54, 15);
            label2.TabIndex = 3;
            label2.Text = "End Date";
            // 
            // dtpEnd
            // 
            dtpEnd.Location = new Point(8, 104);
            dtpEnd.Name = "dtpEnd";
            dtpEnd.Size = new Size(200, 23);
            dtpEnd.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(8, 32);
            label1.Name = "label1";
            label1.Size = new Size(58, 15);
            label1.TabIndex = 1;
            label1.Text = "Start Date";
            // 
            // dtpStart
            // 
            dtpStart.Location = new Point(8, 48);
            dtpStart.Name = "dtpStart";
            dtpStart.Size = new Size(200, 23);
            dtpStart.TabIndex = 0;
            // 
            // frmViewDeliveries
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(656, 681);
            Controls.Add(dgvTickets);
            Controls.Add(groupBox1);
            Name = "frmViewDeliveries";
            Text = "ViewDeliveries";
            Load += frmViewDeliveries_Load;
            ((System.ComponentModel.ISupportInitialize)dgvTickets).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private DataGridView dgvTickets;
        private GroupBox groupBox1;
        private Button btnDelete;
        private Button btnSearch;
        private Label label2;
        private DateTimePicker dtpEnd;
        private Label label1;
        private DateTimePicker dtpStart;
    }
}