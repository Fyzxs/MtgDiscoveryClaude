namespace Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions.Args;

public sealed class CardNameTrigramSearchInquisitionArgs
{
    public string Trigram { get; init; }
    public string Partition { get; init; }
    public string Normalized { get; init; }
}
