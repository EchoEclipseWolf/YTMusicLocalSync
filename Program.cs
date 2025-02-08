using System;

namespace YTMusicLocalSync
{
    class Program
    {
        public static Config config = new Config();
        static async Task Main(string[] args)
        {
            config.Load();
            var api = new API();
            while(true){
                await api.Start(async () =>
                {
                    var playlists = api.GetPlaylists();
                    foreach (var item in playlists)
                    {
                        Console.WriteLine(item);
                    }
                    await Task.Delay(100);
                    
                });
            }
        }
    }
}