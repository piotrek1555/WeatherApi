using static API.Common.CountriesApi.CountryResponseDto;

namespace API.Common.CountriesApi;

public record CountryResponseDto(decimal[] latlng, string cca3, CountryResponseName name)
{
    public record CountryResponseName(string common, string official);
}
