using System.Text.Json.Serialization;

namespace AgileOctopus.Twilio;

[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
[JsonSerializable(typeof(Message))]
internal partial class TwilioSourceGenerationContext : JsonSerializerContext;
