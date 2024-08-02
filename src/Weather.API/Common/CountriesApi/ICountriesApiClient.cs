namespace API.Common.CountriesApi;

public interface ICountriesApiClient
{
    Task<HttpResponseMessage> GetCountryAsync(string countryCode, CancellationToken cancellationToken);
}
