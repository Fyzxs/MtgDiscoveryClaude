namespace Lib.Shared.DataModels.Abstractions;

/// <summary>
/// Base marker interface for all argument entities.
/// Argument entities represent data from external input (GraphQL, REST, etc.).
/// Data flow: App Layer → Entry Layer
/// </summary>
public interface IArgEntity
{
    // Marker interface - no members
}
