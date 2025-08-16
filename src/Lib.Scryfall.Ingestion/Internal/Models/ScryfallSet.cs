using System.Collections.Generic;
using Lib.Scryfall.Ingestion.Apis.Collections;
using Lib.Scryfall.Ingestion.Apis.Dtos;
using Lib.Scryfall.Ingestion.Apis.Models;
using Lib.Scryfall.Ingestion.Apis.Values;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.Internal.Models;

/// <summary>
/// Implementation of a Scryfall set.
/// </summary>
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
    public Url SearchUri() => new((string)_dto.Data.search_uri);
    public dynamic Data() => _dto.Data;
    public string Id() => _dto.Data.id;
    public bool IsDigital() => _dto.Data.digital ?? false;
    public bool IsNotDigital() => IsDigital() is false;
    public string IconSvgPath() => _dto.Data.icon_svg_uri;
    public string ParentSetCode() => _dto.Data.parent_set_code ?? string.Empty;
    public bool HasParentSet() => _dto.Data.parent_set_code != null;

    public IAsyncEnumerable<IScryfallCard> Cards()
    {
        return new HttpScryfallCardCollection(this, _cardListPagingLogger);
    }
}
