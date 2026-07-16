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

            // Idempotent CREATE IF NOT EXISTS; the POS queue must exist before any
            // screen opens so a sale can never race an uninitialized store.
            PosLocalStore.Initialize();

            // POS is the startup screen; deliveries/production/stock are reached from
            // its nav buttons and opened as ShowDialog, same as before the flip.
            Application.Run(new frmPos(config.BranchName));
        }
    }
}
