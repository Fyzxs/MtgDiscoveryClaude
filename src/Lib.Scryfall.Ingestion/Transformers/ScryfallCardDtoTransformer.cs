using Lib.Scryfall.Ingestion.Collections;
using Lib.Scryfall.Ingestion.Dtos;
using Lib.Scryfall.Ingestion.Models;
using Lib.Scryfall.Shared.Apis.Models;

namespace Lib.Scryfall.Ingestion.Transformers;

internal sealed class ScryfallCardDtoTransformer : IScryfallDtoTransformer<ExtScryfallCardDto, IScryfallCard>
{
    private readonly IScryfallSet _set;

    public ScryfallCardDtoTransformer(IScryfallSet set)
    {
        _set = set;
    }
    public IScryfallCard Transform(ExtScryfallCardDto dto)
    {
        return new ScryfallCard(dto, _set);
    }
}
