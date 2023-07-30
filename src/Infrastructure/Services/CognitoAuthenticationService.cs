using Amazon.AspNetCore.Identity.Cognito;
using Amazon.Extensions.CognitoAuthentication;

using Microsoft.AspNetCore.Identity;

namespace Haystac.Console.Services;

public class CognitoAuthenticationService : IAuthenticationService
{
    private readonly SignInManager<CognitoUser> _signInManager;
    private readonly UserManager<CognitoUser> _userManager;
    private readonly ILogger<CognitoAuthenticationService> _logger;

    public CognitoAuthenticationService(SignInManager<CognitoUser> signInManager,
                                        UserManager<CognitoUser> userManager,
                                        ILogger<CognitoAuthenticationService> logger)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<string?> SignInAsync(string userName, string password, bool persist)
    {
        var result = await _signInManager.PasswordSignInAsync(userName, password, persist, lockoutOnFailure: false);

        if (result.Succeeded)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null)
            {
                _logger.LogError("User logged in successfully, but wasn't able to be found.");
                return null;
            }

            return user?.SessionTokens.AccessToken.ToString();
        }

        if (result.RequiresTwoFactor)
        {
            _logger.LogWarning("[UNSUPPORTED] 2FA is required for User: {Name}.", userName);
            return null;
        }

        if (result.IsCognitoSignInResult())
        {
            if (result is CognitoSignInResult cognitoResult)
            {
                if (cognitoResult.RequiresPasswordChange)
                {
                    //< TODO - Add PasswordChange functionality
                    _logger.LogWarning("User ({Name}) needs to change their password", userName);
                    return null;
                }

                if (cognitoResult.RequiresPasswordReset)
                {
                    //< TODO - Add PasswordReset functionality
                    _logger.LogWarning("User ({Name}) needs to reset their password.", userName);
                    return null;
                }
            }
        }

        return null;
    }
}
