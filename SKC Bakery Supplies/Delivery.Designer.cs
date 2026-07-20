namespace SKC_Bakery_Supplies
{
    partial class frmDelivery
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
            label1 = new Label();
            cmbBranch = new ComboBox();
            txtProductSearch = new TextBox();
            label2 = new Label();
            numQty = new NumericUpDown();
            label3 = new Label();
            btnAddItem = new Button();
            lstSearch = new ListBox();
            dgvDeliveryItems = new DataGridView();
            btnSubmitDelivery = new Button();
            cmbRequester = new ComboBox();
            label4 = new Label();
            txtReason = new TextBox();
            label5 = new Label();
            grpDetails = new GroupBox();
            ((System.ComponentModel.ISupportInitialize)numQty).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvDeliveryItems).BeginInit();
            grpDetails.SuspendLayout();
            SuspendLayout();
            //
            // lblHeader
            //
            lblHeader.AutoSize = true;
            lblHeader.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblHeader.ForeColor = Color.FromArgb(45, 52, 64);
            lblHeader.Location = new Point(20, 15);
            lblHeader.Name = "lblHeader";
            lblHeader.Size = new Size(160, 32);
            lblHeader.TabIndex = 0;
            lblHeader.Text = "New Delivery";
            //
            // label1
            //
            label1.AutoSize = true;
            label1.Location = new Point(16, 28);
            label1.Name = "label1";
            label1.Size = new Size(130, 20);
            label1.TabIndex = 0;
            label1.Text = "Destination Branch";
            //
            // cmbBranch
            //
            cmbBranch.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbBranch.FormattingEnabled = true;
            cmbBranch.Location = new Point(16, 48);
            cmbBranch.Name = "cmbBranch";
            cmbBranch.Size = new Size(140, 28);
            cmbBranch.TabIndex = 1;
            //
            // label4
            //
            label4.AutoSize = true;
            label4.Location = new Point(172, 28);
            label4.Name = "label4";
            label4.Size = new Size(72, 20);
            label4.TabIndex = 2;
            label4.Text = "Requester";
            //
            // cmbRequester
            //
            cmbRequester.FormattingEnabled = true;
            cmbRequester.Location = new Point(172, 48);
            cmbRequester.Name = "cmbRequester";
            cmbRequester.Size = new Size(172, 28);
            cmbRequester.TabIndex = 3;
            //
            // label5
            //
            label5.AutoSize = true;
            label5.Location = new Point(16, 88);
            label5.Name = "label5";
            label5.Size = new Size(54, 20);
            label5.TabIndex = 4;
            label5.Text = "Reason";
            //
            // txtReason
            //
            txtReason.Location = new Point(16, 108);
            txtReason.Name = "txtReason";
            txtReason.Size = new Size(328, 28);
            txtReason.TabIndex = 5;
            //
            // label2
            //
            label2.AutoSize = true;
            label2.Location = new Point(16, 150);
            label2.Name = "label2";
            label2.Size = new Size(100, 20);
            label2.TabIndex = 6;
            label2.Text = "Search Product";
            //
            // txtProductSearch
            //
            txtProductSearch.Location = new Point(16, 170);
            txtProductSearch.Name = "txtProductSearch";
            txtProductSearch.Size = new Size(328, 28);
            txtProductSearch.TabIndex = 7;
            txtProductSearch.TextChanged += txtProductSearch_TextChanged;
            txtProductSearch.KeyDown += txtProductSearch_KeyDown;
            //
            // lstSearch
            //
            lstSearch.FormattingEnabled = true;
            lstSearch.Location = new Point(16, 200);
            lstSearch.Name = "lstSearch";
            lstSearch.Size = new Size(328, 144);
            lstSearch.TabIndex = 8;
            lstSearch.Visible = false;
            lstSearch.Click += lstSearch_Click;
            //
            // label3
            //
            label3.AutoSize = true;
            label3.Location = new Point(16, 212);
            label3.Name = "label3";
            label3.Size = new Size(64, 20);
            label3.TabIndex = 9;
            label3.Text = "Quantity";
            //
            // numQty
            //
            numQty.Location = new Point(16, 232);
            numQty.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numQty.Name = "numQty";
            numQty.Size = new Size(100, 28);
            numQty.TabIndex = 10;
            numQty.Value = new decimal(new int[] { 1, 0, 0, 0 });
            numQty.Click += HighlightNumericText;
            numQty.Enter += HighlightNumericText;
            //
            // btnAddItem
            //
            btnAddItem.BackColor = Color.SeaGreen;
            btnAddItem.FlatStyle = FlatStyle.Flat;
            btnAddItem.ForeColor = Color.White;
            btnAddItem.Location = new Point(16, 280);
            btnAddItem.Name = "btnAddItem";
            btnAddItem.Size = new Size(328, 40);
            btnAddItem.TabIndex = 11;
            btnAddItem.Text = "Add Item";
            btnAddItem.UseVisualStyleBackColor = false;
            btnAddItem.Click += btnAddItem_Click;
            //
            // grpDetails
            //
            grpDetails.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            grpDetails.Controls.Add(label1);
            grpDetails.Controls.Add(cmbBranch);
            grpDetails.Controls.Add(label4);
            grpDetails.Controls.Add(cmbRequester);
            grpDetails.Controls.Add(label5);
            grpDetails.Controls.Add(txtReason);
            grpDetails.Controls.Add(label2);
            grpDetails.Controls.Add(txtProductSearch);
            grpDetails.Controls.Add(label3);
            grpDetails.Controls.Add(numQty);
            grpDetails.Controls.Add(btnAddItem);
            grpDetails.Controls.Add(lstSearch);
            grpDetails.Font = new Font("Segoe UI", 10F);
            grpDetails.Location = new Point(900, 70);
            grpDetails.Name = "grpDetails";
            grpDetails.Size = new Size(360, 550);
            grpDetails.TabIndex = 2;
            grpDetails.TabStop = false;
            grpDetails.Text = "Delivery Details";
            //
            // dgvDeliveryItems
            //
            dgvDeliveryItems.AllowUserToAddRows = false;
            dgvDeliveryItems.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvDeliveryItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDeliveryItems.BackgroundColor = Color.White;
            dgvDeliveryItems.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvDeliveryItems.Location = new Point(20, 70);
            dgvDeliveryItems.Name = "dgvDeliveryItems";
            dgvDeliveryItems.ReadOnly = true;
            dgvDeliveryItems.RowHeadersVisible = false;
            dgvDeliveryItems.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDeliveryItems.Size = new Size(860, 510);
            dgvDeliveryItems.TabIndex = 1;
            //
            // btnSubmitDelivery
            //
            btnSubmitDelivery.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnSubmitDelivery.BackColor = Color.SeaGreen;
            btnSubmitDelivery.FlatStyle = FlatStyle.Flat;
            btnSubmitDelivery.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnSubmitDelivery.ForeColor = Color.White;
            btnSubmitDelivery.Location = new Point(720, 592);
            btnSubmitDelivery.Name = "btnSubmitDelivery";
            btnSubmitDelivery.Size = new Size(160, 40);
            btnSubmitDelivery.TabIndex = 3;
            btnSubmitDelivery.Text = "Submit Delivery";
            btnSubmitDelivery.UseVisualStyleBackColor = false;
            btnSubmitDelivery.Click += btnSubmitDelivery_Click;
            //
            // frmDelivery
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(1280, 640);
            Controls.Add(lblHeader);
            Controls.Add(btnSubmitDelivery);
            Controls.Add(dgvDeliveryItems);
            Controls.Add(grpDetails);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "frmDelivery";
            StartPosition = FormStartPosition.CenterScreen;
            WindowState = FormWindowState.Maximized;
            Text = "Delivery";
            Load += frmDelivery_Load;
            ((System.ComponentModel.ISupportInitialize)numQty).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvDeliveryItems).EndInit();
            grpDetails.ResumeLayout(false);
            grpDetails.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblHeader;
        private Label label1;
        private ComboBox cmbBranch;
        private TextBox txtProductSearch;
        private Label label2;
        private NumericUpDown numQty;
        private Label label3;
        private Button btnAddItem;
        private ListBox lstSearch;
        private DataGridView dgvDeliveryItems;
        private Button btnSubmitDelivery;
        private ComboBox cmbRequester;
        private Label label4;
        private TextBox txtReason;
        private Label label5;
        private GroupBox grpDetails;
    }
}
