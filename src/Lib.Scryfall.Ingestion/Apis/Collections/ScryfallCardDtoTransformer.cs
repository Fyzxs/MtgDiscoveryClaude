using Lib.Scryfall.Ingestion.Apis.Dtos;
using Lib.Scryfall.Ingestion.Apis.Models;

namespace Lib.Scryfall.Ingestion.Apis.Collections;

/// <summary>
/// Transforms ExtScryfallCardDto to IScryfallCard.
/// </summary>
internal sealed class ScryfallCardDtoTransformer : IScryfallDtoTransformer<ExtScryfallCardDto, IScryfallCard>
{
    public IScryfallCard Transform(ExtScryfallCardDto dto)
    {
        return new ScryfallCard(dto);
    }
}
