using System.Diagnostics.CodeAnalysis;

namespace Lib.Scryfall.Ingestion.Apis.Dtos;

/// <summary>
/// Marker interface for Scryfall DTOs.
/// </summary>
[SuppressMessage("Design", "CA1040:Avoid empty interfaces", Justification = "Marker interface for type constraints")]
public interface IScryfallDto;
