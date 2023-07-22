using Microsoft.Extensions.Configuration;
using Npgsql;

using Haystac.Application.Common.Interfaces;

using Haystac.Infrastructure.Services;
using Haystac.Infrastructure.Persistence;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        //< TODO - Add transient services
        services.AddTransient<IDateTimeService, DateTimeService>();

        //< TODO - Consider injecting an in-memory DB for testing

        var datasourceBuilder = new NpgsqlDataSourceBuilder(configuration.GetConnectionString("DefaultConnection"));
        datasourceBuilder.UseNetTopologySuite();
        var datasource = datasourceBuilder.Build();

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(datasource, o => o.UseNetTopologySuite()));

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<ApplicationDbContextInitializer>();

        //< TODO - Add Identity & Auth

        return services;
    }
}
