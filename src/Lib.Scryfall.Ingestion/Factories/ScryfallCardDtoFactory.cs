using Lib.Scryfall.Ingestion.Dtos;

namespace Lib.Scryfall.Ingestion.Factories;

internal sealed class ScryfallCardDtoFactory : IScryfallDtoFactory<ExtScryfallCardDto>
{
    public ExtScryfallCardDto Create(dynamic data)
    {
        return new ExtScryfallCardDto(data);
    }
}
