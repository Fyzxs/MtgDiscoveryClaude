namespace Lib.Scryfall.Ingestion.Apis.Dtos;

/// <summary>
/// Factory for creating DTOs from dynamic data.
/// </summary>
public interface IScryfallDtoFactory<out TDto> where TDto : IScryfallDto
{
    /// <summary>
    /// Creates a DTO from dynamic data.
    /// </summary>
    TDto Create(dynamic data);
}
