using Lib.Scryfall.Ingestion.Dtos;
using Lib.Scryfall.Shared.Apis.Models;

namespace Lib.Scryfall.Ingestion.Models;
internal sealed class ScryfallCard : IScryfallCard
{
    private readonly ExtScryfallCardDto _dto;
    private readonly IScryfallSet _set;

    public ScryfallCard(ExtScryfallCardDto dto, IScryfallSet set)
    {
        _dto = dto;
        _set = set;
    }

    public string Id() => _dto.Data.id;
    public string Name() => _dto.Data.name;
    public dynamic Data() => _dto.Data;

    public IScryfallSet Set() => _set;
}
