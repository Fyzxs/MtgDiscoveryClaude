using System.Collections.Generic;

namespace Lib.Scryfall.Shared.Entities;

public interface ITrigramCollectionEntity
{
    string Normalized { get; }
    IReadOnlyList<string> Trigrams { get; }
}
