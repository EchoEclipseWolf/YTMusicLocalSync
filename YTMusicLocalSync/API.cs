using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using System.Net.Http;

namespace YTMusicLocalSync
{
    public class API
    {
        public YouTubeService youtubeService;
        public async Task Start(Action callback)
        {
            try
            {
                // Load client secrets
                using (var stream = new FileStream("googleusercontent.com.json"))
                {
                    using (var reader = new StreamReader(stream))
                    {
                        GoogleClientSecrets clientSecrets = GoogleClientSecrets.Load(reader).Result;
                        var initializer = new GoogleAuthorizationCodeFlow.Initializer(clientSecrets);
                        UserCredential userCredential = initializer.CreateCredential(new[] { YouTubeService.Scope });

                        // Initialize the YouTube service.
                        youtubeService = new YouTubeService(new BaseClientService.Initializer() { ApplicationName = "YTMusicLocalSync", HttpClient = new HttpClient() });
                        youtubeService.HttpClient.MessageHandler += delegate (object sender, System.Net.Http.HttpResponseMessage e)
                        {

                        };
                        await youtubeService.AuthenticateAsync(userCredential);
                    }
                }
                await callback.Invoke();
            }
            catch(Exception ex)
            {
                Console.WriteLine("Failed to start API connection.");
                Environment.Exit(1);
            }
        }

        public List<string> GetPlaylists()
        {
            List<string> Playlists = new List<string>();
            var channelsListRequest = youtubeService.Playlists.List("me");
            channelsListRequest.MaxResults = 10;
            foreach(var item in channelsListRequest.Items)
            {
                Playlists.Add(item.Snippet.Title);
            }
            return Playlists;
        }
    }
}
