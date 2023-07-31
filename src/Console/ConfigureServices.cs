using Microsoft.Extensions.Configuration;

using Haystac.Console.Infrastructure.Http;
using Haystac.Console.Infrastructure.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddConsoleServices(this IServiceCollection services, IConfiguration configuration)
    {
        //< TODO - Use Options pattern, properly construct base paths
        var stac_uri = new Uri(configuration["HaystacUrl"] ?? "");

        services.AddHttpClient<IAuthenticationRepository, AuthenticationRepository>(client => client.BaseAddress = stac_uri);
        services.AddHttpClient<ICollectionRepository, CollectionRepository>(client => client.BaseAddress = stac_uri)
                .AddHttpMessageHandler<BearerTokenHandler>();
        services.AddHttpClient<IItemRepository, ItemRepository>(client => client.BaseAddress = stac_uri)
                .AddHttpMessageHandler<BearerTokenHandler>();

        services.AddTransient<IJsonService, JsonService>();
        services.AddTransient<ITokenService, TokenService>();

        return services;
    }
}