using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System;

namespace Fanex.COProfit.Service.Utilities;

using Logging;
using Logging.Extensions;
using Logging.Extensions.Database;

public class ExceptionLoggerMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            await HandleException(exception);
        }
    }

    private static async Task HandleException( Exception exception)
    {
        var logMessage = new LogMessage($"Exception: {exception.Message}")
            .AddSection("Database Info", DatabaseInfoGetters.GetDbExceptionInfo(exception))
            .AddSection("Server Info", CommonInfoGetters.GetServerInfo())
            .AddSection("Exception Info", CommonInfoGetters.GetExceptionInfo(exception))
            .ToString();
        await LogService.ErrorAsync(logMessage, exception);
    }
}