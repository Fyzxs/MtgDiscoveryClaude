using Lib.Scryfall.Ingestion.Dtos;

namespace Lib.Scryfall.Ingestion.Factories;
internal sealed class ScryfallSetDtoFactory : IScryfallDtoFactory<ExtScryfallSetDto>
{
    public ExtScryfallSetDto Create(dynamic data)
    {
        return new ExtScryfallSetDto(data);
    }
}
