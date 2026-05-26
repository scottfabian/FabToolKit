using System.Net.Http.Headers;

namespace FabToolKit.Api;

public class BasicAuthConfig : ApiAuthConfig
{
    public string Key { get; }
    public string Secret { get; }

    public BasicAuthConfig(string key, string secret)
    {
        Key = key;
        Secret = secret;
    }

    internal override void Apply(HttpRequestHeaders headers)
    {
        var credentials = Convert.ToBase64String(
            System.Text.Encoding.UTF8.GetBytes($"{Key}:{Secret}"));

        headers.Authorization = new AuthenticationHeaderValue("Basic", credentials);
    }
}
