using FabToolKit.Api;

namespace ConsoleTester;

internal class Program
{
    static async Task Main(string[] args)
    {
        string vendorKey = "RxwslAgoPK4YinPORltFssWHCXVnPGdo9JtP0K0BlwiZ52bP";
        string userKey = "wxJMHOyXOyHsNC4QGEadNxBmbJo1ba5JFgfkbExN3ip7tYwz";
        string baseUrl = "https://api-mi.metrc.com";
        string facilityLicense = "AU-G-EX-000001";
        var httpClient = new HttpClient();

        string packageID = "1A4FF0300000515000000039";

        var client = new ApiServiceBase(baseUrl, httpClient, vendorKey, userKey);

        var request = client.SetEndpoint("/items/v2/{id}")
                                .InjectQueryParameter("id", "91201")
                                .AddQueryParameter("licenseNumber", "AU-G-EX-000001");

        string fullUri = request.GetFullURI();

        var response = request.GetAsync().GetAwaiter().GetResult();

        //var response = await client.SetEndpoint("/packages/v2/{id}")
        //                        .InjectQueryParameter("id", packageID)
        //                        .AddQueryParameter("licenseNumber", facilityLicense)
        //                        .GetAsync();
        //string apiKey = "2f6ddb4f3f2408db0a75bdfb0ec58b45";
        //double lat = 42.963292;
        //double lng = -87.993814;
        //string baseUrl = "https://api.openweathermap.org/data/2.5";
        //string endpoint = "/weather";

        //var apiService = new ApiServiceBase(baseUrl);

        //string response = await apiService.SetEndpoint(endpoint)
        //                .AddQueryParameter("lat", lat.ToString())
        //                .AddQueryParameter("lon", lng.ToString())
        //                .AddQueryParameter("appid", apiKey)
        //                .GetContentAsync();

        //Console.WriteLine(response);
    }
}
