using Haystac.Application.Items.Commands;

namespace Haystac.Console.Commands;

public class ItemDeleteCommand : AsyncCommand<ItemDeleteCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<CollectionId>")]
        [Description("Name of the Collection to remove the item from")]
        public string CollectionId { get; set; } = string.Empty;

        [CommandArgument(1, "<ItemId>")]
        [Description("Name of the Item to be removed")]
        public string ItemId { get; set; } = string.Empty;
    }

    private readonly IMediator _mediator;

    public ItemDeleteCommand(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        Helper.WriteDivider($"Deleting Item from Collection");

        Helper.Write("Attempting to delete the following:");
        Helper.Write($"Collection: [yellow]{settings.CollectionId.EscapeMarkup()}[/]");
        Helper.Write($"Item: [yellow]{settings.ItemId.EscapeMarkup()}[/]");

        var cmd = new DeleteItemCommand
        {
            CollectionId = settings.CollectionId,
            Identifier = settings.ItemId,
        };
        await _mediator.Send(cmd);

        Helper.Write($"\t .. Done!");

        return 0;
    }
}
