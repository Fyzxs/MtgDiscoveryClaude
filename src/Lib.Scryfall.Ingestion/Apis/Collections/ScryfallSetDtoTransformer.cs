using Lib.Scryfall.Ingestion.Apis.Dtos;
using Lib.Scryfall.Ingestion.Apis.Models;

namespace Lib.Scryfall.Ingestion.Apis.Collections;

/// <summary>
/// Transforms ExtScryfallSetDto to IScryfallSet.
/// </summary>
internal sealed class ScryfallSetDtoTransformer : IScryfallDtoTransformer<ExtScryfallSetDto, IScryfallSet>
{
    public IScryfallSet Transform(ExtScryfallSetDto dto)
    {
        return new ScryfallSet(dto);
    }
}
