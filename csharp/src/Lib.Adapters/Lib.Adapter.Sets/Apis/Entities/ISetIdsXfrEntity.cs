using System.Collections.Generic;
using Lib.Shared.DataModels.Abstractions;

namespace Lib.Adapter.Sets.Apis.Entities;

/// <summary>
/// Transfer representation of set identifiers used by the adapter layer.
/// This entity crosses the Aggregator→Adapter boundary when no actual entity mapping is needed,
/// providing a simple wrapper for set ID collection values in external system operations.
/// </summary>
public interface ISetIdsXfrEntity : IXfrEntity
{
    /// <summary>
    /// The collection of unique identifiers for sets.
    /// Typically represents the sets' IDs as stored in the external data source.
    /// </summary>
    IEnumerable<string> SetIds { get; }
}
