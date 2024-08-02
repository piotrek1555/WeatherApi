using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.User;

[ApiController]
[Route("api")]
public partial class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }
}

