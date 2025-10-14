using System.Collections.Generic;

namespace Lib.Scryfall.Shared.Entities;

public sealed class TrigramCollectionEntity : ITrigramCollectionEntity
{
    public required string Normalized { get; init; }
    public required IReadOnlyList<string> Trigrams { get; init; }
}
