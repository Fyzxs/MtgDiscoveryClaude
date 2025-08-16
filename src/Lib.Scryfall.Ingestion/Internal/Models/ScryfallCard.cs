using Lib.Scryfall.Ingestion.Apis.Models;
using Lib.Scryfall.Ingestion.Internal.Dtos;

namespace Lib.Scryfall.Ingestion.Internal.Models;
internal sealed class ScryfallCard : IScryfallCard
{
    private readonly ExtScryfallCardDto _dto;

    public ScryfallCard(ExtScryfallCardDto dto)
    {
        _dto = dto;
    }

    public string Name() => _dto.Data.name;
    public dynamic Data() => _dto.Data;
}
