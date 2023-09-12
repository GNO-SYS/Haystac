using Haystac.Application.Common.Middleware;
using Haystac.Infrastructure.Persistence;

Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
Log.Information($" -- Starting up -- ");


const string _corsPolicyName = "HaystacPolicy";

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((ctx, lc) => lc
           .WriteTo.Console()
           .ReadFrom.Configuration(ctx.Configuration));

    builder.Configuration
        //.AddJsonFile("appsettings.json", optional: false)
        .AddEnvironmentVariables();

    builder.Services.AddCors(o => o.AddPolicy(_corsPolicyName, builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    }));

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

    app.UseCors(_corsPolicyName);

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