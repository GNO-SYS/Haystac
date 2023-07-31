namespace Haystac.Console.Application.Items;

public class ItemAddCommand : AsyncCommand<ItemAddCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<JSONFILE>")]
        [Description("Path to input STAC Collection JSON file to be parsed")]
        public string JsonFile { get; set; } = string.Empty;
    }

    private readonly IItemRepository _items;
    private readonly IJsonService _json;

    public ItemAddCommand(IItemRepository items, IJsonService json)
    {
        _items = items;
        _json = json;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        Helper.WriteDivider($"Adding Item from JSON");

        Helper.Write($"Parsing Item from: [yellow]{Helper.GetEscapedFileName(settings.JsonFile)}[/]");

        var dto = await _json.ParseFromFile<ItemDto>(settings.JsonFile);

        Helper.Write($"Attempting to create Item: [yellow]{dto.Identifier.EscapeMarkup()}[/]");

        var id = await _items.CreateItemAsync(dto);

        Helper.Write($"\t - Created with UUID: [yellow]{id}[/]");

        return 0;
    }
}
