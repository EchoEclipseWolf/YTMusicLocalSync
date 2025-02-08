using System.Text.Json;

namespace YTMusicLocalSync;

public class Config
{
    public string DirToPlaylists { get; set; } = string.Empty;
    private static readonly string ConfigFilePath = Path.Combine(AppContext.BaseDirectory, "config.ini");

    public void Load()
    {
        if (File.Exists(ConfigFilePath))
        {
            var json = File.ReadAllText(ConfigFilePath);
            var config = JsonSerializer.Deserialize<Config>(json, ConfigJsonContext.Default.Config);
            if (config is not null)
            {
                DirToPlaylists = config.DirToPlaylists;
            }
        }
        else
        {
            DirToPlaylists = "";
            Save();
        }
    }

    public void Save()
    {
        var json = JsonSerializer.Serialize(this, ConfigJsonContext.Default.Config);
        File.WriteAllText(ConfigFilePath, json);
    }
}