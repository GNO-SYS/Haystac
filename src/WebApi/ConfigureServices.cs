﻿using Haystac.Application.Common.Interfaces;

using Haystac.Infrastructure.Persistence;

using Haystac.WebApi.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddCors(o => o.AddPolicy("HaystacPolicy", builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        }));

        services.AddScoped<IUser, CurrentUser>();
        services.AddScoped<IUrlService, UrlService>();
        services.AddHttpContextAccessor();

        services.AddHealthChecks()
                .AddDbContextCheck<ApplicationDbContext>();

        return services;
    }
}