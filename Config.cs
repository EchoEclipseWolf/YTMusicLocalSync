using System.Text.Json;

namespace YTMusicLocalSync
{
    public class Config
    {
        public string DirToPlaylists { get; set; } = string.Empty;

        public void Load()
        {
            string configPath = "config.ini";
            if (!File.Exists(configPath))
            {
                // Create a default config file
                var newConfig = new Config();
                newConfig.Save();
                return;
            }
            var json = File.ReadAllText(configPath);
            var loadedConfig = JsonSerializer.Deserialize<Config>(json);
            //Apply loaded config into class
            DirToPlaylists = loadedConfig.DirToPlaylists;
        }

        public void Save()
        {
            string configPath = "config.ini";
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(this, options);
            File.WriteAllText(configPath, json.ToString());
        }
    }
}
