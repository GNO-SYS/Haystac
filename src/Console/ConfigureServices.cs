using Microsoft.Extensions.Configuration;

using Haystac.Console.Infrastructure.Http;
using Haystac.Console.Infrastructure.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddConsoleServices(this IServiceCollection services, IConfiguration configuration)
    {
        //< Add the transient services
        services.AddTransient<IJsonService, JsonService>();
        services.AddTransient<ITokenService, TokenService>();

        var stac_url = configuration["Haystac:ApiRoute"];
        if (stac_url == null) throw new Exception($"Unable to retrieve STAC API route - please configure it!");

        //< Add the IAuthenticationRepository w/ no HttpMessageHandler
        services.AddHttpClient<IAuthenticationRepository, AuthenticationRepository>(client => 
            client.BaseAddress = new Uri($@"{stac_url}/auth/"));

        //< Add the BearerTokenHandler HttpMessageHandler
        services.AddScoped<BearerTokenHandler>();

        //< Add Item & Collection repositories w/ BearerTokenHandler to attach available JWT
        services.AddHttpClient<ICollectionRepository, CollectionRepository>(client => 
            client.BaseAddress = new Uri($@"{stac_url}/collections/"))
                .AddHttpMessageHandler<BearerTokenHandler>();
        services.AddHttpClient<IItemRepository, ItemRepository>(client => 
            client.BaseAddress = new Uri($@"{stac_url}/collections/"))
                .AddHttpMessageHandler<BearerTokenHandler>();

        return services;
    }
}