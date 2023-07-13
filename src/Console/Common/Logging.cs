using Serilog;
using Serilog.Events;

namespace Haystac.Console.Common;

public static class Logging
{
    public static ILogger GetLogger()
    {
        return new LoggerConfiguration()
                    .MinimumLevel.Is(LogEventLevel.Information)
                    .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning)
                    .WriteTo.File("Haystac.Cli.Log-.txt", rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: LogEventLevel.Information)
                    .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Warning)
                    .CreateLogger();
    }
}
