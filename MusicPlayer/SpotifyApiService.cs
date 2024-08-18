using SpotifyAPI.Web;
using System.Net.Http;
using System.Threading.Tasks;

namespace MusicPlayer
{
    public class SpotifyApiService
    {
        private SpotifyClient _spotifyClient;
        private HttpClient _client;

        public async Task InitializeAsync(string clientId, string clientSecret)
        {
            var config = SpotifyClientConfig
                .CreateDefault()
                .WithAuthenticator(new ClientCredentialsAuthenticator(clientId, clientSecret));

            _spotifyClient = new SpotifyClient(config);
        }

        public async Task<FullTrack> GetTrackAsync(string trackId)
        {
            return await _spotifyClient.Tracks.Get(trackId);
        }

        public async Task<SearchResponse> SearchTracksAsync(string query)
        {
            return await _spotifyClient.Search.Item(new SearchRequest(SearchRequest.Types.Track, query));
        }
    }
}
