using System.Text.Json.Serialization;

namespace AgileOctopus;

internal class StandardUnitRates
{
    public required Rate[] Results { get; init; }

    internal record Rate
    {
        public required decimal ValueExcVat { get; init; }
        public required DateTime ValidFrom { get; init; }
        public required DateTime ValidTo { get; init; }
    }
}

[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.SnakeCaseLower)]
[JsonSerializable(typeof(StandardUnitRates))]
internal partial class SourceGenerationContext : JsonSerializerContext;
