namespace SKC_Branch
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            var config = BranchConfig.Load();
            if (config == null)
            {
                using var picker = new frmBranchPicker();
                if (picker.ShowDialog() != DialogResult.OK) return;

                config = new BranchConfig { BranchName = picker.SelectedBranch };
                config.Save();
            }

            Application.Run(new frmMain(config.BranchName));
        }
    }
}
