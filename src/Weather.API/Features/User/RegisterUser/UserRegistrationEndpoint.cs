using API.Features.User.RegisterUser;
using Microsoft.AspNetCore.Mvc;
using static API.Features.User.RegisterUser.UserRegistration;

namespace API.Features.User;

public partial class UserController 
{
    [HttpPost("registration")]
    public async Task<ActionResult<UserRegistrationResponse>> Register(
        [FromBody] UserRegistrationRequest request, 
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new UserRegistrationCommand(
                request.FirstName, 
                request.LastName, 
                request.Password, 
                request.Country),
            cancellationToken);

        return Ok(response);
    }
}