using System;
using System.IO;
using System.Text.Json;

namespace YTMusicLocalSync
{
    public class Config
    {
        public string DirToPlaylists { get; set; }
        

        public Config()
        {
            DirToPlaylists = "";
        }

        public void Load()
        {
            string configPath = "config.ini";
            if (File.Exists(configPath))
            {
                string json = File.ReadAllText(configPath);
                var options = new JsonSerializerOptions()
                {
                    WriteIndented = true
                };
                Config loadedConfig = JsonSerializer.Deserialize<Config>(json,options);
                DirToPlaylists = loadedConfig.DirToPlaylists;
            }
            else
            {
                //Create File
                var options = new JsonSerializerOptions()
                {
                    WriteIndented = true
                };
                string json = JsonSerializer.Serialize<Config>(this,options);
                File.WriteAllText(configPath,json);
            }
        }
    }
}
