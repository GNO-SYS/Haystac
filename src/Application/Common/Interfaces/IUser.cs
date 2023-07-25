namespace Haystac.Application.Common.Interfaces;

public interface IUser
{
    string? Id { get; }

    Task<string?> GetIdAsync();
}