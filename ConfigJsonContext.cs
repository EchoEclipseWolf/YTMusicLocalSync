using System.Text.Json.Serialization;

namespace YTMusicLocalSync;


    [JsonSerializable(typeof(Config))]
    public partial class ConfigJsonContext : JsonSerializerContext
    {
    }
