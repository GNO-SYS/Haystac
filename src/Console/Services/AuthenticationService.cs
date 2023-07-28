using Amazon.AspNetCore.Identity.Cognito;
using Amazon.Extensions.CognitoAuthentication;

using Microsoft.AspNetCore.Identity;

using Haystac.Domain.Constants;

namespace Haystac.Console.Services;

public interface IAuthenticationService
{
    Task<Result> SignInAsync(string userName, string password);

    Task SignOutAsync();

    string? GetUserId();

    Task SetUserIdAsync(string? userId);
}

public class AuthenticationService : IAuthenticationService
{
    private static readonly string VAR_NAME = $"{Prefixes.ENV_PREFIX}__UserId";

    private readonly SignInManager<CognitoUser> _signInManager;
    private readonly UserManager<CognitoUser> _userManager;

    public AuthenticationService(SignInManager<CognitoUser> signInManager,
                                 UserManager<CognitoUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    public async Task<Result> SignInAsync(string userName, string password)
    {
        var user = await _userManager.FindByNameAsync(userName);

        if (user == null) return Result.Failure(new[] { "Invalid credentials." });

        var result = await _signInManager.PasswordSignInAsync(user, password, isPersistent: true, lockoutOnFailure: false);

        if (result.Succeeded)
        {
            await SetUserIdAsync(user.UserID);
            return Result.Success();
        }

        if (result.RequiresTwoFactor) return Result.Failure(new[] { "You require 2FA in order to login." });

        if (result.IsCognitoSignInResult())
        {
            if (result is CognitoSignInResult cognitoResult)
            {
                if (cognitoResult.RequiresPasswordChange)
                    return Result.Failure(new[] { "Your password needs to be changed!" });

                if (cognitoResult.RequiresPasswordReset)
                    return Result.Failure(new[] { "Your password needs to be reset!" });
            }
        }

        return Result.Failure(new[] { "Invalid credentials." });
    }

    public async Task SignOutAsync()
    {
        await SetUserIdAsync(null);
        await _signInManager.SignOutAsync();
    }

    public string? GetUserId()
        => Environment.GetEnvironmentVariable(VAR_NAME);

    public Task SetUserIdAsync(string? userId)
    {
        Environment.SetEnvironmentVariable(VAR_NAME, userId);

        return Task.CompletedTask;
    }
}
