using System.Text.Json.Serialization;

namespace AgileOctopus.Octopus;

[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.SnakeCaseLower)]
[JsonSerializable(typeof(StandardUnitRates))]
internal partial class OctopusSourceGenerationContext : JsonSerializerContext;
