namespace Haystac.Console.Commands;

public class LogOutCommand : AsyncCommand<LogOutCommand.Settings>
{
    public class Settings : CommandSettings
    {
        //< TODO - Any settings?
    }

    private readonly IAuthenticationService _auth;

    public LogOutCommand(IAuthenticationService auth)
    {
        _auth = auth;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        Helper.Write($"Logging out..");

        await _auth.SignOutAsync();

        Helper.Write($"\t .. Done!");

        return 0;
    }
}