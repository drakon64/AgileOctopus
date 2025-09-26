namespace AgileOctopus.Octopus;

internal record Rate
{
    public required decimal ValueExcVat { get; init; }
    public required DateTime ValidFrom { get; set; }
    public required DateTime ValidTo { get; set; }
}
