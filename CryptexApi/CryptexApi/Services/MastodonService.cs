using CryptexApi.Models.Options;
using CryptexApi.Services.Interfaces;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;
using Tweetinvi;
using Tweetinvi.Exceptions;

namespace CryptexApi.Services
{
    public class MastodonService : IMastodonService
    {
        private readonly HttpClient _httpClient;
        private readonly string _instanceUrl;
        private readonly string _accessToken;

        public MastodonService(IOptions<MastodonApiOptions> options, HttpClient httpClient)
        {
            _httpClient = httpClient;
            var mastodonOptions = options.Value;

            _instanceUrl = mastodonOptions.InstanceUrl;
            _accessToken = mastodonOptions.AccessToken;

            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _accessToken);
        }

        public async Task<bool> PostStatusAsync(string message)
        {
            var content = new StringContent(
                JsonSerializer.Serialize(new { status = message }),
                Encoding.UTF8,
                "application/json");

            try
            {
                var response = await _httpClient.PostAsync($"{_instanceUrl}/api/v1/statuses", content);
                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Mastodon API Error: {error}");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error posting to Mastodon: {ex.Message}");
                return false;
            }
        }
    }
}
