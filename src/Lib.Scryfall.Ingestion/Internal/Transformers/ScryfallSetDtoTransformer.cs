using Lib.Scryfall.Ingestion.Apis.Models;
using Lib.Scryfall.Ingestion.Internal.Collections;
using Lib.Scryfall.Ingestion.Internal.Dtos;
using Lib.Scryfall.Ingestion.Internal.Models;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.Internal.Transformers;
internal sealed class ScryfallSetDtoTransformer : IScryfallDtoTransformer<ExtScryfallSetDto, IScryfallSet>
{
    private readonly ILogger _cardListPagingLogger;

    public ScryfallSetDtoTransformer(ILogger cardListPagingLogger)
    {
        _cardListPagingLogger = cardListPagingLogger;
    }

    public IScryfallSet Transform(ExtScryfallSetDto dto)
    {
        return new ScryfallSet(dto, _cardListPagingLogger);
    }
}
