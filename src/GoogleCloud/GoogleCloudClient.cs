using System.Net.Http.Json;

namespace AgileOctopus.GoogleCloud;

internal static class GoogleCloudClient
{
    public static async void Send()
    {
        var httpClient = new HttpClient
        {
            DefaultRequestHeaders = { { "Metadata-Flavor", "Google" } },
        };

        var token = (
            await httpClient.GetFromJsonAsync(
                "http://metadata.google.internal/computeMetadata/v1/instance/service-accounts/default/token",
                SourceGenerationContext.Default.Token
            )
        )!;
    }
}

internal class Token
{
    public required string AccessToken { get; init; }
}
