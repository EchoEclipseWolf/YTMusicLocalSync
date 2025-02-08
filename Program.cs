using System;
using System;
using System.Threading.Tasks;

namespace YTMusicLocalSync {
    internal class Program {
        public static Config Configuration { get; private set; }

        static async Task Main(string[] args) {
            Configuration = Config.Load();

            Console.WriteLine($"DirToPlaylists from config: {Configuration.DirToPlaylists}");

            API api = new API();
            await api.Start(async (youtubeService) => {
                if (youtubeService != null) {
                    Console.WriteLine("Successfully connected to YouTube API.");
                    var playlists = await api.GetUserPlaylistsAsync(youtubeService);
                    if (playlists != null) {
                        Console.WriteLine("\nYour YouTube Playlists:");
                        foreach (var playlist in playlists) {
                            Console.WriteLine($"- {playlist}");
                        }
                    }
                    else {
                        Console.WriteLine("Failed to retrieve playlists.");
                    }
                }
                else {
                    Console.WriteLine("Failed to initialize YouTube API. Program will exit.");
                    Environment.Exit(1);
                }
            });

            Console.ReadKey();
        }
    }
}