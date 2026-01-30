namespace Lib.Shared.DataModels.Abstractions;

/// <summary>
/// Base marker interface for all internal transfer entities.
/// Internal transfer entities move data between service layers.
/// Data flow: Entry ↔ Shared ↔ Domain ↔ Aggregator
/// </summary>
public interface IItrEntity
{
    // Marker interface - no members
}
