using Microsoft.Extensions.Configuration;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    private const string APIKEY_HEADER_NAME = "X-API-KEY";
    private string _key;
    public ApiKeyMiddleware(RequestDelegate next, IOptions<Entities.Configuration.SecurityKey> key)
    {
        _next = next;
        _key = key.Value.ApiKey.ToString();
    }

    public async Task InvokeAsync(HttpContext context, IConfiguration configuration)
    {
        if (!context.Request.Headers.TryGetValue(APIKEY_HEADER_NAME, out var extractedApiKey))
        {
            context.Response.StatusCode = 401; // Unauthorized
            await context.Response.WriteAsync("API Key gerekli.");
            return;
        }

        if (!_key.Equals(extractedApiKey))
        {
            context.Response.StatusCode = 403; // Forbidden
            await context.Response.WriteAsync("Geçersiz API Key.");
            return;
        }

        await _next(context);
    }
}
