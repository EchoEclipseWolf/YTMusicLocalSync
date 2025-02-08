using System;

namespace YTMusicLocalSync
{
    class Program
    {
        private static readonly Config _config = new();

        public static async Task Main(string[] args)
        {
            _config.Load();

            await API.StartAsync(async youtubeService =>
            {
                var playlists = await API.GetPlaylistsAsync(youtubeService);
                Console.WriteLine("User Playlists:");
                foreach (var playlist in playlists)
                {
                    Console.WriteLine($"{playlist.Snippet.Title} (ID: {playlist.Id})");
                }
            });

            Console.WriteLine("Application running. Press Ctrl+C to exit.");
            await Task.Delay(-1);
        }
    }
}