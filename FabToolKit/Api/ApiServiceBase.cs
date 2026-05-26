namespace FabToolKit.Api;

public abstract class ApiServiceBase
{
    public readonly string BaseURL;
    private readonly HttpClient _httpClient;
    private readonly enumAuthType _authType;
    private readonly string? _basicAuthenticationKey;
    private readonly string? _basicAuthenticationSecret;
    private string? _bearerToken;


    #region Ctors

    protected ApiServiceBase(string baseUrl)
    {
        this.BaseURL = baseUrl;
        this._httpClient = new HttpClient();
    }

    protected ApiServiceBase(string baseUrl, HttpClient httpClient)
    {
        this.BaseURL = baseUrl;
        this._httpClient = httpClient;
    }

    protected ApiServiceBase(string baseUrl, string basicAuthenticationKey, string basicAuthenticationSecret)
    {
        this.BaseURL = baseUrl;
        this._basicAuthenticationKey = basicAuthenticationKey;
        this._basicAuthenticationSecret = basicAuthenticationSecret;
        this._httpClient = new();
        SetBasicAuthenticationHeader();
    }

    protected ApiServiceBase(string baseUrl, HttpClient httpClient, string basicAuthenticationKey, string basicAuthenticationSecret)
    : this(baseUrl, httpClient)
    {
        this._basicAuthenticationKey = basicAuthenticationKey;
        this._basicAuthenticationSecret = basicAuthenticationSecret;
        SetBasicAuthenticationHeader();
    }


    protected ApiServiceBase(string baseUrl, string bearerToken, HttpClient? httpClient = null, enumAuthType authType = enumAuthType.Bearer)
    {
        this.BaseURL = baseUrl;
        this._bearerToken = bearerToken;
        this._httpClient = httpClient ?? new HttpClient();
        this._authType = authType;
        SetBearerTokenHeader();
    }



    #endregion Ctors


    #region AuthenticationManagement

    

    // Private method to set the basic authentication header
    private protected void SetBasicAuthenticationHeader()
    {
        var credentials = Convert.ToBase64String(
            System.Text.Encoding.UTF8.GetBytes($"{_basicAuthenticationKey}:{_basicAuthenticationSecret}"));

        _httpClient.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);
    }

    private void SetBearerTokenHeader()
    {
        if (!string.IsNullOrWhiteSpace(_bearerToken))
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _bearerToken);
        }
    }

    public void UpdateBearerToken(string newBearerToken)
    {
        _bearerToken = newBearerToken;
        SetBearerTokenHeader();
    }

    #endregion AuthenticationManagement

    public ApiRequestBuilder SetEndpoint(string endpoint)
    {
        return new ApiRequestBuilder(this, endpoint);
    }

    public async Task<string> ExecuteRequestReturnStringContentAsync(HttpRequestMessage request)
    {
        HttpResponseMessage response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<byte[]> ExecuteRequestReturnByteArrayAsync(HttpRequestMessage request)
    {
        HttpResponseMessage response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsByteArrayAsync();
    }

    public async Task<HttpResponseMessage> ExecuteRequestAsync(HttpRequestMessage request)
    {
        return await _httpClient.SendAsync(request);
    }

}
