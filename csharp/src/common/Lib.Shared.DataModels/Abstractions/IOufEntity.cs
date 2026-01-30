namespace Lib.Shared.DataModels.Abstractions;

/// <summary>
/// Base marker interface for all output flow entities.
/// Output flow entities represent internal layer outputs before final mapping.
/// Data flow: Domain/Aggregator → Entry Layer
/// </summary>
public interface IOufEntity
{
    // Marker interface - no members
}
