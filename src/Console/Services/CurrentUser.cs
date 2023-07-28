namespace Haystac.Console.Services;

//< TODO: https://github.com/gclodge/Haystac/issues/1
//<       - Need to actually cache the encrypted JWT in ENV VAR, and then pull from there

public class CurrentUser : IUser
{
    private readonly IAuthenticationService _auth;

    public CurrentUser(IAuthenticationService auth)
    {
        _auth = auth;
    }

    public string? Id => _auth.GetUserId();
}