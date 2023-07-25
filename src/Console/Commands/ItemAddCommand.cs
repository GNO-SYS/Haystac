using Haystac.Application.Items.Commands;

namespace Haystac.Console.Commands;

public class ItemAddCommand : AsyncCommand<ItemAddCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<JSONFILE>")]
        [Description("Path to input STAC Collection JSON file to be parsed")]
        public string JsonFile { get; set; } = string.Empty;
    }

    private readonly IMediator _mediator;
    private readonly IJsonService _json;

    public ItemAddCommand(IMediator mediator, IJsonService json)
    {
        _mediator = mediator;
        _json = json;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        Helper.WriteDivider($"Adding Item from JSON");

        Helper.Write($"Creating Item from: [yellow]{Helper.GetEscapedFileName(settings.JsonFile)}[/]");

        var dto = await _json.ParseFromFile<ItemDto>(settings.JsonFile);

        Helper.Write($"Attempting to create Item: [yellow]{dto.Identifier.EscapeMarkup()}[/]");

        var id = await _mediator.Send(new CreateItemCommand { Dto = dto });

        Helper.Write($"\t - Created with UUID: [yellow]{id}[/]");

        return 0;
    }
}
