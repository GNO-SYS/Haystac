using Haystac.Application.Common.Interfaces;

namespace Haystac.Console.Services;

//< TODO: https://github.com/gclodge/Haystac/issues/1
//<       - Need to actually cache the encrypted JWT in ENV VAR, and then pull from there
//<       - Will get User Name and all claims from underlying IAuthService

public class CurrentUser : IUser
{
    public string? Id => Environment.UserName;

    public Task<string?> GetIdAsync() => Task.FromResult(Id);
}
