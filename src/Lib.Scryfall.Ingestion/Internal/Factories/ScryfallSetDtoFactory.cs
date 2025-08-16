using Lib.Scryfall.Ingestion.Internal.Dtos;

namespace Lib.Scryfall.Ingestion.Internal.Factories;
internal sealed class ScryfallSetDtoFactory : IScryfallDtoFactory<ExtScryfallSetDto>
{
    public ExtScryfallSetDto Create(dynamic data)
    {
        return new ExtScryfallSetDto(data);
    }
}
