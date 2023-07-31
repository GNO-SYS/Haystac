namespace Haystac.Console.Application.Authentication;

public class LogOutCommand : AsyncCommand<LogOutCommand.Settings>
{
    public class Settings : CommandSettings
    {
        //< TODO - Any settings?
    }

    private readonly IAuthenticationRepository _auth;

    public LogOutCommand(IAuthenticationRepository auth)
    {
        _auth = auth;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        Helper.Write($"Logging out..");

        await _auth.LogOutAsync();

        Helper.Write($"\t .. Done!");

        return 0;
    }
}