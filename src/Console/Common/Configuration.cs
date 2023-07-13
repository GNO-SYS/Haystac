using System.Diagnostics;
using System.Reflection;

using Microsoft.Extensions.Configuration;

namespace Haystac.Console.Common;

public static class Configuration
{
    public static string GetVersion()
    {
        var assemblyLocation = Assembly.GetExecutingAssembly().Location;
        var productVersion = FileVersionInfo.GetVersionInfo(assemblyLocation).ProductVersion;

        return productVersion ?? "Unknown";
    }

    public static IConfiguration GetConfiguration()
    {
        return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .AddEnvironmentVariables()
            .Build();
    }
}
