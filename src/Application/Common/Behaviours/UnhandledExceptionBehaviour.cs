using Microsoft.Extensions.Logging;

namespace Haystac.Application.Common.Behaviours;

public class UnhandledExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly ILogger<TRequest> _logger;

    public UnhandledExceptionBehaviour(ILogger<TRequest> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception ex)
        {
            var requestName = typeof(TRequest).Name;

            //< TODO - Consider truncating this request serialization to minimize logging impact on performance
            _logger.LogError(ex, "Haystac Request: Unhandled Exception for Request {Name} {@Request}", requestName, request);

            throw;
        }
    }
}