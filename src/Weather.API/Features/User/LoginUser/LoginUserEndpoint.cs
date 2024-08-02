using API.Features.User.LoginUser;
using Microsoft.AspNetCore.Mvc;
using static API.Features.User.LoginUser.LoginUser;

namespace API.Features.User;

public partial class UserController
{
    [HttpPost("login")]
    public async Task<ActionResult<LoginUserResponse>> Login(
        [FromBody] LoginUserRequest loginRequest,
        CancellationToken cancellationToken)
    {
        var tokenInfo = await _mediator.Send(
            new LoginUserCommand(
                loginRequest.Username, 
                loginRequest.Password), 
            cancellationToken);

        return Ok(tokenInfo);
    }
}

