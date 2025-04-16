using FabToolKit.Api;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FabToolKit.Tkx.Sentinel.Rest;

public class SentinelApiClient : ApiServiceBase
{
    public SentinelApiClient(string baseUrl) : base(baseUrl)
    {
    }

    public SentinelApiClient(string baseUrl, HttpClient httpClient) : base(baseUrl, httpClient)
    {
    }

    public SentinelApiClient(string baseUrl, string basicAuthenticationKey, string basicAuthenticationSecret) : base(baseUrl, basicAuthenticationKey, basicAuthenticationSecret)
    {
    }

    public SentinelApiClient(string baseUrl, HttpClient httpClient, string basicAuthenticationKey, string basicAuthenticationSecret) : base(baseUrl, httpClient, basicAuthenticationKey, basicAuthenticationSecret)
    {
    }


    public async Task<string> PostJob(PostJobDTO[] jobDTO)
    {
        ApiRequestBuilder request = SetEndpoint(SentinelEndpoints.PostJob);

        string payload = JsonSerializer.Serialize(jobDTO, new JsonSerializerOptions { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull});

        return await request.PostContentAsync(payload);
    }
}
