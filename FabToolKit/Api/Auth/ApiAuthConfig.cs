using System.Net.Http.Headers;

namespace FabToolKit.Api;

public abstract class ApiAuthConfig
{
    // Called once in the ApiServiceBase ctor for strategies with static credentials.
    internal abstract void Apply(HttpRequestHeaders headers);

    // Called before every request. Override for strategies that require async work
    // (e.g. OAuth2 token fetch/refresh). Default is a no-op.
    internal virtual Task PrepareAsync(HttpRequestHeaders headers) => Task.CompletedTask;
}
