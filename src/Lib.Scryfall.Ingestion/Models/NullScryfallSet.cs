using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Scryfall.Shared.Apis.Models;
using Lib.Universal.Primitives;

namespace Lib.Scryfall.Ingestion.Models;

internal sealed class NullScryfallSet : IScryfallSet
{
    private readonly string _code;

    public NullScryfallSet(string code)
    {
        _code = code;
    }

    public string Code() => _code;
    public string Name() => string.Empty;
    public string Id() => _code;
    public dynamic Data() => new { };
    public bool IsDigital() => false;
    public bool IsNotDigital() => true;
    public Url IconSvgPath() => new ProvidedUrl(string.Empty);
    public string ParentSetCode() => string.Empty;
    public bool HasParentSet() => false;
    public IAsyncEnumerable<IScryfallCard> Cards() => EmptyCards();
    public Url SearchUri() => new ProvidedUrl(string.Empty);

    private static async IAsyncEnumerable<IScryfallCard> EmptyCards()
    {
        await Task.CompletedTask.ConfigureAwait(false);
        yield break;
    }
}
