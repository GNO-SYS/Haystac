using System.Net.Http.Headers;

using Haystac.Console.Infrastructure.Services;

namespace Haystac.Console.Infrastructure.Http;

public class BearerTokenHandler : DelegatingHandler
{
    private readonly ITokenService _token;

    public BearerTokenHandler(ITokenService token)
    {
        _token = token;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var token = await _token.GetTokenAsync();

        if (token == null)
        {
            throw new Exception("Unable to retrieve cached credentials, please login via 'haystac login'");
        }

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        return await base.SendAsync(request, cancellationToken);
    }
}
