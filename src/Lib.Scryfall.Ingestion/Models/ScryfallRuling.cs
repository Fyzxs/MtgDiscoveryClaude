using Lib.Scryfall.Ingestion.Dtos;
using Lib.Scryfall.Shared.Apis.Models;

namespace Lib.Scryfall.Ingestion.Models;

internal sealed class ScryfallRuling : IScryfallRuling
{
    private readonly ExtScryfallRulingDto _dto;

    public ScryfallRuling(ExtScryfallRulingDto dto)
    {
        _dto = dto;
    }

    public string OracleId() => _dto.Data.OracleId ?? string.Empty;

    public AggregatedRulingData Data() => _dto.Data;
}