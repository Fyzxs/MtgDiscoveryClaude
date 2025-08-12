namespace Lib.Scryfall.Ingestion.Apis.Dtos;

/// <summary>
/// Factory for creating Scryfall card DTOs.
/// </summary>
internal sealed class ScryfallCardDtoFactory : IScryfallDtoFactory<ExtScryfallCardDto>
{
    public ExtScryfallCardDto Create(dynamic data)
    {
        return new ExtScryfallCardDto(data);
    }
}