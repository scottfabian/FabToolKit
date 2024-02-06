namespace FabToolKit.Api;

public class ApiServiceBase
{
    public readonly string BaseURL;
    private readonly HttpClient httpClient;
    internal readonly string? basicAuthenticationKey;
    internal readonly string? basicAuthenticationSecret;


    #region Ctors

    public ApiServiceBase(string baseUrl)
    {
        this.BaseURL = baseUrl;
        this.httpClient = new HttpClient();
    }

    public ApiServiceBase(string baseUrl, HttpClient httpClient)
    {
        this.BaseURL = baseUrl;
        this.httpClient = httpClient;
    }

    public ApiServiceBase(string baseUrl, string basicAuthenticationKey, string basicAuthenticationSecret)
    {
        this.BaseURL = baseUrl;
        this.basicAuthenticationKey = basicAuthenticationKey;
        this.basicAuthenticationSecret = basicAuthenticationSecret;
        this.httpClient = new();
        SetBasicAuthenticationHeader();
    }

    public ApiServiceBase(string baseUrl, HttpClient httpClient, string basicAuthenticationKey, string basicAuthenticationSecret)
    {
        this.BaseURL = baseUrl;
        this.basicAuthenticationKey = basicAuthenticationKey;
        this.basicAuthenticationSecret = basicAuthenticationSecret;
        this.httpClient = httpClient;
        SetBasicAuthenticationHeader();
    }

    #endregion Ctors


    #region PrivateUtilities
    // Private method to set the basic authentication header
    private void SetBasicAuthenticationHeader()
    {
        var credentials = Convert.ToBase64String(
            System.Text.Encoding.ASCII.GetBytes($"{basicAuthenticationKey}:{basicAuthenticationSecret}"));
        httpClient.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);
    }

    #endregion PrivateUtilities

    public ApiRequestBuilder SetEndpoint(string endpoint)
    {
        return new ApiRequestBuilder(this, endpoint);
    }

    public async Task<string> ExecuteRequestReturnContentAsync(HttpRequestMessage request)
    {
        HttpResponseMessage response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<HttpResponseMessage> ExecuteRequestAsync(HttpRequestMessage request)
    {
        return await httpClient.SendAsync(request);
    }

}
