namespace Hatt.Middleware;

public class HttpResponseException(int statusCode, string message) : Exception(message)
{
    public int StatusCode { get; } = statusCode;
}
