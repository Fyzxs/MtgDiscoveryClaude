namespace Lib.Scryfall.Ingestion.Apis.Dtos;

/// <summary>
/// Factory for creating Scryfall set DTOs.
/// </summary>
internal sealed class ScryfallSetDtoFactory : IScryfallDtoFactory<ExtScryfallSetDto>
{
    public ExtScryfallSetDto Create(dynamic data)
    {
        return new ExtScryfallSetDto(data);
    }
}
