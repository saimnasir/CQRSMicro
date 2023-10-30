using CQRSMicro.Domain;
using CQRSMicro.Product.CQRS.Commands.Request;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Patika.Framework.Shared.Entities;
using Patika.Framework.Shared.Services;
using System.Net;
using System.Text;
using System.Text.Json;

public class OTPRateLimitMiddleware : CoreService
{
    private readonly RequestDelegate _next;
    private readonly BlockedNumbersConfig _blockedNumbers;

    public OTPRateLimitMiddleware(IServiceProvider serviceProvider,
        RequestDelegate next,
        IOptions<Configuration> Configuration) : base(serviceProvider)
    {
        _next = next;
        //_blockedNumbers = blockedNumbers.Value;
        _blockedNumbers = GetService<BlockedNumbersConfig>();
    }

    public async Task Invoke(HttpContext context)
    {
        if (await IsBlockedAsync(context))
        {
            context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
            await context.Response.WriteAsync("Rate Limit Exceeded");
        }
        await _next(context);

    }

    private async Task<bool> IsBlockedAsync(HttpContext context)
    {
        if (context.Request.Method == HttpMethods.Post)
        {
            var mobileNumber = await GetMobileNumberAsync(context);
            return _blockedNumbers.Numbers.Contains(mobileNumber ?? string.Empty);
        }
        return false;
    }

    private static async Task<string?> GetMobileNumberAsync(HttpContext context)
    {
        string mobileNumber = string.Empty;
        var requestBodyBuffer = new MemoryStream();
        await context.Request.Body.CopyToAsync(requestBodyBuffer);
        requestBodyBuffer.Seek(0, SeekOrigin.Begin);

        using StreamReader reader = new(requestBodyBuffer, Encoding.UTF8, true, 1024, true);
        string requestBody = await reader.ReadToEndAsync();
        try
        {
            var data = JsonConvert.DeserializeObject<SendOTPCommandRequest>(requestBody);
            mobileNumber = data?.MobileNumber ?? string.Empty;
        }
        catch (System.Text.Json.JsonException)
        {
            context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
            await context.Response.WriteAsync("Rate Limit Exceeded");
        }
        finally
        {
            // Restore the original request body
            requestBodyBuffer.Seek(0, SeekOrigin.Begin);
            context.Request.Body = requestBodyBuffer;
        }
        return mobileNumber;
    }
}
