using System;

namespace YTMusicLocalSync
{
    class Program
    {
        public static Config config;
        static async Task Main(string[] args)
        {
            config = new Config();
            config.Load();
            
            API api = new API();
            await api.Start(async () =>
            {
                List<string> playlists = api.GetPlaylists();
                Console.WriteLine("User Playlists: ");
                foreach(string item in playlists)
                {
                    Console.WriteLine(item);
                }
            });
        }
    }
}