namespace FabToolKit.Api;


public class ApiRequestBuilder
{
    private readonly ApiServiceBase _apiClient;
    internal readonly HttpRequestMessage Request;
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

    #region RequestBuilders

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

        if (originalUri.Contains($"{{{key}}}"))
        {
            Request.RequestUri = new Uri(originalUri.Replace($"{{{key}}}", value));
            EndpointPath = EndpointPath.Replace($"{{{key}}}", value);
            this.InjectedParameters[key] = value;
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

    #endregion RequestBuilders

    #region Helpers

    private async Task<HttpResponseMessage> SendRequestWithBody(string requestBody, HttpMethod verb, string contentType)
    {
        Request.Method = verb;
        Request.Content = new StringContent(requestBody, System.Text.Encoding.UTF8, contentType);
        return await _apiClient.ExecuteRequestAsync(Request);
    }

    #endregion Helpers

    #region HttpMethods

    #region GET

    public async Task<HttpResponseMessage> GetAsync()
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

    #endregion GET


    #region POST

    public async Task<HttpResponseMessage> PostAsync()
    {
        Request.Method = HttpMethod.Post;
        return await _apiClient.ExecuteRequestAsync(Request);
    }

    public async Task<HttpResponseMessage> PostAsync(string requestBody, string contentType) => await SendRequestWithBody(requestBody, HttpMethod.Post, contentType);

    #endregion POST


    #region PUT

    public async Task<HttpResponseMessage> PutAsync()
    {
        Request.Method = HttpMethod.Put;
        return await _apiClient.ExecuteRequestAsync(Request);
    }

    public async Task<HttpResponseMessage> PutAsync(string requestBody, string contentType) => await SendRequestWithBody(requestBody, HttpMethod.Put, contentType);

    #endregion PUT


    #region PATCH

    public async Task<HttpResponseMessage> PatchAsync()
    {
        Request.Method = HttpMethod.Patch;
        return await _apiClient.ExecuteRequestAsync(Request);
    }

    public async Task<HttpResponseMessage> PatchAsync(string requestBody, string contentType) => await SendRequestWithBody(requestBody, HttpMethod.Patch, contentType);

    #endregion PATCH


    #region DELETE

    public async Task<HttpResponseMessage> DeleteAsync()
    {
        Request.Method = HttpMethod.Delete;
        return await _apiClient.ExecuteRequestAsync(Request);
    }

    public async Task<HttpResponseMessage> DeleteAsync(string requestBody, string contentType) => await SendRequestWithBody(requestBody, HttpMethod.Delete, contentType);

    #endregion DELETE

    #endregion HttpMethods
  
}
