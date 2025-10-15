using System.Collections.Generic;

namespace Lib.Scryfall.Ingestion.Tests.Fakes;

internal sealed class ScryfallSetFake : IScryfallSet
{
    private readonly string _code;
    private readonly string _name;
    private readonly dynamic _data;
    private readonly string _id;
    private readonly bool _isDigital;
    private readonly string _iconSvgPath;
    private readonly string _parentSetCode;

    public ScryfallSetFake(
        string code = "tst",
        string name = "Test Set",
        dynamic data = null,
        string id = "test-set-id",
        bool isDigital = false,
        string iconSvgPath = "https://example.com/icon.svg",
        string parentSetCode = null)
    {
        _code = code;
        _name = name;
        _data = data ?? new { };
        _id = id;
        _isDigital = isDigital;
        _iconSvgPath = iconSvgPath;
        _parentSetCode = parentSetCode;
    }

    public Url SearchUri() => new ProvidedUrl("https://api.scryfall.com/cards/search?q=set:tst");
    public string Code() => _code;
    public string Name() => _name;
    public dynamic Data() => _data;
    public string Id() => _id;
    public bool IsDigital() => _isDigital;
    public bool IsNotDigital() => IsDigital() is false;
    public Url IconSvgPath() => new ProvidedUrl(_iconSvgPath);
    public string ParentSetCode() => _parentSetCode ?? string.Empty;
    public bool HasParentSet() => _parentSetCode != null;
    public IAsyncEnumerable<IScryfallCard> Cards() => throw new System.NotImplementedException();

}
