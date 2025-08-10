using Lib.Scryfall.Ingestion.Apis.Dtos;

namespace Lib.Scryfall.Ingestion.Apis.Models;

/// <summary>
/// Implementation of a Scryfall card.
/// </summary>
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
