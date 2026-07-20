namespace SKC_Admin
{
    partial class frmMain
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
            tabMain = new System.Windows.Forms.TabControl();
            tabProducts = new System.Windows.Forms.TabPage();
            dgvProducts = new System.Windows.Forms.DataGridView();
            pnlProductActions = new System.Windows.Forms.Panel();
            btnSetPrice = new System.Windows.Forms.Button();
            btnClassify = new System.Windows.Forms.Button();
            btnRefreshProducts = new System.Windows.Forms.Button();
            tabRecipes = new System.Windows.Forms.TabPage();
            dgvRecipes = new System.Windows.Forms.DataGridView();
            pnlRecipeActions = new System.Windows.Forms.Panel();
            btnDeactivateRecipe = new System.Windows.Forms.Button();
            btnEditRecipe = new System.Windows.Forms.Button();
            btnNewRecipe = new System.Windows.Forms.Button();
            btnRefreshRecipes = new System.Windows.Forms.Button();
            tabMain.SuspendLayout();
            tabProducts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvProducts).BeginInit();
            pnlProductActions.SuspendLayout();
            tabRecipes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvRecipes).BeginInit();
            pnlRecipeActions.SuspendLayout();
            SuspendLayout();
            //
            // tabMain
            //
            tabMain.Controls.Add(tabProducts);
            tabMain.Controls.Add(tabRecipes);
            tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
            tabMain.Location = new System.Drawing.Point(0, 0);
            tabMain.Name = "tabMain";
            tabMain.SelectedIndex = 0;
            tabMain.Size = new System.Drawing.Size(900, 600);
            tabMain.TabIndex = 0;
            //
            // tabProducts
            //
            tabProducts.Controls.Add(dgvProducts);
            tabProducts.Controls.Add(pnlProductActions);
            tabProducts.Location = new System.Drawing.Point(4, 24);
            tabProducts.Name = "tabProducts";
            tabProducts.Padding = new System.Windows.Forms.Padding(8);
            tabProducts.Size = new System.Drawing.Size(892, 572);
            tabProducts.TabIndex = 0;
            tabProducts.Text = "Products / Categories";
            tabProducts.UseVisualStyleBackColor = true;
            //
            // dgvProducts
            //
            dgvProducts.AllowUserToAddRows = false;
            dgvProducts.AllowUserToDeleteRows = false;
            dgvProducts.Dock = System.Windows.Forms.DockStyle.Fill;
            dgvProducts.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dgvProducts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvProducts.Location = new System.Drawing.Point(8, 48);
            dgvProducts.Name = "dgvProducts";
            dgvProducts.ReadOnly = true;
            dgvProducts.RowHeadersVisible = false;
            dgvProducts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvProducts.Size = new System.Drawing.Size(876, 516);
            dgvProducts.TabIndex = 1;
            //
            // pnlProductActions
            //
            pnlProductActions.Controls.Add(btnSetPrice);
            pnlProductActions.Controls.Add(btnClassify);
            pnlProductActions.Controls.Add(btnRefreshProducts);
            pnlProductActions.Dock = System.Windows.Forms.DockStyle.Top;
            pnlProductActions.Location = new System.Drawing.Point(8, 8);
            pnlProductActions.Name = "pnlProductActions";
            pnlProductActions.Size = new System.Drawing.Size(876, 40);
            pnlProductActions.TabIndex = 0;
            //
            // btnClassify
            //
            btnClassify.BackColor = System.Drawing.Color.White;
            btnClassify.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            btnClassify.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnClassify.Location = new System.Drawing.Point(130, 6);
            btnClassify.Name = "btnClassify";
            btnClassify.Size = new System.Drawing.Size(160, 28);
            btnClassify.TabIndex = 1;
            btnClassify.Text = "Set Category / UoM...";
            btnClassify.UseVisualStyleBackColor = false;
            btnClassify.Click += btnClassify_Click;
            //
            // btnSetPrice
            //
            btnSetPrice.BackColor = System.Drawing.Color.White;
            btnSetPrice.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            btnSetPrice.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnSetPrice.Location = new System.Drawing.Point(306, 6);
            btnSetPrice.Name = "btnSetPrice";
            btnSetPrice.Size = new System.Drawing.Size(120, 28);
            btnSetPrice.TabIndex = 2;
            btnSetPrice.Text = "Set Price...";
            btnSetPrice.UseVisualStyleBackColor = false;
            btnSetPrice.Click += btnSetPrice_Click;
            //
            // btnRefreshProducts
            //
            btnRefreshProducts.BackColor = System.Drawing.Color.White;
            btnRefreshProducts.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            btnRefreshProducts.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnRefreshProducts.Location = new System.Drawing.Point(3, 6);
            btnRefreshProducts.Name = "btnRefreshProducts";
            btnRefreshProducts.Size = new System.Drawing.Size(110, 28);
            btnRefreshProducts.TabIndex = 0;
            btnRefreshProducts.Text = "Refresh";
            btnRefreshProducts.UseVisualStyleBackColor = false;
            btnRefreshProducts.Click += btnRefreshProducts_Click;
            //
            // tabRecipes
            //
            tabRecipes.Controls.Add(dgvRecipes);
            tabRecipes.Controls.Add(pnlRecipeActions);
            tabRecipes.Location = new System.Drawing.Point(4, 24);
            tabRecipes.Name = "tabRecipes";
            tabRecipes.Padding = new System.Windows.Forms.Padding(8);
            tabRecipes.Size = new System.Drawing.Size(892, 572);
            tabRecipes.TabIndex = 1;
            tabRecipes.Text = "Recipes";
            tabRecipes.UseVisualStyleBackColor = true;
            //
            // dgvRecipes
            //
            dgvRecipes.AllowUserToAddRows = false;
            dgvRecipes.AllowUserToDeleteRows = false;
            dgvRecipes.Dock = System.Windows.Forms.DockStyle.Fill;
            dgvRecipes.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dgvRecipes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvRecipes.Location = new System.Drawing.Point(8, 48);
            dgvRecipes.Name = "dgvRecipes";
            dgvRecipes.ReadOnly = true;
            dgvRecipes.RowHeadersVisible = false;
            dgvRecipes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvRecipes.Size = new System.Drawing.Size(876, 516);
            dgvRecipes.TabIndex = 1;
            //
            // pnlRecipeActions
            //
            pnlRecipeActions.Controls.Add(btnDeactivateRecipe);
            pnlRecipeActions.Controls.Add(btnEditRecipe);
            pnlRecipeActions.Controls.Add(btnNewRecipe);
            pnlRecipeActions.Controls.Add(btnRefreshRecipes);
            pnlRecipeActions.Dock = System.Windows.Forms.DockStyle.Top;
            pnlRecipeActions.Location = new System.Drawing.Point(8, 8);
            pnlRecipeActions.Name = "pnlRecipeActions";
            pnlRecipeActions.Size = new System.Drawing.Size(876, 40);
            pnlRecipeActions.TabIndex = 0;
            //
            // btnDeactivateRecipe
            //
            btnDeactivateRecipe.BackColor = System.Drawing.Color.Firebrick;
            btnDeactivateRecipe.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnDeactivateRecipe.ForeColor = System.Drawing.Color.White;
            btnDeactivateRecipe.Location = new System.Drawing.Point(360, 6);
            btnDeactivateRecipe.Name = "btnDeactivateRecipe";
            btnDeactivateRecipe.Size = new System.Drawing.Size(110, 28);
            btnDeactivateRecipe.TabIndex = 3;
            btnDeactivateRecipe.Text = "Deactivate";
            btnDeactivateRecipe.UseVisualStyleBackColor = false;
            btnDeactivateRecipe.Click += btnDeactivateRecipe_Click;
            //
            // btnEditRecipe
            //
            btnEditRecipe.BackColor = System.Drawing.Color.White;
            btnEditRecipe.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            btnEditRecipe.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnEditRecipe.Location = new System.Drawing.Point(244, 6);
            btnEditRecipe.Name = "btnEditRecipe";
            btnEditRecipe.Size = new System.Drawing.Size(110, 28);
            btnEditRecipe.TabIndex = 2;
            btnEditRecipe.Text = "Edit...";
            btnEditRecipe.UseVisualStyleBackColor = false;
            btnEditRecipe.Click += btnEditRecipe_Click;
            //
            // btnNewRecipe
            //
            btnNewRecipe.BackColor = System.Drawing.Color.White;
            btnNewRecipe.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            btnNewRecipe.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnNewRecipe.Location = new System.Drawing.Point(128, 6);
            btnNewRecipe.Name = "btnNewRecipe";
            btnNewRecipe.Size = new System.Drawing.Size(110, 28);
            btnNewRecipe.TabIndex = 1;
            btnNewRecipe.Text = "New Recipe...";
            btnNewRecipe.UseVisualStyleBackColor = false;
            btnNewRecipe.Click += btnNewRecipe_Click;
            //
            // btnRefreshRecipes
            //
            btnRefreshRecipes.BackColor = System.Drawing.Color.White;
            btnRefreshRecipes.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            btnRefreshRecipes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnRefreshRecipes.Location = new System.Drawing.Point(3, 6);
            btnRefreshRecipes.Name = "btnRefreshRecipes";
            btnRefreshRecipes.Size = new System.Drawing.Size(110, 28);
            btnRefreshRecipes.TabIndex = 0;
            btnRefreshRecipes.Text = "Refresh";
            btnRefreshRecipes.UseVisualStyleBackColor = false;
            btnRefreshRecipes.Click += btnRefreshRecipes_Click;
            //
            // frmMain
            //
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(900, 600);
            Controls.Add(tabMain);
            MinimumSize = new System.Drawing.Size(700, 450);
            Name = "frmMain";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "SKC Admin";
            Load += frmMain_Load;
            tabMain.ResumeLayout(false);
            tabProducts.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvProducts).EndInit();
            pnlProductActions.ResumeLayout(false);
            tabRecipes.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvRecipes).EndInit();
            pnlRecipeActions.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage tabProducts;
        private System.Windows.Forms.DataGridView dgvProducts;
        private System.Windows.Forms.Panel pnlProductActions;
        private System.Windows.Forms.Button btnSetPrice;
        private System.Windows.Forms.Button btnClassify;
        private System.Windows.Forms.Button btnRefreshProducts;
        private System.Windows.Forms.TabPage tabRecipes;
        private System.Windows.Forms.DataGridView dgvRecipes;
        private System.Windows.Forms.Panel pnlRecipeActions;
        private System.Windows.Forms.Button btnDeactivateRecipe;
        private System.Windows.Forms.Button btnEditRecipe;
        private System.Windows.Forms.Button btnNewRecipe;
        private System.Windows.Forms.Button btnRefreshRecipes;
    }
}
