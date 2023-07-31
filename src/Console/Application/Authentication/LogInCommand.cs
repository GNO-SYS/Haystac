namespace Haystac.Console.Application.Authentication;

public class LogInCommand : AsyncCommand<LogInCommand.Settings>
{
    public class Settings : CommandSettings
    {
        //< TODO - Any settings?
    }

    private readonly IAuthenticationRepository _auth;

    public LogInCommand(IAuthenticationRepository auth)
    {
        _auth = auth;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        Helper.WriteDivider("Login");

        var userName = PromptForUserName();
        var password = PromptForPassword();

        Helper.Write($"Attempting to login as: {userName}");
        var res = await _auth.LogInAsync(userName, password);
        if (!res.Succeeded)
        {
            //< TODO - Handle password reset/change/2FA
            Helper.Write($"FAILED: [yellow]{string.Join(Environment.NewLine, res.Errors)}[/]");
            return -1;
        }

        Helper.Write("\t ..Sucess!");
        return 0;
    }

    static string PromptForUserName()
    {
        //< TODO - Client-side validation on input user name prior to sending request
        return AnsiConsole.Ask<string>("Enter [green]username[/]:");
    }

    static string PromptForPassword()
    {
        return AnsiConsole.Prompt(
            new TextPrompt<string>("Enter [green]password[/]:")
                .PromptStyle("red")
                .Secret());
    }
}
