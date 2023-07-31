using Haystac.Application.Authentication.Commands;

namespace Haystac.WebApi.Controllers;

[Route("auth")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthenticationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("signin")]
    public async Task<ActionResult<string?>> SignIn([FromBody] PasswordSignInCommand cmd)
        => await _mediator.Send(cmd);

    //< TODO - Add: 'resetpassword', 'changepassword', 'multifactor'
}
