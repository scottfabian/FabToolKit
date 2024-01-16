namespace FabToolKit.Api;


public class ApiRequestBuilder
{
    private readonly ApiServiceBase apiClient;
    private readonly HttpRequestMessage request;

    public ApiRequestBuilder(ApiServiceBase apiClient, string endpoint)
    {
        this.apiClient = apiClient;
        this.request = new HttpRequestMessage { RequestUri = new Uri($"{apiClient.baseUrl}{endpoint}") };
    }

    public ApiRequestBuilder AddQueryParameter(string key, string value)
    {
        var uriBuilder = new UriBuilder(request.RequestUri!);
        var query = System.Web.HttpUtility.ParseQueryString(uriBuilder.Query);
        query[key] = value;
        uriBuilder.Query = query.ToString();
        request.RequestUri = uriBuilder.Uri;
        return this;
    }

    public ApiRequestBuilder InjectQueryParameter(string key, string value)
    {
        string originalUri = request.RequestUri!.ToString();
        string modifiedUri = originalUri.Replace($"{{{key}}}", value);
        request.RequestUri = new Uri(modifiedUri);
        return this;
    }

    public ApiRequestBuilder AddRequestHeader(string headerName, string headerValue)
    {
        //remove the header if it already exists
        if (this.request.Headers.Contains(headerName))
        {
            this.request.Headers.Remove(headerName);
        }

        this.request.Headers.Add(headerName, headerValue);
        return this;
    }

    public ApiRequestBuilder AddApiKeyQueryParameter(string apiKey)
    {
        return AddQueryParameter("apiKey", apiKey);
    }

    public async Task<HttpResponseMessage> GetAsync()
    {
        request.Method = HttpMethod.Get;
        return await apiClient.ExecuteRequestAsync(request);
    }

    public async Task<string> GetContentAsync()
    {
        request.Method = HttpMethod.Get;
        return await apiClient.ExecuteRequestReturnContentAsync(request);
    }

    public async Task<HttpResponseMessage> PostAsync()
    {
        request.Method = HttpMethod.Post;
        return await apiClient.ExecuteRequestAsync(request);
    }

    public async Task<string> PostContentAsync(string requestBody)
    {
        request.Method = HttpMethod.Post;
        request.Content = new StringContent(requestBody, System.Text.Encoding.UTF8, "application/json");
        return await apiClient.ExecuteRequestReturnContentAsync(request);
    }
}
