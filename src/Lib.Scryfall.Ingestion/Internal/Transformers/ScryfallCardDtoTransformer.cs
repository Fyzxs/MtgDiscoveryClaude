using Lib.Scryfall.Ingestion.Apis.Models;
using Lib.Scryfall.Ingestion.Internal.Collections;
using Lib.Scryfall.Ingestion.Internal.Dtos;
using Lib.Scryfall.Ingestion.Internal.Models;

namespace Lib.Scryfall.Ingestion.Internal.Transformers;
internal sealed class ScryfallCardDtoTransformer : IScryfallDtoTransformer<ExtScryfallCardDto, IScryfallCard>
{
    public IScryfallCard Transform(ExtScryfallCardDto dto)
    {
        return new ScryfallCard(dto);
    }
}
