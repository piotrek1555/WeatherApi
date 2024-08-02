using API.Common.Constants;

namespace API.Common.CountriesApi;

public class CountriesApiClient : ICountriesApiClient
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _baseAddress;

    public CountriesApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _baseAddress = configuration.GetValue<string>(ApiConstatns.CountryApiBaseAddress) 
            ?? throw new ArgumentNullException(nameof(configuration), "CountryApiBaseAddress is not set in the app settings");
    }

    public async Task<HttpResponseMessage> GetCountryAsync(string countryCode, CancellationToken cancellationToken)
        => await GetAsync(new Uri($"codes={countryCode}", UriKind.Relative), cancellationToken);

    private async Task<HttpResponseMessage> GetAsync(Uri uri, CancellationToken cancellationToken = default)
    {
        var httpClient = GetClient();
        return await httpClient.GetAsync(uri, cancellationToken);
    }

    private HttpClient GetClient()
    {
        var httpClient = _httpClientFactory.CreateClient("CountryApiClient");
        httpClient.BaseAddress = new Uri(_baseAddress);

        return httpClient;
    }
}
