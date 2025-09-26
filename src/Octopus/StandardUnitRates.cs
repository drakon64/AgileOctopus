using System.Text.Json.Serialization;

namespace AgileOctopus.Octopus;

internal class StandardUnitRates
{
    public required byte Count { get; init; }
    public required Rate[] Results { get; init; }
}

[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.SnakeCaseLower)]
[JsonSerializable(typeof(StandardUnitRates))]
internal partial class SourceGenerationContext : JsonSerializerContext;
