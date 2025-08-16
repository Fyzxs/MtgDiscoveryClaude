using Lib.Scryfall.Shared.Apis.Models;
using Lib.Universal.Primitives;

namespace Lib.Scryfall.Ingestion.Models;

internal sealed class ScryfallSetSearchUriAdapter : IScryfallSearchUri
{
    private readonly IScryfallSet _set;

    public ScryfallSetSearchUriAdapter(IScryfallSet set)
    {
        _set = set;
    }

    public Url SearchUri() => _set.SearchUri();
}
