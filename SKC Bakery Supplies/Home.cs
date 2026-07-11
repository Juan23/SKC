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
            btnSync.Text = "Syncing...";

            // Assuming this POS terminal is stationed at the "Yoho" branch. 
            // You can make this dynamic later via a settings file.
            string resultMessage = await NetworkSyncManager.SyncDeliveriesToServer("Yoho");

            MessageBox.Show(resultMessage, "Sync Status");

            btnSync.Text = "Sync to Central Server";
            btnSync.Enabled = true;
        }
    }
}
