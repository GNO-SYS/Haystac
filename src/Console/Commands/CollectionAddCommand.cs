using Haystac.Application.Collections.Commands;

namespace Haystac.Console.Commands;

public class CollectionAddCommand : AsyncCommand<CollectionAddCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<JSONFILE>")]
        [Description("Path to input STAC Collection JSON file to be parsed")]
        public string JsonFile { get; set; } = string.Empty;
    }

    private readonly IMediator _mediator;
    private readonly IJsonService _json;

    public CollectionAddCommand(IMediator mediator, IJsonService json)
    {
        _mediator = mediator;
        _json = json;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        Helper.WriteDivider($"Adding Collection from JSON");

        //< TODO - Add 'AuthenticationService' that caches a local JWT, and manages access to these commands
        Helper.Write($"Creating Collection from: [yellow]{Helper.GetEscapedFileName(settings.JsonFile)}[/]");

        var dto = await _json.ParseFromFile<CollectionDto>(settings.JsonFile);

        Helper.Write($"Attempting to create Collection: [yellow]{dto.Identifier.EscapeMarkup()}[/]");

        var id = await _mediator.Send(new CreateCollectionCommand { Dto = dto });

        Helper.Write($"\t - Created with UUID: [yellow]{id}[/]");

        return 0;
    }
}
