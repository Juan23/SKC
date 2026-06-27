namespace SKC_POS
{
    partial class frmUploadSales
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
            grpBranch = new GroupBox();
            rdoIpil = new RadioButton();
            btnUploadSales = new Button();
            rdoGaisano = new RadioButton();
            rdoLiloy = new RadioButton();
            rdoLabason = new RadioButton();
            dtpSaleDate = new DateTimePicker();
            grpBranch.SuspendLayout();
            SuspendLayout();
            // 
            // grpBranch
            // 
            grpBranch.Controls.Add(rdoLabason);
            grpBranch.Controls.Add(rdoLiloy);
            grpBranch.Controls.Add(rdoGaisano);
            grpBranch.Controls.Add(rdoIpil);
            grpBranch.Location = new Point(16, 24);
            grpBranch.Name = "grpBranch";
            grpBranch.Size = new Size(112, 128);
            grpBranch.TabIndex = 0;
            grpBranch.TabStop = false;
            grpBranch.Text = "Select Branch";
            // 
            // rdoIpil
            // 
            rdoIpil.AutoSize = true;
            rdoIpil.Location = new Point(8, 24);
            rdoIpil.Name = "rdoIpil";
            rdoIpil.Size = new Size(41, 19);
            rdoIpil.TabIndex = 0;
            rdoIpil.TabStop = true;
            rdoIpil.Text = "Ipil";
            rdoIpil.UseVisualStyleBackColor = true;
            // 
            // btnUploadSales
            // 
            btnUploadSales.Location = new Point(384, 128);
            btnUploadSales.Name = "btnUploadSales";
            btnUploadSales.Size = new Size(75, 23);
            btnUploadSales.TabIndex = 1;
            btnUploadSales.Text = "Upload Sales";
            btnUploadSales.UseVisualStyleBackColor = true;
            btnUploadSales.Click += btnUploadSales_Click;
            // 
            // rdoGaisano
            // 
            rdoGaisano.AutoSize = true;
            rdoGaisano.Location = new Point(8, 48);
            rdoGaisano.Name = "rdoGaisano";
            rdoGaisano.Size = new Size(67, 19);
            rdoGaisano.TabIndex = 1;
            rdoGaisano.TabStop = true;
            rdoGaisano.Text = "Gaisano";
            rdoGaisano.UseVisualStyleBackColor = true;
            // 
            // rdoLiloy
            // 
            rdoLiloy.AutoSize = true;
            rdoLiloy.Location = new Point(8, 72);
            rdoLiloy.Name = "rdoLiloy";
            rdoLiloy.Size = new Size(50, 19);
            rdoLiloy.TabIndex = 2;
            rdoLiloy.TabStop = true;
            rdoLiloy.Text = "Liloy";
            rdoLiloy.UseVisualStyleBackColor = true;
            // 
            // rdoLabason
            // 
            rdoLabason.AutoSize = true;
            rdoLabason.Location = new Point(8, 96);
            rdoLabason.Name = "rdoLabason";
            rdoLabason.Size = new Size(69, 19);
            rdoLabason.TabIndex = 3;
            rdoLabason.TabStop = true;
            rdoLabason.Text = "Labason";
            rdoLabason.UseVisualStyleBackColor = true;
            // 
            // dtpSaleDate
            // 
            dtpSaleDate.Format = DateTimePickerFormat.Short;
            dtpSaleDate.Location = new Point(144, 32);
            dtpSaleDate.Name = "dtpSaleDate";
            dtpSaleDate.Size = new Size(200, 23);
            dtpSaleDate.TabIndex = 2;
            // 
            // frmUploadSales
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(468, 164);
            Controls.Add(dtpSaleDate);
            Controls.Add(btnUploadSales);
            Controls.Add(grpBranch);
            Name = "frmUploadSales";
            Text = "Upload Sales";
            grpBranch.ResumeLayout(false);
            grpBranch.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox grpBranch;
        private RadioButton rdoIpil;
        private Button btnUploadSales;
        private RadioButton rdoLabason;
        private RadioButton rdoLiloy;
        private RadioButton rdoGaisano;
        private DateTimePicker dtpSaleDate;
    }
}