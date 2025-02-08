using System;
using System;
using System.Threading.Tasks;

namespace YTMusicLocalSync {
    internal class Program {
        public static Config Configuration { get; private set; }

        static async Task Main(string[] args) {
// Main program loop
            while (true)
            {
                Console.WriteLine("Options:");
                Console.WriteLine("1. Load YouTube Playlists");
                Console.WriteLine("2. Exit");
                Console.Write("Enter your choice: ");

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.WriteLine("Loading YouTube Playlists...");
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

                                var likedVideosPlaylist = await api.GetLikedVideosPlaylistAsync(youtubeService);
                                if (likedVideosPlaylist != null) {
                                    Console.WriteLine($"\nYour Liked Videos Playlist: {likedVideosPlaylist}");
                                }
                                else {
                                    Console.WriteLine("Failed to retrieve liked videos playlist.");
                                }
                            }
                            else {
                                Console.WriteLine("Failed to initialize YouTube API. Program will exit.");
                                Environment.Exit(1);
                            }
                        });

                        Console.ReadKey();
                        break;
                    case "2":
                        Console.WriteLine("Exiting.");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                Console.Clear();
            }
        }
    }
}