namespace Haystac.Console.Application.Items;

public class ItemGetCommand : AsyncCommand<ItemGetCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<CollectionId>")]
        [Description("Name of the Collection to remove the item from")]
        public string CollectionId { get; set; } = string.Empty;

        [CommandArgument(1, "<ItemId>")]
        [Description("Name of the Item to be removed")]
        public string ItemId { get; set; } = string.Empty;

        [CommandArgument(2, "<FilePath>")]
        [Description("Path of output file to write JSON to")]
        public string OutputFile { get; set; } = string.Empty;
    }

    private readonly IItemRepository _items;

    public ItemGetCommand(IItemRepository items)
    {
        _items = items;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        Helper.WriteDivider($"Fetching Item from Collection");

        if (!TryValidateInputs(settings, out var errors))
        {
            foreach (var error in errors) Helper.Write($"ERROR: {error}");

            return -1;
        }

        Helper.Write("Attempting to retrieve the following:");
        Helper.Write($"Collection: [yellow]{settings.CollectionId.EscapeMarkup()}[/]");
        Helper.Write($"Item: [yellow]{settings.ItemId.EscapeMarkup()}[/]");

        var dto = await _items.GetItemByIdAsync(settings.CollectionId, settings.ItemId);

        Helper.Write($"\t .. Done!");
        Helper.Write($"Saving to file: [yellow]{settings.OutputFile.EscapeMarkup()}[/]");

        var json = JsonSerializer.Serialize(dto);
        await File.WriteAllTextAsync(settings.OutputFile, json);

        Helper.Write($"\t .. Done!");

        return 0;
    }

    static bool TryValidateInputs(Settings settings, out List<string> errors)
    {
        errors = new List<string>();

        if (string.IsNullOrEmpty(settings.CollectionId)) errors.Add("A valid CollectionId is required");
        if (string.IsNullOrEmpty(settings.ItemId)) errors.Add("A valid ItemId is required");
        if (string.IsNullOrEmpty(settings.OutputFile)) errors.Add("A valid OutputFile path is required");

        return errors.Count == 0;
    }
}
