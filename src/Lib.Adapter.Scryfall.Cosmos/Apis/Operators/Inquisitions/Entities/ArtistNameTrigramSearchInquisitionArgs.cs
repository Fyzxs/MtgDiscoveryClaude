namespace Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions.Entities;

public sealed class ArtistNameTrigramSearchInquisitionArgs
{
    public string Trigram { get; init; }
    public string Partition { get; init; }
    public string Normalized { get; init; }
}
