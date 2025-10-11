namespace AgileOctopus.Twilio;

internal sealed class Message
{
    public required string To { get; init; }
    public required string From { get; init; }
    public required string Body { get; init; }
}
