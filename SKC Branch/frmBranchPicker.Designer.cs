namespace SKC_Branch
{
    partial class frmBranchPicker
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
            lblPrompt = new Label();
            cmbBranch = new ComboBox();
            btnOk = new Button();
            SuspendLayout();
            //
            // lblPrompt
            //
            lblPrompt.AutoSize = true;
            lblPrompt.Location = new Point(20, 20);
            lblPrompt.Name = "lblPrompt";
            lblPrompt.Size = new Size(220, 20);
            lblPrompt.TabIndex = 0;
            lblPrompt.Text = "Which branch is this computer at?";
            //
            // cmbBranch
            //
            cmbBranch.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbBranch.FormattingEnabled = true;
            cmbBranch.Location = new Point(20, 50);
            cmbBranch.Name = "cmbBranch";
            cmbBranch.Size = new Size(280, 28);
            cmbBranch.TabIndex = 1;
            cmbBranch.SelectedIndexChanged += cmbBranch_SelectedIndexChanged;
            //
            // btnOk
            //
            btnOk.Enabled = false;
            btnOk.Location = new Point(20, 95);
            btnOk.Name = "btnOk";
            btnOk.Size = new Size(280, 35);
            btnOk.TabIndex = 2;
            btnOk.Text = "OK";
            btnOk.UseVisualStyleBackColor = true;
            btnOk.Click += btnOk_Click;
            //
            // frmBranchPicker
            //
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(320, 150);
            Controls.Add(btnOk);
            Controls.Add(cmbBranch);
            Controls.Add(lblPrompt);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmBranchPicker";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "SKC Branch - Setup";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblPrompt;
        private ComboBox cmbBranch;
        private Button btnOk;
    }
}
