namespace YTMusicLocalSync;

using System;
using System.IO;
using Newtonsoft.Json;

public class Config
{
    public string DirToPlaylists { get; set; }

    public Config()
    {
        // Default value
        DirToPlaylists = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
    }

    public static Config Load()
    {
        string configFilePath = "config.ini";
        if (File.Exists(configFilePath))
        {
            try
            {
                string json = File.ReadAllText(configFilePath);
                return JsonConvert.DeserializeObject<Config>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading config file: {ex.Message}. Loading default config.");
                return CreateDefaultConfigAndSave(configFilePath);
            }
        }
        else
        {
            Console.WriteLine("Config file not found. Creating default config.");
            return CreateDefaultConfigAndSave(configFilePath);
        }
    }

    private static Config CreateDefaultConfigAndSave(string configFilePath)
    {
        Config config = new Config();
        Save(config, configFilePath);
        return config;
    }

    public static void Save(Config config, string configFilePath)
    {
        try
        {
            string json = JsonConvert.SerializeObject(config, Formatting.Indented);
            File.WriteAllText(configFilePath, json);
            Console.WriteLine($"Config saved to {configFilePath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving config file: {ex.Message}");
        }
    }
}