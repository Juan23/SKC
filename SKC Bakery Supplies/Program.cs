namespace SKC_Bakery_Supplies
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            // Idempotent CREATE IF NOT EXISTS; the POS queue must exist before the POS
            // screen can open so a sale can never race an uninitialized store.
            PosLocalStore.Initialize();

            Application.Run(new frmHome());
        }
    }
}