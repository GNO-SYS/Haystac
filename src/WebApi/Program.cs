using Haystac.Application.Common.Middleware;
using Haystac.Infrastructure.Persistence;

Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
Log.Information($" -- Starting up -- ");

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((ctx, lc) => lc
           .WriteTo.Console()
           .ReadFrom.Configuration(ctx.Configuration));

    //< TODO - Configure HAYSTAC__ header-based ENV var extraction & binding
    builder.Configuration
        //.AddJsonFile("appsettings.json", optional: false)
        .AddEnvironmentVariables();

    builder.Services.AddApplicationServices();
    builder.Services.AddInfrastructureServices(builder.Configuration);
    builder.Services.AddApiServices();

    builder.Services.AddSwaggerGen();
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        using var scope = app.Services.CreateScope();
        var init = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitializer>();
        await init.InitializeAsync();
    }
    else
    {
        app.UseHsts();
    }

    app.UseSerilogRequestLogging();
    app.UseHealthChecks("/health");
    app.UseHttpsRedirection();

    app.UseMiddleware<ErrorHandlingMiddleware>();

    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception encountered!");
}
finally
{
    Log.Information(" -- Shut Down Complete --");
    Log.CloseAndFlush();
}

public partial class Program { }