using Microsoft.Extensions.Logging;

namespace CheckersBot.utils;

public class LoggerSingleton
{
    private static readonly ILoggerFactory LoggerFactory = Microsoft.Extensions.Logging.LoggerFactory.Create(builder =>
    {
        if (!bool.Parse(Environment.GetEnvironmentVariable("NO_LOGGER")!))
        {
            builder.AddConsole();
        }
    });
    public static ILogger<T> CreateLogger<T>() => LoggerFactory.CreateLogger<T>();
    public static readonly bool DetailedLogger = bool.Parse(Environment.GetEnvironmentVariable("DETAILED_LOGGER")!);
    public static readonly bool NoLogger = bool.Parse(Environment.GetEnvironmentVariable("NO_LOGGER")!);
}