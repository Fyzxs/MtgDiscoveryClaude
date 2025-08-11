using Lib.Scryfall.Ingestion.Apis.Models;
using Lib.Scryfall.Ingestion.Apis.Values;

namespace Lib.Scryfall.Ingestion.Tests.Fakes;

internal sealed class ScryfallSetFake : IScryfallSet
{
    public dynamic Data { get; init; }
    public string NameResult { get; init; }
    public string CodeResult { get; init; }
    public Url SearchUriResult { get; init; }

    public int NameInvokeCount { get; private set; }
    public int CodeInvokeCount { get; private set; }
    public int SearchUriInvokeCount { get; private set; }
    public int DataInvokeCount { get; private set; }

    public string Code()
    {
        CodeInvokeCount++;
        return CodeResult ?? Data?.code?.ToString();
    }

    public string Name()
    {
        NameInvokeCount++;
        return NameResult ?? Data?.name?.ToString();
    }

    public Url SearchUri()
    {
        SearchUriInvokeCount++;
        return SearchUriResult ?? (Data?.search_uri != null ? new Url(Data.search_uri.ToString()) : null);
    }

    dynamic IScryfallSet.Data()
    {
        DataInvokeCount++;
        return Data;
    }
}
