using Microsoft.AspNetCore.Http;

namespace Haystac.Application.Common.Interfaces;

public interface IUrlService
{
    string GetBaseUrl();
}
