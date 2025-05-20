using System.Text.Json;
using System.Text.Json.Serialization;

namespace FabToolKit.Api.GraphQL;

public class GraphQLServiceBase : ApiServiceBase
{
    public string GraphEndpoint { get; set; }
    private JsonSerializerOptions _jsonOptions = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public GraphQLServiceBase(string baseUrl, string graphEndpoint) : base(baseUrl)
    {
        GraphEndpoint = graphEndpoint;
    }

    public GraphQLServiceBase(string baseUrl, string graphEndpoint, HttpClient httpClient) : base(baseUrl, httpClient)
    {
        GraphEndpoint = graphEndpoint;
    }

    public GraphQLServiceBase(string baseUrl, string graphEndpoint, string basicAuthenticationKey, string basicAuthenticationSecret) : base(baseUrl, basicAuthenticationKey, basicAuthenticationSecret)
    {
        GraphEndpoint = graphEndpoint;
    }

    public GraphQLServiceBase(string baseUrl, string graphEndpoint, HttpClient httpClient, string basicAuthenticationKey, string basicAuthenticationSecret) : base(baseUrl, httpClient, basicAuthenticationKey, basicAuthenticationSecret)
    {
        GraphEndpoint = graphEndpoint;
    }

    public async Task<string> SendGraphRequest(GraphQLRequestBody requestBody)
    {
        var request = SetEndpoint(GraphEndpoint);
        
        string requestBodyJson = JsonSerializer.Serialize(requestBody, _jsonOptions); 
        
        return await request.PostContentAsync(requestBodyJson);
    }

    public async Task<string> SendGraphRequest(ApiRequestBuilder request, GraphQLRequestBody requestBody)
    {
        string requestBodyJson = JsonSerializer.Serialize(requestBody, _jsonOptions);

        return await request.PostContentAsync(requestBodyJson);
    }
}
