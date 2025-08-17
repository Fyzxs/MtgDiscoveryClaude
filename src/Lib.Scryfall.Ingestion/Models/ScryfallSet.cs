using System.Collections.Generic;
using Lib.Scryfall.Ingestion.Collections;
using Lib.Scryfall.Ingestion.Dtos;
using Lib.Scryfall.Shared.Apis.Models;
using Lib.Universal.Primitives;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.Models;
internal sealed class ScryfallSet : IScryfallSet
{
    private readonly ExtScryfallSetDto _dto;
    private readonly ILogger _cardListPagingLogger;

    public ScryfallSet(ExtScryfallSetDto dto, ILogger cardListPagingLogger)
    {
        _dto = dto;
        _cardListPagingLogger = cardListPagingLogger;
    }

    public string Code() => _dto.Data.code;
    public string Name() => _dto.Data.name;
    public Url SearchUri() => new ProvidedUrl((string)_dto.Data.search_uri);
    public dynamic Data() => _dto.Data;
    public string Id() => _dto.Data.id;
    public bool IsDigital() => _dto.Data.digital ?? false;
    public bool IsNotDigital() => IsDigital() is false;
    public Url IconSvgPath() => new ProvidedUrl((string)_dto.Data.icon_svg_uri);
    public string ParentSetCode() => _dto.Data.parent_set_code;
    public bool HasParentSet() => _dto.Data.parent_set_code != null;

    public IAsyncEnumerable<IScryfallCard> Cards()
    {
        return new HttpScryfallCardCollection(this, _cardListPagingLogger);
    }
}
