using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.ExceptionServices;

namespace Infrastructure.Performance;

public class PerformanceProxy<T> : DispatchProxy
{
    private T _decorated = default!;
    private ILogger _logger = default!;

    // DispatchProxy requires a public parameterless constructor.
    public PerformanceProxy()
    {
    }

    public static T Create(
        T decorated,
        ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(decorated);
        ArgumentNullException.ThrowIfNull(logger);

        var proxy =
            DispatchProxy.Create<T, PerformanceProxy<T>>();

        var performanceProxy =
            (PerformanceProxy<T>)(object)proxy;

        performanceProxy._decorated = decorated;
        performanceProxy._logger = logger;

        return proxy;
    }

    protected override object? Invoke(
        MethodInfo? targetMethod,
        object?[]? args)
    {
        if (targetMethod is null)
            throw new ArgumentNullException(nameof(targetMethod));

        var stopwatch = Stopwatch.StartNew();

        try
        {
            var result = targetMethod.Invoke(
                _decorated,
                args);

            return HandleResult(
                result,
                targetMethod,
                stopwatch);
        }
        catch (TargetInvocationException ex)
        {
            stopwatch.Stop();

            var exception =
                ex.InnerException ?? ex;

            LogPerformance(
                targetMethod,
                stopwatch,
                exception);

            ExceptionDispatchInfo
                .Capture(exception)
                .Throw();

            throw;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();

            LogPerformance(
                targetMethod,
                stopwatch,
                ex);

            throw;
        }
    }

    private object? HandleResult(
        object? result,
        MethodInfo method,
        Stopwatch stopwatch)
    {
        // -----------------------------------------
        // Method returned null
        // -----------------------------------------

        if (result is null)
        {
            stopwatch.Stop();

            LogPerformance(
                method,
                stopwatch,
                null);

            return null;
        }

        // -----------------------------------------
        // Task / Task<T>
        // -----------------------------------------

        if (result is Task task)
        {
            // Task<T>
            if (method.ReturnType.IsGenericType &&
                method.ReturnType.GetGenericTypeDefinition()
                    == typeof(Task<>))
            {
                return HandleGenericTask(
                    task,
                    method,
                    stopwatch);
            }

            // Task
            return HandleTask(
                task,
                method,
                stopwatch);
        }

        // -----------------------------------------
        // Synchronous method
        // -----------------------------------------

        stopwatch.Stop();

        LogPerformance(
            method,
            stopwatch,
            null);

        return result;
    }

    private object HandleGenericTask(
        Task task,
        MethodInfo method,
        Stopwatch stopwatch)
    {
        var resultType =
            method.ReturnType.GetGenericArguments()[0];

        var wrapperMethod =
            typeof(PerformanceProxy<T>)
                .GetMethod(
                    nameof(HandleGenericTaskInternal),
                    BindingFlags.Instance |
                    BindingFlags.NonPublic);

        if (wrapperMethod is null)
        {
            throw new InvalidOperationException(
                $"Could not find {nameof(HandleGenericTaskInternal)}.");
        }

        var genericMethod =
            wrapperMethod.MakeGenericMethod(resultType);

        return genericMethod.Invoke(
            this,
            new object[]
            {
                task,
                method,
                stopwatch
            })!;
    }

    private async Task<TResult> HandleGenericTaskInternal<TResult>(
        Task<TResult> task,
        MethodInfo method,
        Stopwatch stopwatch)
    {
        try
        {
            var result = await task;

            stopwatch.Stop();

            LogPerformance(
                method,
                stopwatch,
                null);

            return result;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();

            LogPerformance(
                method,
                stopwatch,
                ex);

            throw;
        }
    }

    private async Task HandleTask(
        Task task,
        MethodInfo method,
        Stopwatch stopwatch)
    {
        try
        {
            await task;

            stopwatch.Stop();

            LogPerformance(
                method,
                stopwatch,
                null);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();

            LogPerformance(
                method,
                stopwatch,
                ex);

            throw;
        }
    }

    private void LogPerformance(
        MethodInfo method,
        Stopwatch stopwatch,
        Exception? exception)
    {
        var duration =
            stopwatch.ElapsedMilliseconds;

        var level = duration switch
        {
            >= 5000 => LogLevel.Critical,
            >= 2000 => LogLevel.Warning,
            >= 1000 => LogLevel.Information,
            _ => LogLevel.Debug
        };

        if (exception is not null)
        {
            _logger.Log(
                level,
                exception,
                "METHOD ERROR | Level={level} | Service={Service} | Method={Method} | Duration={Duration}ms",
                level,typeof(T).Name,
                method.Name,
                duration);

            return;
        }

        _logger.Log(
            level,
            "METHOD PERFORMANCE | Level={level} | Service={Service} | Method={Method} | Duration={Duration}ms",
            level,typeof(T).Name,
            method.Name,
            duration);
    }
}