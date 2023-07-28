namespace Haystac.Console.Commands;

public class LogInCommand : AsyncCommand<LogInCommand.Settings>
{
    public class Settings : CommandSettings
    {
        //< TODO - Any settings?
    }

    private readonly IAuthenticationService _auth;

    public LogInCommand(IAuthenticationService auth)
    {
        _auth = auth;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        var userName = PromptForUserName();
        var password = PromptForPassword();

        var res = await _auth.SignInAsync(userName, password);
        if (!res.Succeeded)
        {
            Helper.Write($"FAILED: [yellow]{string.Join(Environment.NewLine, res.Errors)}[/]");
            return -1;
        }

        return 0;
    }

    static string PromptForUserName()
        => AnsiConsole.Ask<string>("Enter [green]username[/]:");

    static string PromptForPassword()
    {
        return AnsiConsole.Prompt(
            new TextPrompt<string>("Enter [green]password[/]:")
                .PromptStyle("red")
                .Secret());
    }
}
