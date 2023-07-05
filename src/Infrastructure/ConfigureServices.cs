using Microsoft.Extensions.Configuration;

using Haystac.Application.Common.Interfaces;
using Haystac.Infrastructure.Persistence;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        //< TODO - Add transient services

        //< TODO - Add actual NPGSQL DB connection & configurration

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        //< TODO - Add Identity & Auth

        return services;
    }
}
