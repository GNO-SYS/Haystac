namespace Haystac.Application.Common.Interfaces;

public interface IAuthenticationService
{
    Task<string?> SignInAsync(string userName, string password, bool persist);
}