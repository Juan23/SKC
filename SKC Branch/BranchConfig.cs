using System;
using System.IO;
using System.Text.Json;

namespace SKC_Branch
{
    public class BranchConfig
    {
        public string BranchName { get; set; } = string.Empty;

        private static readonly string ConfigDir =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SKC Branch");
        private static readonly string ConfigPath = Path.Combine(ConfigDir, "config.json");

        public static BranchConfig? Load()
        {
            try
            {
                if (!File.Exists(ConfigPath)) return null;
                var config = JsonSerializer.Deserialize<BranchConfig>(File.ReadAllText(ConfigPath));
                if (config == null || string.IsNullOrWhiteSpace(config.BranchName)) return null;
                return config;
            }
            catch
            {
                // Corrupt config: treat as first run so the picker shows again.
                return null;
            }
        }

        public void Save()
        {
            Directory.CreateDirectory(ConfigDir);
            File.WriteAllText(ConfigPath, JsonSerializer.Serialize(this));
        }
    }
}
