namespace Lib.Scryfall.Ingestion.Internal.Dtos;
internal sealed class ExtScryfallCardDto : IScryfallDto
{
    public ExtScryfallCardDto(dynamic payload)
    {
        Data = payload;
    }

    public dynamic Data { get; }
}
