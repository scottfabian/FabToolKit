using System.Net;

namespace FabToolKit.Api;

public class ApiExceptionBase : Exception
{
    public HttpStatusCode StatusCode { get; private set; }
    public string? Response { get; private set; }
    public string? RequestURI { get; private set; }


    public ApiExceptionBase() : base() { }

    public ApiExceptionBase(string message) : base(message) { }

    public ApiExceptionBase(string message, Exception ex) : base(message, ex) { }

    public ApiExceptionBase(HttpStatusCode statusCode, string response) : base() { this.StatusCode = statusCode; this.Response = response; }   

    public ApiExceptionBase(string message, HttpStatusCode statusCode, string response) : base(message) { this.StatusCode = statusCode; this.Response = response; }

    public ApiExceptionBase(string message, Exception ex, HttpStatusCode statusCode, string response) : base(message, ex) { this.StatusCode = statusCode; this.Response = response; }

    public ApiExceptionBase(HttpStatusCode statusCode, string response, string requestURL) : base() { this.StatusCode = statusCode; this.Response = response; this.RequestURI = requestURL; }

    //Should use this on all occasssions
    public ApiExceptionBase(string message, HttpStatusCode statusCode, string response, string requestURL) : base(message) { this.StatusCode = statusCode; this.Response = response; this.RequestURI = requestURL; }

    
}
