using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FabToolKit.Api;

public class OAuth2AuthConfig : ApiAuthConfig
{
    private readonly string _tokenEndpoint;
    private readonly string _clientId;
    private readonly string _clientSecret;
    private readonly string? _scope;
    private readonly Dictionary<string, string>? _authFormData;
    private readonly HttpClient _httpClient;

    private string? _accessToken;
    private DateTime _expiresAt = DateTime.MinValue;

    // Buffer subtracted from expires_in to avoid using a token right as it expires.
    private const int ExpiryBufferSeconds = 60;

    // Guards against concurrent token fetches issuing two requests to the token endpoint.
    private readonly SemaphoreSlim _tokenLock = new(1, 1);


    public OAuth2AuthConfig(string tokenEndpoint, string clientId, string clientSecret, string? scope = null, HttpClient? httpClient = null, Dictionary<string, string>? authFormData = null)
    {
        _tokenEndpoint = tokenEndpoint;
        _clientId = clientId;
        _clientSecret = clientSecret;
        _scope = scope;
        _httpClient = httpClient ?? new HttpClient();
        _authFormData = authFormData;
    }


    // Token is fetched lazily on the first request; nothing to do at construction time.
    internal override void Apply(HttpRequestHeaders headers) { }

    internal override async Task PrepareAsync(HttpRequestHeaders headers)
    {
        if (TokenIsValid())
        {
            ApplyToken(headers);
            return;
        }

        await _tokenLock.WaitAsync();
        try
        {
            // Re-check after acquiring the lock — another thread may have refreshed already.
            if (TokenIsValid())
            {
                ApplyToken(headers);
                return;
            }

            await FetchTokenAsync(headers);
        }
        finally
        {
            _tokenLock.Release();
        }
    }


    private bool TokenIsValid() =>
        !string.IsNullOrEmpty(_accessToken) && DateTime.UtcNow < _expiresAt;

    private async Task FetchTokenAsync(HttpRequestHeaders headers)
    {
        var formData = new Dictionary<string, string>
        {
            ["grant_type"] = "client_credentials",
            ["client_id"] = _clientId,
            ["client_secret"] = _clientSecret
        };

        if (_authFormData is not null)
        {
            foreach (var kvp in _authFormData)
            {
                formData[kvp.Key] = kvp.Value;
            }
        }

        if (!string.IsNullOrEmpty(_scope))
            formData["scope"] = _scope;

        var response = await _httpClient.PostAsync(_tokenEndpoint, new FormUrlEncodedContent(formData));
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var token = JsonSerializer.Deserialize<OAuth2TokenResponse>(json)
            ?? throw new InvalidOperationException($"OAuth2 token endpoint returned an empty or unparseable response.\n\nResponse:\n{json}");

        _accessToken = token.AccessToken;
        int expiresIn = token.ExpiresIn > 0 ? token.ExpiresIn : 3600;
        _expiresAt = DateTime.UtcNow.AddSeconds(expiresIn - ExpiryBufferSeconds);

        ApplyToken(headers);
    }

    private void ApplyToken(HttpRequestHeaders headers) =>
        headers.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);


    private class OAuth2TokenResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; } = string.Empty;

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; } = string.Empty;
    }
}
