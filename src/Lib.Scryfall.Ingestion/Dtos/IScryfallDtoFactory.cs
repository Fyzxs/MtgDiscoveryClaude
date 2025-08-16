namespace Lib.Scryfall.Ingestion.Dtos;
internal interface IScryfallDtoFactory<out TDto> where TDto : IScryfallDto
{
    TDto Create(dynamic data);
}
