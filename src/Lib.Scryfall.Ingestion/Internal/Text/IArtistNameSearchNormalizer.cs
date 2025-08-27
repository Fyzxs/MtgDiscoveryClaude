using System.Collections.Generic;

namespace Lib.Scryfall.Ingestion.Internal.Text;

internal interface IArtistNameSearchNormalizer
{
    string Normalize(IEnumerable<string> artistNames);
}
