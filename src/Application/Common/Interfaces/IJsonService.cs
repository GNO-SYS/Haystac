namespace Haystac.Application.Common.Interfaces;

public interface IJsonService
{
    Task<T> ParseFromFile<T>(string jsonFile);
}