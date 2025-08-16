using Lib.Scryfall.Ingestion.Apis.Models;
using Lib.Scryfall.Ingestion.Apis.Values;

namespace Lib.Scryfall.Ingestion.Internal.Models;

internal sealed class ScryfallSetSearchUriAdapter : IScryfallSearchUri
{
    private readonly IScryfallSet _set;

    public ScryfallSetSearchUriAdapter(IScryfallSet set)
    {
        _set = set;
    }

    public Url SearchUri() => _set.SearchUri();
}
