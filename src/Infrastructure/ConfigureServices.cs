using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

using Npgsql;

using Haystac.Infrastructure.Identity;
using Haystac.Infrastructure.Services;
using Haystac.Infrastructure.Persistence;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    private const string TestAuthProvider = "Test";
    private const string AwsCognitoAuthProvider = "Cognito";

    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IDateTimeService, DateTimeService>();

        //< TODO - Consider injecting an in-memory DB for testing
        var datasourceBuilder = new NpgsqlDataSourceBuilder(configuration.GetConnectionString("DefaultConnection"));
        datasourceBuilder.UseNetTopologySuite();
        var datasource = datasourceBuilder.Build();

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(datasource, o => o.UseNetTopologySuite()));

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<ApplicationDbContextInitializer>();

        //< Configure auth & identity
        string authProvider = configuration.GetValue<string>("AuthProvider") ?? TestAuthProvider;

        //< Consider wrapping these in functions, also they're quite similar
        if (authProvider == TestAuthProvider)
        {
            services.Configure<TestAuthSettings>(configuration.GetSection(TestAuthSettings.SectionName));
            services.AddSingleton<IAuthenticationService, TestAuthenticationService>();
            services.AddSingleton<IIdentityService, TestIdentityService>();

            var opt = configuration.Get<TestAuthSettings>() ?? new();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Authority = opt.Issuer;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    IssuerSigningKey = opt.SecurityKey,
                    ValidAudience = opt.Audience,
                    ValidIssuer = opt.Issuer,
                    ValidateLifetime = true,
                    LifetimeValidator = (before, expires, token, param) => expires > DateTime.UtcNow,
                    ValidateAudience = true
                };
            });
        }
        else if (authProvider == AwsCognitoAuthProvider)
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
        }

        return services;
    }
}
