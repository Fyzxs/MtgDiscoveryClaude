namespace Lib.Scryfall.Ingestion.Apis.Dtos;

/// <summary>
/// External DTO wrapper for Scryfall set data.
/// </summary>
public sealed class ExtScryfallSetDto : IScryfallDto
{
    public ExtScryfallSetDto(dynamic payload)
    {
        Data = payload;
    }

    public dynamic Data { get; }
}
