namespace API.Common.Constants
{
    public static class ApiConstatns
    {
        public const string CacheDurationInMinutes = nameof(CacheDurationInMinutes);
        public const string CountryApiBaseAddress = nameof(CountryApiBaseAddress);
        public const string WeatherApiBaseAddress = nameof(WeatherApiBaseAddress);
        public const string TokenOptions = nameof(TokenOptions);

        public class Claims
        {
            public const string Longitude = nameof(Longitude);
            public const string Latitude = nameof(Latitude);
        }
    }
}
