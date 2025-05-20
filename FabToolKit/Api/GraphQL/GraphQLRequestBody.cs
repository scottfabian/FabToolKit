using System.Text.Json.Serialization;

namespace FabToolKit.Api.GraphQL;

public class GraphQLRequestBody
{
    [JsonPropertyName("query")]
    public string Query { get; set; }

    [JsonPropertyName("variables")]
    public GraphQLRequestVariables? Variables { get; set; }

    [JsonPropertyName("operationName")]
    public string OperationName { get; set; }

    public GraphQLRequestBody(string operationName, string query)
    {
        OperationName = operationName;
        Query = query;
    }

    public GraphQLRequestBody(string operationName, string query, GraphQLRequestVariables variables) : this(operationName, query)
    {
        Variables = variables;
    }
}
