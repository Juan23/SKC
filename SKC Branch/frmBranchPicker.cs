namespace SKC_Branch
{
    public partial class frmBranchPicker : Form
    {
        // Must match the destination branch strings in the central app's Delivery form
        // (SKC Bakery Supplies\Delivery.cs) exactly - the server compares them case-sensitively.
        private static readonly string[] Branches = { "Yoho", "Gaisano", "Liloy", "Labason" };

        public string SelectedBranch => cmbBranch.SelectedItem?.ToString() ?? string.Empty;

        public frmBranchPicker()
        {
            InitializeComponent();
            cmbBranch.Items.AddRange(Branches);
        }

        private void cmbBranch_SelectedIndexChanged(object? sender, EventArgs e)
        {
            btnOk.Enabled = cmbBranch.SelectedIndex >= 0;
        }

        private void btnOk_Click(object? sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
