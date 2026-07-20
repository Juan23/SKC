namespace SKC_Branch
{
    partial class frmMain
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
            btnRefresh = new Button();
            btnAccept = new Button();
            dgvPending = new DataGridView();
            lblLines = new Label();
            dgvLines = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dgvPending).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvLines).BeginInit();
            SuspendLayout();
            //
            // lblHeader
            //
            lblHeader.AutoSize = true;
            lblHeader.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblHeader.ForeColor = Color.FromArgb(45, 52, 64);
            lblHeader.Location = new Point(20, 15);
            lblHeader.Name = "lblHeader";
            lblHeader.Size = new Size(220, 32);
            lblHeader.TabIndex = 0;
            lblHeader.Text = "Pending Deliveries";
            //
            // btnRefresh
            //
            btnRefresh.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnRefresh.BackColor = Color.White;
            btnRefresh.FlatAppearance.BorderColor = Color.Silver;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.Font = new Font("Segoe UI", 10F);
            btnRefresh.Location = new Point(1130, 15);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(130, 40);
            btnRefresh.TabIndex = 1;
            btnRefresh.Text = "Refresh";
            btnRefresh.UseVisualStyleBackColor = false;
            btnRefresh.Click += btnRefresh_Click;
            //
            // dgvPending
            //
            dgvPending.AllowUserToAddRows = false;
            dgvPending.AllowUserToDeleteRows = false;
            dgvPending.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dgvPending.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPending.BackgroundColor = Color.White;
            dgvPending.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPending.Location = new Point(20, 70);
            dgvPending.MultiSelect = false;
            dgvPending.Name = "dgvPending";
            dgvPending.ReadOnly = true;
            dgvPending.RowHeadersVisible = false;
            dgvPending.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPending.Size = new Size(1240, 220);
            dgvPending.TabIndex = 2;
            dgvPending.SelectionChanged += dgvPending_SelectionChanged;
            //
            // lblLines
            //
            lblLines.AutoSize = true;
            lblLines.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblLines.Location = new Point(20, 305);
            lblLines.Name = "lblLines";
            lblLines.Size = new Size(160, 23);
            lblLines.TabIndex = 3;
            lblLines.Text = "Items in this delivery";
            //
            // dgvLines
            //
            dgvLines.AllowUserToAddRows = false;
            dgvLines.AllowUserToDeleteRows = false;
            dgvLines.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvLines.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvLines.BackgroundColor = Color.White;
            dgvLines.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvLines.Location = new Point(20, 340);
            dgvLines.MultiSelect = false;
            dgvLines.Name = "dgvLines";
            dgvLines.ReadOnly = true;
            dgvLines.RowHeadersVisible = false;
            dgvLines.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLines.Size = new Size(1240, 215);
            dgvLines.TabIndex = 4;
            //
            // btnAccept
            //
            btnAccept.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnAccept.BackColor = Color.SeaGreen;
            btnAccept.FlatStyle = FlatStyle.Flat;
            btnAccept.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            btnAccept.ForeColor = Color.White;
            btnAccept.Location = new Point(20, 570);
            btnAccept.Name = "btnAccept";
            btnAccept.Size = new Size(1240, 55);
            btnAccept.TabIndex = 5;
            btnAccept.Text = "Accept Selected Delivery";
            btnAccept.UseVisualStyleBackColor = false;
            btnAccept.Click += btnAccept_Click;
            //
            // frmMain
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(1280, 640);
            Controls.Add(btnAccept);
            Controls.Add(dgvLines);
            Controls.Add(lblLines);
            Controls.Add(dgvPending);
            Controls.Add(btnRefresh);
            Controls.Add(lblHeader);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "frmMain";
            StartPosition = FormStartPosition.CenterScreen;
            WindowState = FormWindowState.Maximized;
            Text = "SKC Branch";
            Load += frmMain_Load;
            ((System.ComponentModel.ISupportInitialize)dgvPending).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvLines).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblHeader;
        private Button btnRefresh;
        private Button btnAccept;
        private DataGridView dgvPending;
        private Label lblLines;
        private DataGridView dgvLines;
    }
}
