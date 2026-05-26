namespace FabToolKit.Api;

public abstract class ApiServiceBase
{
    public readonly string BaseURL;
    private readonly HttpClient _httpClient;
    private ApiAuthConfig? _authConfig;


    #region Ctors

    protected ApiServiceBase(string baseUrl, ApiAuthConfig? auth = null)
    {
        BaseURL = baseUrl;
        _httpClient = new HttpClient();
        _authConfig = auth;
        auth?.Apply(_httpClient.DefaultRequestHeaders);
    }

    protected ApiServiceBase(string baseUrl, HttpClient httpClient, ApiAuthConfig? auth = null)
    {
        BaseURL = baseUrl;
        _httpClient = httpClient;
        _authConfig = auth;
        auth?.Apply(_httpClient.DefaultRequestHeaders);
    }

    #endregion Ctors


    #region AuthenticationManagement

    public void UpdateBearerToken(string newToken)
    {
        if (_authConfig is BearerAuthConfig bearer)
        {
            bearer.UpdateToken(newToken);
            bearer.Apply(_httpClient.DefaultRequestHeaders);
        }
        else
        {
            throw new InvalidOperationException("UpdateBearerToken requires the service to be configured with BearerAuthConfig.");
        }
    }

    #endregion AuthenticationManagement


    public ApiRequestBuilder SetEndpoint(string endpoint)
    {
        return new ApiRequestBuilder(this, endpoint);
    }

    public async Task<string> ExecuteRequestReturnStringContentAsync(HttpRequestMessage request)
    {
        if (_authConfig != null) await _authConfig.PrepareAsync(_httpClient.DefaultRequestHeaders);
        HttpResponseMessage response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<byte[]> ExecuteRequestReturnByteArrayAsync(HttpRequestMessage request)
    {
        if (_authConfig != null) await _authConfig.PrepareAsync(_httpClient.DefaultRequestHeaders);
        HttpResponseMessage response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsByteArrayAsync();
    }

    public async Task<HttpResponseMessage> ExecuteRequestAsync(HttpRequestMessage request)
    {
        if (_authConfig != null) await _authConfig.PrepareAsync(_httpClient.DefaultRequestHeaders);
        return await _httpClient.SendAsync(request);
    }

}
