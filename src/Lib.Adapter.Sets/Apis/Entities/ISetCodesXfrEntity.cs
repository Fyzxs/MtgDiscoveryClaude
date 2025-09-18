using System.Collections.Generic;

namespace Lib.Adapter.Sets.Apis.Entities;

/// <summary>
/// Transfer representation of set codes used by the adapter layer.
/// This entity crosses the Aggregator→Adapter boundary when no actual entity mapping is needed,
/// providing a simple wrapper for set code collection values in external system operations.
/// </summary>
public interface ISetCodesXfrEntity
{
    /// <summary>
    /// The collection of codes that uniquely identify sets.
    /// Typically represents the sets' codes as stored in the external data source.
    /// </summary>
    IEnumerable<string> SetCodes { get; }
}
