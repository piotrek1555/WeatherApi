using API.Common.Cache;
using API.Common.Exceptions;
using API.Common.WeatherApi;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using System.Net;
using System.Net.Http.Json;
using static API.Common.WeatherApi.GetWeatherResponse;
using static API.Features.Weather.GetWeather.GetWeather;

namespace Api.Test.Features.Weather.GetWeather
{
    [TestFixture]
    public class GetWeatherHandlerTests
    {
        private IWeatherApiClient _weatherApiClient;
        private IMemoryCache _memoryCache;
        private IConfiguration _configuration;
        private GetWeatherHandler _handler;

        [TearDown]
        public void CleanUp()
        {
            _memoryCache.Dispose();
        }

        [SetUp]
        public void SetUp()
        {
            _weatherApiClient = Substitute.For<IWeatherApiClient>();
            _memoryCache = new MemoryCache(new MemoryCacheOptions());

            var myConfiguration = new Dictionary<string, string?>
            {
                {"CacheDurationInMinutes", "60"},
                {"Nested:Key1", "NestedValue1"},
            };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(myConfiguration)
                .Build();

            _handler = new GetWeatherHandler(_weatherApiClient, _memoryCache, _configuration);
        }

        [Test]
        public async Task Handle_ShouldReturnWeather_WhenCacheHit()
        {
            // Arrange
            var coordinates = new Coordinates("10.0", "20.0");
            var query = new GetWeatherQuery(coordinates);
            var cachedWeather = new GetWeatherResponse
            {
                hourly = new Hourly
                {
                    time = new List<string> { "2021-10-10T10:00:00Z" },
                    temperature_2m = new List<double> { 25 }
                }
            };
            _memoryCache.Set(CacheKeyGenerator.WeatherCacheKey(coordinates.Latitude, coordinates.Longitude), cachedWeather);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.That(result, Is.EqualTo(cachedWeather));
        }

        [Test]
        public async Task Handle_ShouldReturnWeather_WhenApiCallSucceeds()
        {
            // Arrange
            var coordinates = new Coordinates("10.0", "20.0");
            var query = new GetWeatherQuery(coordinates);
            var weatherResponse = new GetWeatherResponse
            {
                hourly = new Hourly
                {
                    time = new List<string> { "2021-10-10T10:00:00Z" },
                    temperature_2m = new List<double> { 25 }

                }
            };
            var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(weatherResponse)
            };
            _weatherApiClient.GetWeatherAsync(coordinates.Latitude, coordinates.Longitude, CancellationToken.None)
                .Returns(httpResponse);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            var cachedWeather = _memoryCache.Get(CacheKeyGenerator.WeatherCacheKey(coordinates.Latitude, coordinates.Longitude));

            // Assert
            Assert.That(result.hourly.time, Is.EqualTo(weatherResponse.hourly.time));
            Assert.That(result.hourly.temperature_2m, Is.EqualTo(weatherResponse.hourly.temperature_2m));
        }

        [Test]
        public void Handle_ShouldThrowBusinessException_WhenApiCallFails()
        {
            // Arrange
            var coordinates = new Coordinates("10.0", "20.0");
            var query = new GetWeatherQuery(coordinates);
            var httpResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
            _weatherApiClient.GetWeatherAsync(coordinates.Latitude, coordinates.Longitude, CancellationToken.None)
                .Returns(httpResponse);

            // Act & Assert
            Assert.ThrowsAsync<BusinessException>(async () => await _handler.Handle(query, CancellationToken.None));
        }
    }
}
