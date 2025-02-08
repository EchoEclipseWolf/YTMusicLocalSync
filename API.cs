using Google.Apis.Auth.OAuth2;
using Google.Apis.YouTube.v3;
using System.Text.Json;

namespace YTMusicLocalSync
{
    public class API
    {
        private string _apiKey = "";
        public async Task Start(Func<Task> callback)
        {
            // Check if googleusercontent.com.json exists
            if (!File.Exists("googleusercontent.com.json"))
            {
                Console.WriteLine("googleusercontent.com.json not found");
                Environment.Exit(0);
                return;
            }
            try
            {
                // Load credentials from file
                var json = File.ReadAllText("googleusercontent.com.json");
                var credential = GoogleJsonWebSignature.From(json);

                // Create Youtube Service
                youtubeService = new YouTubeService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = new HttpClientInitializer()
                    {
                        ApplicationName = "YTMusicLocalSync",
                        ApplicationVersion = "v1",
                    },
                    Credential = credential
                });
            
        }
        public List<string> GetPlaylists()
        {
            List<string> playlistTitles = new List<string>();
            // Replace with actual implementation to get the playlists from the users youtube account
            var listRequest = youtubeService.Playlists.List();
            listRequest.MaxResults = 50; // set max results to avoid loading time outs.
            var playlists = await listRequest.ExecuteAsync();

            if(playlists.Items == null)
            {
                return new List<string>();
            }

            foreach (var playlist in playlists.Items)
            {
                playlistTitles.Add(playlist.Snippet.Title);
            }
            return playlistTitles;
        }
    }

        public static GoogleJsonWebSignature From(GoogleJsonWebSignature signature)
        {
            return new GoogleJsonWebSignature(signature.ClientId, signature.ClientSecret, signature.IssuedAt, signature.ExpiresAt);
        }
    }

    public class YouTubeService
    {
        public YouTubeService(BaseClientService.Initializer initializer)
        {
            // Implementation
        }
    }
    public class BaseClientService
    {
        public class Initializer
        {
             public Initializer(GoogleJsonWebSignature signature)
            {
                // Implementation
            }
        }
    }

}
