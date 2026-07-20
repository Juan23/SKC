using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SKC_Bakery_Supplies
{
    public partial class frmHome : Form
    {
        public frmHome()
        {
            InitializeComponent();

            var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            Text = $"SKC Bakery Supplies  (v{version?.ToString(3)})";
        }

        private void btnPos_Click(object? sender, EventArgs e)
        {
            using var posScreen = new frmPos();
            posScreen.ShowDialog();
        }

        private void btnBranchSales_Click(object? sender, EventArgs e)
        {
            frmBranchSalesReport salesScreen = new frmBranchSalesReport();
            salesScreen.ShowDialog();
        }

        private void btnBranchInventoryReport_Click(object sender, EventArgs e)
        {
            frmBranchInventoryReport reportScreen = new frmBranchInventoryReport();
            reportScreen.ShowDialog();
        }

        private void btnAddNewPurchase_Click(object sender, EventArgs e)
        {
            frmPurchases purchaseScreen = new frmPurchases();
            purchaseScreen.ShowDialog();
        }

        private void btnPurchasesReport_Click(object sender, EventArgs e)
        {
            frmPurchasesReport reportsScreen = new frmPurchasesReport();
            reportsScreen.ShowDialog();
        }

        private void btnViewInventory_Click(object sender, EventArgs e)
        {
            frmViewProducts productsScreen = new frmViewProducts();
            productsScreen.ShowDialog();
        }

        private void btnAddInventoryItem_Click(object sender, EventArgs e)
        {
            using (var addForm = new frmAddMasterItem(""))
            {
                addForm.ShowDialog();
            }
        }

        private void btnAdjustmentHistory_Click(object sender, EventArgs e)
        {
            frmAdjustmentHistory historyScreen = new frmAdjustmentHistory();
            historyScreen.ShowDialog();
        }

        private void btnCreateDelivery_Click(object sender, EventArgs e)
        {
            frmDelivery deliveryScreen = new frmDelivery();
            deliveryScreen.ShowDialog();
        }

        private void btnViewDeliveries_Click(object sender, EventArgs e)
        {
            frmViewDeliveries viewDeliveriesScreen = new frmViewDeliveries();
            viewDeliveriesScreen.ShowDialog();
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private async void btnSync_Click(object sender, EventArgs e)
        {
            btnSync.Enabled = false;
            btnSync.Text = "Checking...";

            try
            {
                await CentralApiClient.CheckHealthAsync();
                MessageBox.Show("Connected to central server.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Cannot reach central server.\n{ex.Message}", "Connection Error");
            }
            finally
            {
                btnSync.Text = "Sync to Central Server";
                btnSync.Enabled = true;
            }
        }
    }
}
