using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;

namespace YTMusicLocalSync;

public static class API {
    public static async Task StartAsync(Func<YouTubeService, Task> callback) {
        try {
            string credentialsPath = Path.Combine(AppContext.BaseDirectory, "googleusercontent.com.json");
            using var stream = new FileStream(credentialsPath, FileMode.Open, FileAccess.Read);
            var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                GoogleClientSecrets.FromStream(stream).Secrets,
                new[] { YouTubeService.Scope.YoutubeReadonly },
                "user",
                CancellationToken.None,
                new FileDataStore("YouTube.Api.Auth.Store", true)
            );

            var youtubeService = new YouTubeService(new BaseClientService.Initializer {
                HttpClientInitializer = credential,
                ApplicationName = "MyApp"
            });

            await callback(youtubeService);
        }
        catch (Exception ex) {
            Console.WriteLine($"Failed to connect to YouTube API: {ex.Message}");
            Environment.Exit(1);
        }
    }

    public static async Task<List<Playlist>> GetPlaylistsAsync(YouTubeService youtubeService) {
        var playlists = new List<Playlist>();
        string nextPageToken = "";
        while (nextPageToken != null) {
            var request = youtubeService.Playlists.List("snippet,contentDetails");
            request.Mine = true;
            request.MaxResults = 50;
            request.PageToken = nextPageToken;
            var response = await request.ExecuteAsync();
            if (response.Items != null) {
                playlists.AddRange(response.Items);
            }

            nextPageToken = response.NextPageToken;
        }

        return playlists;
    }
}