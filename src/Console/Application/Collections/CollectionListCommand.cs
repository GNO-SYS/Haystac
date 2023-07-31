namespace Haystac.Console.Application.Collections;

public class CollectionListCommand : AsyncCommand<CollectionListCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "[count]")]
        [Description("[UNUSUED] The total number of Client entities to list - defaults to 10")]
        public int Count { get; set; } = 10;
    }

    private readonly ICollectionRepository _collections;

    public CollectionListCommand(ICollectionRepository collections)
    {
        _collections = collections;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        Helper.WriteDivider($"Listing Collections");

        Helper.Write("Fetching collections..");
        var collecs = await _collections.GetAllCollectionsAsync();
        Helper.Write($" - Done! Found [yellow]{collecs.Count}[/] Collections");

        var table = new Table();
        table.AddColumn("Name");
        table.AddColumn("Type");
        table.AddColumn("Description");

        foreach (var collec in collecs)
        {
            table.AddRow(collec.Identifier, $"[yellow]{collec.Type}[/]", collec.Description);
        }

        table.Expand();
        AnsiConsole.Write(table);

        return 0;
    }
}
