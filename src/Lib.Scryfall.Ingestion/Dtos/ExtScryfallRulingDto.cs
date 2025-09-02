using Lib.Scryfall.Shared.Apis.Models;

namespace Lib.Scryfall.Ingestion.Dtos;

internal sealed class ExtScryfallRulingDto : IScryfallDto
{
    public ExtScryfallRulingDto(AggregatedRulingData data)
    {
        Data = data;
    }

    public AggregatedRulingData Data { get; }
}