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
            lblHeader.Location = new Point(20, 15);
            lblHeader.Name = "lblHeader";
            lblHeader.Size = new Size(220, 32);
            lblHeader.TabIndex = 0;
            lblHeader.Text = "Pending Deliveries";
            //
            // btnRefresh
            //
            btnRefresh.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnRefresh.Location = new Point(674, 15);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(110, 35);
            btnRefresh.TabIndex = 1;
            btnRefresh.Text = "Refresh";
            btnRefresh.UseVisualStyleBackColor = true;
            btnRefresh.Click += btnRefresh_Click;
            //
            // dgvPending
            //
            dgvPending.AllowUserToAddRows = false;
            dgvPending.AllowUserToDeleteRows = false;
            dgvPending.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dgvPending.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPending.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPending.Location = new Point(20, 60);
            dgvPending.MultiSelect = false;
            dgvPending.Name = "dgvPending";
            dgvPending.ReadOnly = true;
            dgvPending.RowHeadersVisible = false;
            dgvPending.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPending.Size = new Size(764, 220);
            dgvPending.TabIndex = 2;
            dgvPending.SelectionChanged += dgvPending_SelectionChanged;
            //
            // lblLines
            //
            lblLines.AutoSize = true;
            lblLines.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblLines.Location = new Point(20, 292);
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
            dgvLines.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvLines.Location = new Point(20, 320);
            dgvLines.MultiSelect = false;
            dgvLines.Name = "dgvLines";
            dgvLines.ReadOnly = true;
            dgvLines.RowHeadersVisible = false;
            dgvLines.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLines.Size = new Size(764, 230);
            dgvLines.TabIndex = 4;
            //
            // btnAccept
            //
            btnAccept.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnAccept.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnAccept.Location = new Point(20, 565);
            btnAccept.Name = "btnAccept";
            btnAccept.Size = new Size(764, 50);
            btnAccept.TabIndex = 5;
            btnAccept.Text = "Accept Selected Delivery";
            btnAccept.UseVisualStyleBackColor = true;
            btnAccept.Click += btnAccept_Click;
            //
            // frmMain
            //
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(804, 631);
            Controls.Add(btnAccept);
            Controls.Add(dgvLines);
            Controls.Add(lblLines);
            Controls.Add(dgvPending);
            Controls.Add(btnRefresh);
            Controls.Add(lblHeader);
            MinimumSize = new Size(700, 550);
            Name = "frmMain";
            StartPosition = FormStartPosition.CenterScreen;
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
