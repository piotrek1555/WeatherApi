using API.Common.Cache;
using API.Common.Constants;
using API.Common.Exceptions;
using API.Common.WeatherApi;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace API.Features.Weather.GetWeather;

public class GetWeather
{
    public record Coordinates(string Latitude, string Longitude);

    public record GetWeatherQuery(Coordinates Coordinates) : IRequest<GetWeatherResponse>;

    public sealed class GetWeatherHandler : IRequestHandler<GetWeatherQuery, GetWeatherResponse>
    {
        private readonly IWeatherApiClient _weatherApiClient;
        private readonly IMemoryCache _memoryCache;
        private readonly int _cacheDurationInMinutes;

        public GetWeatherHandler(
            IWeatherApiClient weatherApiClient, 
            IMemoryCache memoryCache,
            IConfiguration configuration)
        {
            _weatherApiClient = weatherApiClient;
            _memoryCache = memoryCache;
            _cacheDurationInMinutes = configuration.GetValue<int>(ApiConstatns.CacheDurationInMinutes);
        }

        public async Task<GetWeatherResponse> Handle(GetWeatherQuery request, CancellationToken cancellationToken)
        {
            var cahcheKey = CacheKeyGenerator.WeatherCacheKey(request.Coordinates.Latitude, request.Coordinates.Longitude);

            if (_memoryCache.TryGetValue(cahcheKey, out GetWeatherResponse? weather))
            {
                return weather!;
            }
            
            var response = await _weatherApiClient.GetWeatherAsync(
                request.Coordinates.Latitude!, 
                request.Coordinates.Longitude!, 
                cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                throw new BusinessException("Failed to get weather data from the weather API.");
            }

            weather = (await response.Content.ReadFromJsonAsync<GetWeatherResponse>(cancellationToken: cancellationToken))!;
            _memoryCache.Set(cahcheKey, weather, TimeSpan.FromMinutes(_cacheDurationInMinutes));

            //TODO return domain model instead of the raw weather api response
            return weather;
        }
    }
}
