using Microsoft.Extensions.Hosting;
using Serilog;

using Haystac.Console.Commands;
using Haystac.Console.Common;

var builder = Host.CreateDefaultBuilder(args);
var config = Configuration.GetConfiguration();

builder.UseSerilog(Logging.GetLogger());
builder.ConfigureServices(services =>
{
    services.AddApplicationServices();
    services.AddInfrastructureServices(config);
    services.AddConsoleServices();
});

AnsiConsole.Write(new FigletText("Haystac"));
AnsiConsole.WriteLine($"Haystac Command-line Interface {Configuration.GetVersion()}");
AnsiConsole.WriteLine();

var registrar = new TypeRegistrar(builder);
var app = new CommandApp(registrar);

app.Configure(config =>
{
    config.SetApplicationName("Haystac CLI");

    config.AddBranch("db", branch =>
    {
        branch.AddCommand<DatabaseInitializeCommand>("init")
              .WithDescription("Attempts to initialize the database to current schema");
    });
});

return app.Run(args);