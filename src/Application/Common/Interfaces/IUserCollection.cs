namespace Haystac.Application.Common.Interfaces;

public interface IUserCollection<T> where T : class
{
    public Task<T?> GetByUserNameAsync(string userName);

    public Task<T?> GetByUserIdAsync(string userId);
}
