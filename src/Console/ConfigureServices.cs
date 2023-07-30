using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddConsoleServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient<IHaystacService, HaystacService>(client =>
        {
            client.BaseAddress = new Uri(configuration["HaystacUrl"] ?? "");
        });

        services.AddTransient<IJsonService, JsonService>();
        services.AddTransient<ITokenService, TokenService>();

        return services;
    }
}