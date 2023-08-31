using Haystac.Application.Common.Interfaces;

namespace Haystac.WebApi.Services;

public class UrlService : IUrlService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UrlService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetBaseUrl()
    {
        var context = _httpContextAccessor.HttpContext;
        if (context == null) throw new NullReferenceException("Unable to access HttpContext!");

        return $"{context.Request.Scheme}://{context.Request.Host}{context.Request.PathBase}";
    }
}
