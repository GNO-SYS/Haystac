namespace Haystac.Application.Authentication.Commands;

public record PasswordSignInCommand : IRequest<string?>
{
    [JsonPropertyName("username")]
    public string UserName { get; set; } = string.Empty;

    [JsonPropertyName("password")]
    public string Password { get; set; } = string.Empty;

    [JsonPropertyName("remember_me")]
    public bool? RememberMe { get; set; }
}

public class PasswordSignInCommandHandler 
    : IRequestHandler<PasswordSignInCommand, string?>
{
    private readonly IAuthenticationService _auth;

    public PasswordSignInCommandHandler(IAuthenticationService auth)
    {
        _auth = auth;
    }

    public async Task<string?> Handle(PasswordSignInCommand cmd, CancellationToken cancellationToken)
    {
        var token = await _auth.SignInAsync(cmd.UserName, cmd.Password, cmd.RememberMe ?? true);

        return token;
    }
}
