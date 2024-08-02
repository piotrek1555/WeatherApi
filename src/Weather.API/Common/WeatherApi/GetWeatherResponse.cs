namespace API.Common.WeatherApi;

public record GetWeatherResponse
{
    public double latitude { get; init; }
    public double longitude { get; init; }
    public double generationtime_ms { get; init; }
    public int utc_offset_seconds { get; init; }
    public string timezone { get; init; } = default!;
    public string timezone_abbreviation { get; init; } = default!;
    public double elevation { get; init; } 
    public HourlyUnits hourly_units { get; init; } = default!;
    public Hourly hourly { get; init; } = default!;

    public record HourlyUnits
    {
        public string time { get; init; } = default!;
        public string temperature_2m { get; init; } = default!;
    }

    public record Hourly
    {
        public List<string> time { get; init; } = default!;
        public List<double> temperature_2m { get; init; } = default!;
    }
}