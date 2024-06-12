namespace SmartLoggerSampleApplication.Middleware;

using SmartLogger.Core;
using SmartLoggerSampleApplication.Exceptions;
using System.Net;
using System.Text.Json;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context,ILogAggregator logAggregator)
    {
        Severity severity = Severity.INFORMATION; 
        try
        {
            await _next(context);
        }
        catch (Exception error)
        {
            severity = Severity.DEBUG; 
            var response = context.Response;
            response.ContentType = "application/json";

            switch (error)
            {
                case BadRequestException e:
                    // custom application error
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                case KeyNotFoundException e:
                    // not found error
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                default:
                    // unhandled error
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            var result = JsonSerializer.Serialize(new { message = error?.Message });
            await response.WriteAsync(result);
        }
        finally
        {
            logAggregator.Flush(severity);

        }
    }
}