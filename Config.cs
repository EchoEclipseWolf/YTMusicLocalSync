using System.Reflection;
using System.Text.Json;

namespace YTMusicLocalSync;


internal class Config {
    public string DirToPlaylists { get; set; } =
        Environment.GetFolderPath(Environment.SpecialFolder.MyMusic); // Default to user's My Music folder

    private static readonly string ConfigFileName = "config.ini";

    private static readonly string ExecutableDirectory =
        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;

    private static readonly string ConfigFilePath = Path.Combine(ExecutableDirectory, ConfigFileName);

    public void Load() {
        try {
            if (File.Exists(ConfigFilePath)) {
                string jsonString = File.ReadAllText(ConfigFilePath);
                Config? loadedConfig = JsonSerializer.Deserialize<Config>(jsonString);
                if (loadedConfig != null) {
                    DirToPlaylists = loadedConfig.DirToPlaylists;
                    Console.WriteLine($"Loaded configuration from {ConfigFileName}");
                }
            }
            else {
                Save(); // Create default config file
                Console.WriteLine($"Created default configuration at {ConfigFileName}");
            }
        }
        catch (Exception ex) {
            Console.WriteLine($"Error loading configuration: {ex.Message}");
        }
    }

    public void Save() {
        try {
            string jsonString = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(ConfigFilePath, jsonString);
        }
        catch (Exception ex) {
            Console.WriteLine($"Error saving configuration: {ex.Message}");
        }
    }
}
