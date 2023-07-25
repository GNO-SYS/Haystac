using Haystac.Application.Common.Models;

namespace Haystac.Console.Services;

public interface IJsonService
{
    Task<T> ParseFromFile<T>(string jsonFile);
}
