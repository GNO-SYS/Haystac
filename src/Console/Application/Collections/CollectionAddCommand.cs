namespace Haystac.Console.Application.Collections;

public class CollectionAddCommand : AsyncCommand<CollectionAddCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<JSONFILE>")]
        [Description("Path to input STAC Collection JSON file to be parsed")]
        public string JsonFile { get; set; } = string.Empty;

        [CommandOption("-a|--anon")]
        [DefaultValue(false)]
        public bool Anonymous { get; set; }
    }

    private readonly ICollectionRepository _collections;
    private readonly IJsonService _json;

    public CollectionAddCommand(ICollectionRepository collections, IJsonService json)
    {
        _collections = collections;
        _json = json;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        Helper.WriteDivider($"Adding Collection from JSON");

        //< TODO - Add 'AuthenticationService' that caches a local JWT, and manages access to these commands
        Helper.Write($"Creating Collection from: [yellow]{Helper.GetEscapedFileName(settings.JsonFile)}[/]");

        var dto = await _json.ParseFromFile<CollectionDto>(settings.JsonFile);

        Helper.Write($"Attempting to create Collection: [yellow]{dto.Identifier.EscapeMarkup()}[/]");

        var id = await _collections.CreateCollectionAsync(dto, settings.Anonymous);

        Helper.Write($"\t - Created with UUID: [yellow]{id}[/]");

        return 0;
    }
}
