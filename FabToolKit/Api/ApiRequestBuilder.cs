namespace FabToolKit.Api;


public class ApiRequestBuilder
{
    private readonly ApiServiceBase apiClient;
    public readonly HttpRequestMessage Request;
    public readonly Dictionary<string, string> Headers;
    public readonly Dictionary<string, string> Parameters;
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
        this.apiClient = apiClient;
        this.Request = new HttpRequestMessage { RequestUri = new Uri($"{apiClient.BaseURL}{endpoint}") };
        this.Headers = new();
        this.Parameters = new();
    }

    public ApiRequestBuilder AddQueryParameter(string key, string value)
    {
        //add query param to member dict
        this.Parameters.Add(key, value);

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
        string modifiedUri = originalUri.Replace($"{{{key}}}", value);
        Request.RequestUri = new Uri(modifiedUri);
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
        this.Headers.Add(headerName, headerValue);

        //add to request
        this.Request.Headers.Add(headerName, headerValue);
        return this;
    }

    public async Task<HttpResponseMessage> GetAsync()
    {
        Request.Method = HttpMethod.Get;
        return await apiClient.ExecuteRequestAsync(Request);
    }

    public async Task<string> GetContentAsync()
    {
        Request.Method = HttpMethod.Get;
        return await apiClient.ExecuteRequestReturnContentAsync(Request);
    }

    public async Task<HttpResponseMessage> PostAsync()
    {
        Request.Method = HttpMethod.Post;
        return await apiClient.ExecuteRequestAsync(Request);
    }

    public async Task<string> PostContentAsync(string requestBody)
    {
        Request.Method = HttpMethod.Post;
        Request.Content = new StringContent(requestBody, System.Text.Encoding.UTF8, "application/json");
        return await apiClient.ExecuteRequestReturnContentAsync(Request);
    }
}
