using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.Weather;

[ApiController]
[Route("api/[controller]")]
public partial class WeatherController : ControllerBase
{
    private readonly IMediator _mediator;

    public WeatherController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
