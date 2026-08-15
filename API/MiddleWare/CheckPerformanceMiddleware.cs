using Application.Features.LoginFeature.Interfaces;
using Domain.Entities;
using System.Diagnostics;

namespace API.MiddleWare;

public class CheckPerformanceMiddleware
{
    private readonly RequestDelegate requestDelegate;
    private readonly ILogger<CheckPerformanceMiddleware> logger;
    public CheckPerformanceMiddleware(RequestDelegate requestDelegate, ILogger<CheckPerformanceMiddleware> logger)
    {
        this.requestDelegate = requestDelegate;
        this.logger = logger;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        var startTime = DateTimeOffset.UtcNow;

        var method = context.Request.Method;
        var path = context.Request.Path.Value ?? "/";
        Exception? exception = null;

        try
        {
            await requestDelegate(context);
        }
        catch (Exception ex)
        {
            exception = ex;
            throw;
        }
        finally
        {
            
            stopwatch.Stop();

            var endTime = DateTimeOffset.UtcNow;
            var duration = stopwatch.ElapsedMilliseconds;

            var level = duration switch
            {
                >= 5000 => "CRITICAL",
                >= 2000 => "SLOW",
                >= 1000 => "WARNING",
                _ => "OK"
            };

            string message = "";
            if (exception != null)
            {
                message = String.Format("API ERROR | Level={0} | Method={1} | Path={2} | StatusCode={3} | Start={4:yyyy-MM-dd HH:mm:ss.fff} | End={5:yyyy-MM-dd HH:mm:ss.fff} | Duration={6}ms",
                    level,
                    method,
                    path,
                    context.Response.StatusCode,
                    startTime,
                    endTime,
                    duration);
                logger.Log(LogLevel.Information, exception, message);

            }
            else
            {
                message = String.Format("API REQUEST | Level={0} | Method={1} | Path={2} | StatusCode={3} | Start={4:yyyy-MM-dd HH:mm:ss.fff} | End={5:yyyy-MM-dd HH:mm:ss.fff} | Duration={6}ms",
                    level,
                    method,
                    path,
                    context.Response.StatusCode,
                    startTime,
                    endTime,
                    duration);
                logger.Log(LogLevel.Information, message);
            }

        }
    }

}
