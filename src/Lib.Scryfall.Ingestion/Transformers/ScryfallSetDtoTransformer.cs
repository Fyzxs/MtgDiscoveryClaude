using Lib.Scryfall.Ingestion.Collections;
using Lib.Scryfall.Ingestion.Dtos;
using Lib.Scryfall.Ingestion.Models;
using Lib.Scryfall.Shared.Apis.Models;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.Transformers;

internal sealed class ScryfallSetDtoTransformer : IScryfallDtoTransformer<ExtScryfallSetDto, IScryfallSet>
{
    private readonly ILogger _cardListPagingLogger;

    public ScryfallSetDtoTransformer(ILogger cardListPagingLogger) => _cardListPagingLogger = cardListPagingLogger;

    public IScryfallSet Transform(ExtScryfallSetDto dto) => new ScryfallSet(dto, _cardListPagingLogger);
}
