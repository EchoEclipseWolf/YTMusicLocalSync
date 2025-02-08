using System;

namespace YTMusicLocalSync {
    class Program {
        private static readonly Config _config = new Config();
        private static readonly API _api = new API();

        static async Task Main(string[] args) {
            _config.Load();

            await _api.Start(async () => {
                try {
                    List<string> playlists = await _api.GetUserPlaylists();
                    Console.WriteLine("\nYour YouTube Playlists:");
                    if (playlists.Count > 0) {
                        foreach (string playlist in playlists) {
                            Console.WriteLine($"- {playlist}");
                        }
                    }
                    else {
                        Console.WriteLine("No playlists found.");
                    }
                }
                catch (Exception ex) {
                    Console.WriteLine($"Error fetching playlists: {ex.Message}");
                }
            });

            //Keep the program alive until the user terminates it.
            Console.WriteLine("\nSync complete. Press any key to exit.");
            Console.ReadKey();
        }
    }

}