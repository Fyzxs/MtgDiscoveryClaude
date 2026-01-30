using Lib.Scryfall.Shared.Apis.Models;
using Lib.Universal.Primitives;

namespace Lib.Scryfall.Ingestion.Models;

internal sealed class ProvidedScryfallSearchUri : IScryfallSearchUri
{
    private readonly Url _url;

    public ProvidedScryfallSearchUri(Url url) => _url = url;

    public Url SearchUri() => _url;
}
