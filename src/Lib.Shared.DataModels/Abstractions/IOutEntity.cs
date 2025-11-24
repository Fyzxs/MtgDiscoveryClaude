namespace Lib.Shared.DataModels.Abstractions;

/// <summary>
/// Base marker interface for all output entities.
/// Output entities are returned to external layer (GraphQL responses, etc.).
/// Data flow: Entry Layer → App Layer
/// </summary>
public interface IOutEntity
{
    // Marker interface - no members
}
