namespace YTMusicLocalSync;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

public class API
{
    private const string ClientSecretsFile = "googleusercontent.com.json"; // Path to your client secrets file

    public async Task Start(Func<YouTubeService, Task> callback)
    {
        YouTubeService youtubeService = await InitializeYoutubeService();
        if (youtubeService != null)
        {
            await callback(youtubeService);
        }
        else
        {
            Console.WriteLine("Failed to initialize YouTube Service.");
            await callback(null); // Indicate failure to the callback
        }
    }

    private async Task<YouTubeService> InitializeYoutubeService()
    {
        UserCredential credential;
        try
        {
            using (var stream = new FileStream(ClientSecretsFile, FileMode.Open, FileAccess.Read))
            {
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    new[] { YouTubeService.Scope.YoutubeReadonly }, // Scope for read-only access to YouTube account
                    "user",
                    System.Threading.CancellationToken.None);
            }
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine($"Error: Client secrets file '{ClientSecretsFile}' not found next to the executable.");
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading client secrets or authorizing: {ex.Message}");
            return null;
        }

        if (credential != null && credential.Token != null) // Basic null check, more robust validation might be needed
        {
            return new YouTubeService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "YouTubePlaylistApp" // Replace with your application name
            });
        }
        else
        {
            Console.WriteLine("Failed to obtain user credentials.");
            return null;
        }
    }

    public async Task<List<string>> GetUserPlaylistsAsync(YouTubeService youtubeService)
    {
        if (youtubeService == null) return null;

        try
        {
            var playlistsListRequest = youtubeService.Playlists.List("snippet");
            playlistsListRequest.Mine = true; // To retrieve playlists for the authenticated user
            playlistsListRequest.MaxResults = 50; // Adjust as needed, max is 50

            PlaylistListResponse playlistsResponse = await playlistsListRequest.ExecuteAsync();
            List<string> playlistTitles = new List<string>();

            if (playlistsResponse.Items != null)
            {
                foreach (var playlist in playlistsResponse.Items)
                {
                    playlistTitles.Add(playlist.Snippet.Title);
                }
            }
            return playlistTitles;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving playlists: {ex.Message}");
            return null;
        }
    }
}