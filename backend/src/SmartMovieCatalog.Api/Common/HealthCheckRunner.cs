namespace SmartMovieCatalog.Api.Common;

public static class HealthCheckRunner
{
    private static readonly Uri HealthCheckUri = BuildHealthCheckUri();

    public static int Run()
    {
        using HttpClient httpClient = new()
        {
            Timeout = TimeSpan.FromSeconds(2)
        };

        try
        {
            using HttpResponseMessage response = httpClient.GetAsync(HealthCheckUri).GetAwaiter().GetResult();
            return response.IsSuccessStatusCode ? 0 : 1;
        }
        catch (HttpRequestException)
        {
            return 1;
        }
        catch (TaskCanceledException)
        {
            return 1;
        }
    }

    private static Uri BuildHealthCheckUri()
    {
        return new UriBuilder
        {
            Scheme = Uri.UriSchemeHttp,
            Host = "127.0.0.1",
            Port = 8080,
            Path = "health"
        }.Uri;
    }
}
