namespace API.Common.Cache;

public static class CacheKeyGenerator
{
    public static string WeatherCacheKey(string latitude, string longitude)
    {
        return Generate("weather", latitude, longitude);
    }

    public static string Generate(string key, string latitude, string longitude)
    {
        return $"{key}_{latitude}_{longitude}";
    }
}
