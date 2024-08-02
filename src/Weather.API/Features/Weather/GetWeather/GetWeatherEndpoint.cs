using API.Common.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static API.Features.Weather.GetWeather.GetWeather;

namespace API.Features.Weather;

public partial class WeatherController
{
    [HttpGet()]
    [Authorize]
    public async Task<IActionResult> GetWeather(/*string userName*/)
    {
        // In the requirements it is stated that the parameter of this endpoint is {username}
        // Why the required parameter is userName? As the user is already authenticated, we can get the user name from the claims. ??

        var claimsIdentity = User.Identity as ClaimsIdentity;
        var latitude = claimsIdentity!.FindFirst(ApiConstatns.Claims.Latitude)?.Value!;
        var longitude = claimsIdentity.FindFirst(ApiConstatns.Claims.Longitude)?.Value!;
        var coordiantes = new Coordinates(latitude, longitude);

        var result = await _mediator.Send(new GetWeatherQuery(coordiantes));
        return Ok(result);
    }
}
