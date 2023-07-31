namespace Haystac.Console.Application.Collections;

public class CollectionUpdateCommand : AsyncCommand<CollectionUpdateCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<JSONFILE>")]
        [Description("Path to input STAC Collection JSON file to be parsed")]
        public string JsonFile { get; set; } = string.Empty;
    }

    private readonly ICollectionRepository _collections;
    private readonly IJsonService _json;

    public CollectionUpdateCommand(ICollectionRepository collections, IJsonService json)
    {
        _collections = collections;
        _json = json;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        Helper.WriteDivider($"Updating existing STAC Collection");

        Helper.Write($"Parsing Collection from: [yellow]{Helper.GetEscapedFileName(settings.JsonFile)}[/]");

        var dto = await _json.ParseFromFile<CollectionDto>(settings.JsonFile);

        Helper.Write($"Attempting to update Collection: [yellow]{dto.Identifier.EscapeMarkup()}[/]");

        await _collections.UpdateCollectionAsync(dto);

        Helper.Write($"\t .. Done!");

        return 0;
    }
}
