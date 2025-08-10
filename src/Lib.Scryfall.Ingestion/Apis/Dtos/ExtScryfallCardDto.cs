namespace Lib.Scryfall.Ingestion.Apis.Dtos;

/// <summary>
/// External DTO wrapper for Scryfall card data.
/// </summary>
public sealed class ExtScryfallCardDto : IScryfallDto
{
    public ExtScryfallCardDto(dynamic payload)
    {
        Data = payload;
    }

    public dynamic Data { get; }
}
