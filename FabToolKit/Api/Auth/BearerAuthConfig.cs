using System.Net.Http.Headers;

namespace FabToolKit.Api;

public class BearerAuthConfig : ApiAuthConfig
{
    public string Token { get; private set; }

    public BearerAuthConfig(string token)
    {
        Token = token;
    }

    public void UpdateToken(string newToken)
    {
        Token = newToken;
    }

    internal override void Apply(HttpRequestHeaders headers)
    {
        if (!string.IsNullOrWhiteSpace(Token))
            headers.Authorization = new AuthenticationHeaderValue("Bearer", Token);
    }
}
