using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Scryfall.Shared.Apis.Models;
using Lib.Universal.Primitives;

namespace Lib.Scryfall.Ingestion.Models;

internal sealed class NullScryfallSet : IScryfallSet
{
    private readonly string _code;

    public NullScryfallSet(string code) => _code = code;

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

#pragma warning disable CS1998 // Async method lacks 'await' operators - yield break requires async signature
    private static async IAsyncEnumerable<IScryfallCard> EmptyCards()
    {
        yield break;
    }
#pragma warning restore CS1998
}
