using Lib.Scryfall.Ingestion.Internal.Dtos;

namespace Lib.Scryfall.Ingestion.Internal.Factories;
internal sealed class ScryfallCardDtoFactory : IScryfallDtoFactory<ExtScryfallCardDto>
{
    public ExtScryfallCardDto Create(dynamic data)
    {
        return new ExtScryfallCardDto(data);
    }
}
