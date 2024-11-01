namespace FabToolKit.Api;

public abstract class ApiServiceBase
{
    public readonly string BaseURL;
    private readonly HttpClient _httpClient;
    private readonly string? _basicAuthenticationKey;
    private readonly string? _basicAuthenticationSecret;


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
                        : this(baseUrl, basicAuthenticationKey, basicAuthenticationSecret)
    {
        this._httpClient = httpClient;
    }

    

    #endregion Ctors


    #region PrivateUtilities
    // Private method to set the basic authentication header
    private void SetBasicAuthenticationHeader()
    {
        var credentials = Convert.ToBase64String(
            System.Text.Encoding.ASCII.GetBytes($"{_basicAuthenticationKey}:{_basicAuthenticationSecret}"));

        _httpClient.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);
    }

    #endregion PrivateUtilities

    public ApiRequestBuilder SetEndpoint(string endpoint)
    {
        return new ApiRequestBuilder(this, endpoint);
    }

    public async Task<string> ExecuteRequestReturnContentAsync(HttpRequestMessage request)
    {
        HttpResponseMessage response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<HttpResponseMessage> ExecuteRequestAsync(HttpRequestMessage request)
    {
        return await _httpClient.SendAsync(request);
    }

}
