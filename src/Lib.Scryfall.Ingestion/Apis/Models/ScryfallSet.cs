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
    public Url SearchUri() => new(_dto.Data.search_uri.ToString());
    public dynamic Data() => _dto.Data;
}
