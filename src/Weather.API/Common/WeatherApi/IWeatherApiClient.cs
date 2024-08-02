namespace API.Common.WeatherApi;

public interface IWeatherApiClient
{
    Task<HttpResponseMessage> GetWeatherAsync(string latitude, string longitude, CancellationToken cancellationToken);
}
