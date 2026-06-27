namespace SKC_POS
{
    partial class frmUploadData
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
            btnUploadProductList = new Button();
            btnUploadSales = new Button();
            SuspendLayout();
            // 
            // btnUploadProductList
            // 
            btnUploadProductList.Location = new Point(16, 16);
            btnUploadProductList.Name = "btnUploadProductList";
            btnUploadProductList.Size = new Size(120, 23);
            btnUploadProductList.TabIndex = 0;
            btnUploadProductList.Text = "Upload Product List";
            btnUploadProductList.UseVisualStyleBackColor = true;
            btnUploadProductList.Click += btnUploadProductList_Click;
            // 
            // btnUploadSales
            // 
            btnUploadSales.Location = new Point(16, 48);
            btnUploadSales.Name = "btnUploadSales";
            btnUploadSales.Size = new Size(120, 23);
            btnUploadSales.TabIndex = 1;
            btnUploadSales.Text = "Upload Sales";
            btnUploadSales.UseVisualStyleBackColor = true;
            // 
            // frmUploadData
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1264, 681);
            Controls.Add(btnUploadSales);
            Controls.Add(btnUploadProductList);
            Name = "frmUploadData";
            Text = "Upload Data";
            Load += frmUploadData_Load;
            ResumeLayout(false);
        }

        #endregion

        private Button btnUploadProductList;
        private Button btnUploadSales;
    }
}