using Lib.Scryfall.Ingestion.Apis.Models;
using Lib.Scryfall.Ingestion.Apis.Values;

namespace Lib.Scryfall.Ingestion.Internal.Models;

internal sealed class ProvidedScryfallSearchUri : IScryfallSearchUri
{
    private readonly Url _url;

    public ProvidedScryfallSearchUri(Url url)
    {
        _url = url;
    }

    public Url SearchUri() => _url;
}
