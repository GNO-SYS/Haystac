namespace Haystac.Console.Commands;

public class LogOutCommand : AsyncCommand<LogOutCommand.Settings>
{
    public class Settings : CommandSettings
    {
        //< TODO - Any settings?
    }

    private readonly IHaystacService _haystac;

    public LogOutCommand(IHaystacService haystac)
    {
        _haystac = haystac;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        Helper.Write($"Logging out..");

        await _haystac.LogOutAsync();

        Helper.Write($"\t .. Done!");

        return 0;
    }
}