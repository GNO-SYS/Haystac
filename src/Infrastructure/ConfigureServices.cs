using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

using Npgsql;

using Haystac.Infrastructure.Identity;
using Haystac.Infrastructure.Services;
using Haystac.Infrastructure.Persistence;
using Haystac.Infrastructure.Authentication;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    private const string TestAuthProvider = "Test";
    private const string AwsCognitoAuthProvider = "Cognito";

    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IClientService, ClientService>();
        services.AddTransient<IConformanceService, ConformanceService>();
        services.AddTransient<IDateTimeService, DateTimeService>();
        services.AddTransient<ILinkService, LinkService>();

        services.Configure<RootCatalogOptions>(configuration.GetSection(RootCatalogOptions.RootCatalog));
        services.AddSingleton<IRootCatalogService, RootCatalogService>();

        //< TODO - Consider injecting an in-memory DB for testing
        var datasourceBuilder = new NpgsqlDataSourceBuilder(configuration.GetConnectionString("DefaultConnection"));
        datasourceBuilder.UseNetTopologySuite();
        var datasource = datasourceBuilder.Build();

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(datasource, o => o.UseNetTopologySuite()));

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<ApplicationDbContextInitializer>();

        //< Configure auth & identity
        var authProvider = configuration.GetValue<string>("AuthProvider");
        if (authProvider == null) throw new Exception($"'AuthProvider' is not configured.");

        if (authProvider == TestAuthProvider)
        {
            services.AddTestAuth(configuration);
        }
        else if (authProvider == AwsCognitoAuthProvider)
        {
            services.AddCognitoAuth(configuration);
        }

        return services;
    }

    static IServiceCollection AddTestAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IAuthenticationService, TestAuthenticationService>();
        services.AddSingleton<IIdentityService, TestIdentityService>();
        services.AddSingleton<IUserCollection<TestUser>, TestUserCollection>();

        var opt = configuration.GetSection(TestAuthSettings.SectionName).Get<TestAuthSettings>();
        if (opt == null) throw new Exception($"'{TestAuthSettings.SectionName}' is not configured");

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            //options.Authority = opt.Issuer;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                IssuerSigningKey = SecurityKeyHelper.GetSecurityKey(opt.Secret),
                ValidAudience = opt.Audience,
                ValidIssuer = opt.Issuer,
                ValidateLifetime = true,
                LifetimeValidator = (before, expires, token, param) => expires > DateTime.UtcNow,
                ValidateAudience = true
            };
        });

        return services;
    }

    static IServiceCollection AddCognitoAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCognitoIdentity();
        services.AddTransient<IAuthenticationService, CognitoAuthenticationService>();
        services.AddTransient<IIdentityService, CognitoIdentityService>();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.Authority = configuration["Cognito:Authority"];
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidIssuer = configuration["Cognito:Authority"],
                ValidateLifetime = true,
                LifetimeValidator = (before, expires, token, param) => expires > DateTime.UtcNow,
                ValidateAudience = false
            };
        });

        return services;
    }
}
