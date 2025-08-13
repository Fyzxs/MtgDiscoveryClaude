namespace Lib.Scryfall.Ingestion.Apis.Collections;

/// <summary>
/// Transforms DTOs to domain models.
/// </summary>
public interface IScryfallDtoTransformer<in TDto, out TDomain>
{
    /// <summary>
    /// Transforms a DTO to a domain model.
    /// </summary>
    TDomain Transform(TDto dto);
}
