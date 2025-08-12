using System.Collections.Generic;
using Lib.Scryfall.Ingestion.Apis.Collections;
using Lib.Scryfall.Ingestion.Apis.Dtos;
using Lib.Scryfall.Ingestion.Apis.Values;

namespace Lib.Scryfall.Ingestion.Apis.Models;

/// <summary>
/// Implementation of a Scryfall set.
/// </summary>
internal sealed class ScryfallSet : IScryfallSet
{
    private readonly ExtScryfallSetDto _dto;

    public ScryfallSet(ExtScryfallSetDto dto)
    {
        _dto = dto;
    }

    public string Code() => _dto.Data.code;
    public string Name() => _dto.Data.name;
    public Url SearchUri() => new((string)_dto.Data.search_uri);
    public dynamic Data() => _dto.Data;

    public IAsyncEnumerable<IScryfallCard> Cards()
    {
        return new HttpScryfallCardCollection(this);
    }
}
