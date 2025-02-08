using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace YTMusicLocalSync
{
    [JsonSerializable(typeof(Config))]
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
            var options = new JsonSerializerOptions()
            {
                TypeInfoResolver = ConfigJsonContext.Default,
                WriteIndented = true
            };

            if (File.Exists(configPath))
            {
                string json = File.ReadAllText(configPath);
                Config loadedConfig = JsonSerializer.Deserialize(json, ConfigJsonContext.Default.Config);
                DirToPlaylists = loadedConfig.DirToPlaylists;
            }
            else
            {
                string json = JsonSerializer.Serialize(this, ConfigJsonContext.Default.Config);
                File.WriteAllText(configPath, json);
            }
        }

    }

    [JsonSourceGenerationOptions(WriteIndented = true)]
    [JsonSerializable(typeof(Config))]
    internal partial class ConfigJsonContext : JsonSerializerContext { }
}