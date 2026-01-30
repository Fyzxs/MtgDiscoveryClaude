using System.Collections.Generic;

namespace Lib.Adapter.UserSetCards.Apis.Entities;

/// <summary>
/// Transfer entity representing a finish-specific group of cards within a user's set collection.
///
/// This entity groups cards by finish type (non-foil, foil, etched) within a collector number group.
/// Each finish group contains the card IDs for that specific finish variant.
///
/// Entity Type: XfrEntity - crosses Aggregator→Adapter boundary
/// </summary>
public interface IUserSetCardFinishGroupXfrEntity
{
    /// <summary>
    /// Collection of card IDs in this finish group.
    /// Card IDs reference specific print variants with this finish type.
    /// </summary>
    ICollection<string> Cards { get; }
}
