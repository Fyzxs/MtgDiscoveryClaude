using Lib.Scryfall.Ingestion.Apis.Models;
using Lib.Scryfall.Ingestion.Internal.Dtos;

namespace Lib.Scryfall.Ingestion.Internal.Models;
internal sealed class ScryfallCard : IScryfallCard
{
    private readonly ExtScryfallCardDto _dto;
    private readonly IScryfallSet _set;

    public ScryfallCard(ExtScryfallCardDto dto, IScryfallSet set)
    {
        _dto = dto;
        _set = set;
    }

    public string Name() => _dto.Data.name;
    public dynamic Data() => _dto.Data;

    public IScryfallSet Set() => _set;
}
