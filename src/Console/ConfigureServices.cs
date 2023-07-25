using Haystac.Application.Common.Interfaces;

using Haystac.Console.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddConsoleServices(this IServiceCollection services)
    {
        services.AddScoped<IUser, CurrentUser>();

        services.AddTransient<IIdentityService, IdentityService>();
        services.AddTransient<IJsonService, JsonService>();

        return services;
    }
}