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
        var mobileNumber = string.Empty;
        // Check if the request method is POST (or the appropriate method for your case)
        if (context.Request.Method != HttpMethods.Post)
        {
            await _next(context);
        }
        else
        {
            //mobileNumber = await GetMobileNumberAsKey(context, mobileNumber);
            mobileNumber = await GetMobileNumberAsmodel(context, mobileNumber);
        }

        if (!IsBlocked(mobileNumber))
        {
            await _next(context);
        }
        else
        {
            context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
            await context.Response.WriteAsync("Rate Limit Exceeded");
        }

        //static async Task<string?> GetMobileNumberAsKey(HttpContext context, string? mobileNumber)
        //{
        //    // var originalBody = context.Request.Body;
        //    var requestBodyBuffer = new MemoryStream();
        //    await context.Request.Body.CopyToAsync(requestBodyBuffer);
        //    requestBodyBuffer.Seek(0, SeekOrigin.Begin);
        //    // Read the request body
        //    using (var reader = new StreamReader(requestBodyBuffer, Encoding.UTF8, true, 1024, true))
        //    {
        //        var requestBody = await reader.ReadToEndAsync();

        //        try
        //        {
        //            // Parse the JSON data without deserialization
        //            using (JsonDocument doc = JsonDocument.Parse(requestBody))
        //            {
        //                if (doc.RootElement.TryGetProperty("mobileNumber", out var mobileNumberProperty))
        //                {
        //                    mobileNumber = mobileNumberProperty.GetString();

        //                    if (!string.IsNullOrEmpty(mobileNumber))
        //                    {
        //                        // Do something with the mobileNumber value
        //                        // For example, you can log it or perform some processing
        //                    }
        //                }
        //            }
        //        }
        //        catch (System.Text.Json.JsonException)
        //        {
        //            // Handle JSON parsing errors
        //            // You may want to log the error or return a response indicating a bad request
        //        }
        //        finally
        //        {
        //            // Geri sardığınızdan emin olun
        //            //context.Request.Body = originalBody; 

        //            // Restore the original request body
        //            requestBodyBuffer.Seek(0, SeekOrigin.Begin);
        //            context.Request.Body = requestBodyBuffer;
        //        }
        //    }

        //    return mobileNumber;
        //}
        static async Task<string?> GetMobileNumberAsmodel(HttpContext context, string? mobileNumber)
        { 
            var requestBodyBuffer = new MemoryStream();
            await context.Request.Body.CopyToAsync(requestBodyBuffer);
            requestBodyBuffer.Seek(0, SeekOrigin.Begin);
            // Read the request body as a string
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
    private string ExtractMobileNumberFromJson(string requestBody)
    {
        // Implement your custom logic to extract the MobileNumber from the JSON object
        // For example, if the JSON structure is like: {"MobileNumber": "1234567890", ...}
        // You can use string manipulation or a JSON parser to extract it:
        // Example using string manipulation:

        const string mobileNumberKey = "\"mobileNumber\": \"";
        int startIndex = requestBody.IndexOf(mobileNumberKey);

        if (startIndex >= 0)
        {
            int endIndex = requestBody.IndexOf("\"", startIndex + mobileNumberKey.Length);
            if (endIndex >= 0)
            {
                string mobileNumber = requestBody.Substring(startIndex + mobileNumberKey.Length, endIndex - (startIndex + mobileNumberKey.Length));
                return mobileNumber;
            }
        }

        return null; // MobileNumber not found
    }
    private bool IsBlocked(string? clientId)
    {
        return _blockedNumbers.Numbers.Contains(clientId ?? string.Empty);
    }
}
