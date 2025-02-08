using System.Reflection;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util;
using Google.Apis.YouTube.v3;

namespace YTMusicLocalSync;

public class API {
    private YouTubeService? _youTubeService;
        private readonly string _credentialsPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "googleusercontent.com.json");

        public async Task Start(Action onConnectedCallback)
        {
            try
            {
                UserCredential credential;

                using (var stream = new FileStream(_credentialsPath, FileMode.Open, FileAccess.Read))
                {
                    credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                       GoogleClientSecrets.Load(stream).Secrets,
                       new[] { YouTubeService.Scope.YoutubeReadonly },
                       "user",
                       CancellationToken.None
                       );
                }

                _youTubeService = new YouTubeService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "MusicPlaylistSync"
                });

                Console.WriteLine("Successfully connected to YouTube API.");
                onConnectedCallback();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error connecting to YouTube API: {ex.Message}");
                Environment.Exit(1); // Exit the program on connection failure.
            }
        }

        public async Task<List<string>> GetUserPlaylists() {
            if (_youTubeService == null) {
                throw new InvalidOperationException("YouTubeService not initialized.  Call Start() first.");
            }

            List<string> playlistTitles = new List<string>();
            string? nextPageToken = null;

            do {
                var playlistsRequest = _youTubeService.Playlists.List("snippet");
                playlistsRequest.Mine = true;
                playlistsRequest.MaxResults = 50; // Maximum allowed by the API
                playlistsRequest.PageToken = nextPageToken;


                try {
                    var playlistsResponse = await playlistsRequest.ExecuteAsync();

                    foreach (var playlist in playlistsResponse.Items) {
                        playlistTitles.Add(playlist.Snippet.Title);
                    }

                    nextPageToken = playlistsResponse.NextPageToken;
                }
                catch (Google.GoogleApiException ex) {
                    Console.WriteLine($"YouTube API Error: {ex.Message}");
                    //Consider handling specific API errors (e.g., rate limits)
                    return new List<string>(); // Return an empty list on error
                }
                catch (Exception ex) {
                    Console.WriteLine($"An unexpected error occurred while fetching playlists: {ex.Message}");
                    return new List<string>(); // Return an empty list on error.
                }

            } while (nextPageToken != null);

            return playlistTitles;
        }
}