namespace Haystac.Console.Application.Collections;

public class CollectionDeleteCommand : AsyncCommand<CollectionDeleteCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<CollectionId>")]
        [Description("Name of the Collection to remove the item from")]
        public string CollectionId { get; set; } = string.Empty;
    }

    private readonly ICollectionRepository _collections;

    public CollectionDeleteCommand(ICollectionRepository collections)
    {
        _collections = collections;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        Helper.WriteDivider($"Deleting Collection");

        Helper.Write("Attempting to delete the following:");
        Helper.Write($"Collection: [yellow]{settings.CollectionId.EscapeMarkup()}[/]");

        await _collections.DeleteCollectionAsync(settings.CollectionId);

        Helper.Write($"\t .. Done!");

        return 0;
    }
}
