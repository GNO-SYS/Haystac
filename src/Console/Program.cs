using Microsoft.Extensions.Hosting;
using Serilog;

using Haystac.Console.Commands;
using Haystac.Console.Common;

AnsiConsole.Write(new FigletText("Haystac"));
AnsiConsole.WriteLine($"Haystac Command-line Interface {Configuration.GetVersion()}");
AnsiConsole.WriteLine();

var builder = Host.CreateDefaultBuilder(args);
var config = Configuration.GetConfiguration();

builder.UseSerilog(Logging.GetLogger());
builder.ConfigureServices(services =>
{
    services.AddApplicationServices();
    services.AddInfrastructureServices(config);
    services.AddConsoleServices();
});

var registrar = new TypeRegistrar(builder);
var app = new CommandApp(registrar);

app.Configure(config =>
{
    config.SetApplicationName("Haystac CLI");

    config.AddBranch("collection", branch =>
    {
        branch.AddCommand<CollectionAddCommand>("add")
              .WithDescription("Attempts to import a STAC Collection from an input JSON file")
              .WithExample(new[] { "add", @"C:\_test\stac_collection.json" });

        branch.AddCommand<CollectionListCommand>("list")
              .WithDescription("Lists detailed information about each Collection currently stored in the DB");
    });

    config.AddBranch("db", branch =>
    {
        branch.AddCommand<DatabaseInitializeCommand>("init")
              .WithDescription("Attempts to initialize the database to current schema");
    });

    config.AddBranch("item", branch =>
    {
        branch.AddCommand<ItemAddCommand>("add")
              .WithDescription("Attempts to import a STAC Item into an existing STAC Collection from an input JSON file")
              .WithExample(new[] { "add", @"C:\_test\stac_item.json" });
    });

});

return app.Run(args);