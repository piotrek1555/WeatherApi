using API.Common.Constants;

namespace API.Common.WeatherApi;

public class WeatherApiClient: IWeatherApiClient
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _baseAddress;

    public WeatherApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _baseAddress = configuration.GetValue<string>(ApiConstatns.WeatherApiBaseAddress)
            ?? throw new ArgumentNullException(nameof(configuration), "");
    }

    public async Task<HttpResponseMessage> GetWeatherAsync(string latitude, string longitude, CancellationToken cancellationToken)
        => await GetAsync(new Uri($"forecast?latitude={latitude}&longitude={longitude}&hourly=temperature_2m", UriKind.Relative), cancellationToken);

    private async Task<HttpResponseMessage> GetAsync(Uri uri, CancellationToken cancellationToken = default)
    {
        var httpClient = GetClient();
        return await httpClient.GetAsync(uri, cancellationToken);
    }

    private HttpClient GetClient()
    {
        var httpClient = _httpClientFactory.CreateClient("WeatherApiClient");
        httpClient.BaseAddress = new Uri(_baseAddress);

        return httpClient;
    }
}

