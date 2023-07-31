namespace Haystac.Console.Application.Collections;

public class CollectionGetCommand : AsyncCommand<CollectionGetCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<CollectionId>")]
        [Description("Name of the Collection to remove the item from")]
        public string CollectionId { get; set; } = string.Empty;

        [CommandArgument(1, "<FilePath>")]
        [Description("Path of output file to write JSON to")]
        public string OutputFile { get; set; } = string.Empty;
    }

    private readonly ICollectionRepository _collections;

    public CollectionGetCommand(ICollectionRepository collections)
    {
        _collections = collections;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        Helper.WriteDivider($"Fetching Collection");

        //< TODO - Any parameter validation?

        Helper.Write("Attempting to retrieve the following:");
        Helper.Write($"Collection: [yellow]{settings.CollectionId.EscapeMarkup()}[/]");

        var dto = await _collections.GetCollectionByIdAsync(settings.CollectionId);

        Helper.Write($"\t .. Done!");
        Helper.Write($"Saving to file: [yellow]{settings.OutputFile.EscapeMarkup()}[/]");

        var json = JsonSerializer.Serialize(dto);
        await File.WriteAllTextAsync(settings.OutputFile, json);

        Helper.Write($"\t .. Done!");

        return 0;
    }
}
