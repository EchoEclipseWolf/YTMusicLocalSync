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
using Google.Apis.Auth.OAuth2.Flows;

namespace YTMusicLocalSync
{
    public class API
    {
        public YouTubeService youtubeService;
        public async Task Start(Action callback)
        {
            try {
                // Load client secrets

                await Task.Factory.StartNew(async () => {
                    using (var stream = new FileStream("googleusercontent.com.json", FileMode.Open, FileAccess.Read))
                    {
                        GoogleClientSecrets clientSecrets = await GoogleClientSecrets.FromStreamAsync(stream);
                        if (clientSecrets != null) {
                            try {
                                UserCredential credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                                    clientSecrets.Secrets,
                                    new[] { YouTubeService.Scope.YoutubeReadonly }, // Specify the required scopes
                                    "user",
                                    CancellationToken.None,
                                    new FileDataStore(this.GetType().ToString()));

                                YouTubeService service = new YouTubeService(new BaseClientService.Initializer {
                                    HttpClientInitializer =
                                        Google.Apis.Auth.OAuth2.GoogleCredential.FromAccessToken(credential.Token
                                            .AccessToken)
                                });
                            }
                            catch (Exception e) {
                                Console.WriteLine(e);
                                throw;
                                
                            }
                            

                            int bob = 1;
                        }
                    }
                });

                
            }
            catch (Exception ex) {
                Console.WriteLine("Failed to start API connection.");
                Environment.Exit(1);
            }
        }

        public List<string> GetPlaylists()
        {
            List<string> Playlists = new List<string>();
            var channelsListRequest = youtubeService.Playlists.List("me");
            channelsListRequest.MaxResults = 10;
            /*foreach(var item in channelsListRequest.Items)
            {
                Playlists.Add(item.Snippet.Title);
            }*/
            return Playlists;
        }
    }
}
