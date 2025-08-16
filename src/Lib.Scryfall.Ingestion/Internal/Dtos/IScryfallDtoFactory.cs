namespace Lib.Scryfall.Ingestion.Internal.Dtos;
internal interface IScryfallDtoFactory<out TDto> where TDto : IScryfallDto
{
    TDto Create(dynamic data);
}
