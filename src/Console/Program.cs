using Microsoft.Extensions.Hosting;

using Serilog;

using Haystac.Console.Common;
using Haystac.Console.Application.Authentication;
using Haystac.Console.Application.Items;
using Haystac.Console.Application.Collections;

AnsiConsole.Write(new FigletText("Haystac"));
AnsiConsole.WriteLine($"Haystac Command-line Interface {Configuration.GetVersion()}");
AnsiConsole.WriteLine();

var builder = Host.CreateDefaultBuilder(args);
var config = Configuration.GetConfiguration();

builder.UseSerilog(Logging.GetLogger());
builder.ConfigureServices(services =>
{
    services.AddDataProtection();
    services.AddConsoleServices(config);
});

var registrar = new TypeRegistrar(builder);
var app = new CommandApp(registrar);

app.Configure(config =>
{
    config.SetApplicationName("haystac");

    config.SetExceptionHandler(ex =>
    {
        AnsiConsole.WriteException(ex);
        return -99;
    });

    config.AddCommand<LogInCommand>("login")
          .WithDescription("Attempts to authenticate & cache credentials for remote Haystac API");

    config.AddCommand<LogOutCommand>("logout")
          .WithDescription("Clears any local credentials for remote Haystac API");

    config.AddBranch("collection", branch =>
    {
        branch.AddCommand<CollectionAddCommand>("add")
              .WithDescription("Attempts to import a STAC Collection from an input JSON file")
              .WithExample(new[] { "collection", "add", @"C:\_test\stac_collection.json" });

        branch.AddCommand<CollectionListCommand>("list")
              .WithDescription("Lists detailed information about each Collection currently stored in the DB");

        branch.AddCommand<CollectionGetCommand>("get")
              .WithDescription("Retrieves a specific Collection and writes it to local JSON file.")
              .WithExample(new[] { "collection", "get", "CollectionName", @"C:\_test\collection_name.json" });

        branch.AddCommand<CollectionUpdateCommand>("update")
              .WithDescription("Attempts to update existing STAC Collection with data parsed from local JSON.")
              .WithExample(new[] { "collection", "update", @"C:\_test\stac_collection.json" });

        branch.AddCommand<CollectionDeleteCommand>("delete")
              .WithDescription("Retrieves a specific Collection and writes it to loca JSON file.")
              .WithExample(new[] { "collection", "delete", "CollectionName" });
    });

    config.AddBranch("item", branch =>
    {
        branch.AddCommand<ItemAddCommand>("add")
              .WithDescription("Attempts to import a STAC Item into an existing STAC Collection from an input JSON file")
              .WithExample(new[] { "item", "add", @"C:\_test\stac_item.json" });

        branch.AddCommand<ItemGetCommand>("get")
              .WithDescription("Retrieves a specific Item and writes it to local JSON file.")
              .WithExample(new[] { "item", "get", "CollectionName", "ItemName", @"C:\_test\item_name.json" });

        branch.AddCommand<ItemUpdateCommand>("update")
              .WithDescription("Retrieves a specific Item and writes it to local JSON file.")
              .WithExample(new[] { "item", "update", @"C:\_test\stac_item.json" });

        branch.AddCommand<ItemDeleteCommand>("delete")
              .WithDescription("Attempts to delete the given Item from the given Collection")
              .WithExample(new[] { "item", "delete", "CollectionName", "ItemName" });
    });

});

return app.Run(args);