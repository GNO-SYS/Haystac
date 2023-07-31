namespace Haystac.Console.Application.Items;

public class ItemUpdateCommand : AsyncCommand<ItemUpdateCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<JSONFILE>")]
        [Description("Path to input STAC Collection JSON file to be parsed")]
        public string JsonFile { get; set; } = string.Empty;
    }

    private readonly IItemRepository _items;
    private readonly IJsonService _json;

    public ItemUpdateCommand(IItemRepository items, IJsonService json)
    {
        _items = items;
        _json = json;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        Helper.WriteDivider($"Updating existing STAC Item");

        Helper.Write($"Parsing Item from: [yellow]{Helper.GetEscapedFileName(settings.JsonFile)}[/]");

        var dto = await _json.ParseFromFile<ItemDto>(settings.JsonFile);

        Helper.Write($"Attempting to update STAC Item");
        Helper.Write($"Collection: [yellow]{dto.Collection.EscapeMarkup()}[/]");
        Helper.Write($"Item: [yellow]{dto.Identifier.EscapeMarkup()}[/]");

        await _items.UpdateItemAsync(dto);

        Helper.Write($"\t .. Done!");

        return 0;
    }
}
