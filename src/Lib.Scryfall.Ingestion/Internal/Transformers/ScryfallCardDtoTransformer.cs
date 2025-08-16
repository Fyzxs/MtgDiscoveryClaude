using Lib.Scryfall.Ingestion.Apis.Collections;
using Lib.Scryfall.Ingestion.Apis.Dtos;
using Lib.Scryfall.Ingestion.Apis.Models;
using Lib.Scryfall.Ingestion.Internal.Models;

namespace Lib.Scryfall.Ingestion.Internal.Transformers;

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
