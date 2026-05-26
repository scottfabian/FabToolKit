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

    public GraphQLServiceBase(string baseUrl, string graphEndpoint, ApiAuthConfig? auth = null)
        : base(baseUrl, auth)
    {
        GraphEndpoint = graphEndpoint;
    }

    public GraphQLServiceBase(string baseUrl, string graphEndpoint, HttpClient httpClient, ApiAuthConfig? auth = null)
        : base(baseUrl, httpClient, auth)
    {
        GraphEndpoint = graphEndpoint;
    }

    public async Task<HttpResponseMessage> SendGraphRequest(GraphQLRequestBody requestBody)
    {
        var request = SetEndpoint(GraphEndpoint);

        string requestBodyJson = JsonSerializer.Serialize(requestBody, _jsonOptions);

        return await request.PostAsync(requestBodyJson, ApiContentType.Json);
    }

    public async Task<HttpResponseMessage> SendGraphRequest(ApiRequestBuilder request, GraphQLRequestBody requestBody)
    {
        string requestBodyJson = JsonSerializer.Serialize(requestBody, _jsonOptions);

        return await request.PostAsync(requestBodyJson, ApiContentType.Json);
    }
}
