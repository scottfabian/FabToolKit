namespace FabToolKit.Api;


public class ApiRequestBuilder
{
    private readonly ApiServiceBase _apiClient;
    public readonly HttpRequestMessage Request;
    public readonly Dictionary<string, string> Headers = new();
    public readonly Dictionary<string, string> QueryParameters = new();
    public readonly Dictionary<string, string> InjectedParameters = new();
    public string EndpointPath { get; private set; }

    public string FullRequestURI
    {
        get
        {
            if (this.Request.RequestUri is not null)
            {
                return this.Request.RequestUri.ToString();
            }
            return "Uri Null";
        }
    }

    public ApiRequestBuilder(ApiServiceBase apiClient, string endpoint)
    {
        this._apiClient = apiClient;
        this.EndpointPath = endpoint;
        this.Request = new HttpRequestMessage { RequestUri = new Uri($"{apiClient.BaseURL}{endpoint}") };
    }

    public ApiRequestBuilder AddQueryParameter(string key, string value)
    {
        //add query param to member dict
        this.QueryParameters[key] = value;

        //add query param to uri
        var uriBuilder = new UriBuilder(Request.RequestUri!);
        var query = System.Web.HttpUtility.ParseQueryString(uriBuilder.Query);
        query[key] = value;
        uriBuilder.Query = query.ToString();
        Request.RequestUri = uriBuilder.Uri;
        return this;
    }

    public ApiRequestBuilder InjectQueryParameter(string key, string value)
    {
        string originalUri = Request.RequestUri!.ToString();

        if (originalUri.Contains(key))
        {
            string modifiedUri = originalUri.Replace($"{{{key}}}", value);
            this.InjectedParameters[key] = value;
            Request.RequestUri = new Uri(modifiedUri);
        }

        if (EndpointPath.Contains(key))
        {
            this.EndpointPath = EndpointPath.Replace($"{{{key}}}", value);
        }

        return this;
    }

    public ApiRequestBuilder InjectQueryParameter(Dictionary<string, string> parameters)
    {
        foreach (var kvp in parameters)
        {
            this.InjectQueryParameter(kvp.Key, kvp.Value);
        }
        return this;
    }

    public ApiRequestBuilder AddRequestHeader(string headerName, string headerValue)
    {


        //remove the header if it already exists

        //member dict
        if (this.Headers.ContainsKey(headerName))
        {
            this.Headers.Remove(headerName);
        }

        //request
        if (this.Request.Headers.Contains(headerName))
        {
            this.Request.Headers.Remove(headerName);
        }


        //add to member dict
        this.Headers[headerName] = headerValue;

        //add to request
        this.Request.Headers.Add(headerName, headerValue);
        return this;
    }

    public async Task<HttpResponseMessage> GetResponseBodyAsync()
    {
        Request.Method = HttpMethod.Get;
        return await _apiClient.ExecuteRequestAsync(Request);
    }

    public async Task<string> GetContentAsync()
    {
        Request.Method = HttpMethod.Get;
        return await _apiClient.ExecuteRequestReturnStringContentAsync(Request);
    }

    public async Task<byte[]> GetByteArrayAsync()
    {
        Request.Method = HttpMethod.Get;
        return await _apiClient.ExecuteRequestReturnByteArrayAsync(Request);
    }

    public async Task<HttpResponseMessage> PostAsync()
    {
        Request.Method = HttpMethod.Post;
        return await _apiClient.ExecuteRequestAsync(Request);
    }

    public async Task<string> PostContentAsync(string requestBody)
    {
        Request.Method = HttpMethod.Post;
        Request.Content = new StringContent(requestBody, System.Text.Encoding.UTF8, "application/json");
        return await _apiClient.ExecuteRequestReturnStringContentAsync(Request);
    }
}
