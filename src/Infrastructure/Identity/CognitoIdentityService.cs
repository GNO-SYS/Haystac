using Amazon.Extensions.CognitoAuthentication;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Haystac.Infrastructure.Identity;

public class CognitoIdentityService : IIdentityService
{
    private readonly SignInManager<CognitoUser> _signInManager;
    private readonly UserManager<CognitoUser> _userManager;
    private readonly IAuthorizationService _authorization;

    public CognitoIdentityService(SignInManager<CognitoUser> signIn,
                                  UserManager<CognitoUser> user,
                                  IAuthorizationService authorization)
    {
        _signInManager = signIn;
        _userManager = user;
        _authorization = authorization;
    }

    public async Task<bool> AuthorizeAsync(string userId, string policyName)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null) return false;

        var principal = await _signInManager.CreateUserPrincipalAsync(user);

        var result = await _authorization.AuthorizeAsync(principal, policyName);

        return result.Succeeded;
    }

    public async Task<string?> GetUserNameAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        return user?.Username;
    }

    public async Task<bool> IsInRoleAsync(string userId, string role)
    {
        var user = await _userManager.FindByIdAsync(userId);

        return user != null && await _userManager.IsInRoleAsync(user, role);
    }

    public Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password)
    {
        throw new NotImplementedException();
    }

    public Task<Result> DeleteUserAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<string?> GetClientIdAsync(string userId)
    {
        throw new NotImplementedException();
    }
}
