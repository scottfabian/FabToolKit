using System.Net.Http.Headers;

namespace FabToolKit.Api;

public abstract class ApiAuthConfig
{
    internal abstract void Apply(HttpRequestHeaders headers);
}
