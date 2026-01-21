namespace Lib.Scryfall.Ingestion.Dtos;

internal sealed class ExtScryfallSetDto : IScryfallDto
{
    public ExtScryfallSetDto(dynamic payload)
    {
        Data = payload;
    }

    public dynamic Data { get; }
}
