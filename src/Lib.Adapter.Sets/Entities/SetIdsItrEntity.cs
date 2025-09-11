using System.Collections.Generic;
using Lib.Shared.DataModels.Entities;

namespace Lib.Adapter.Sets.Entities;

/// <summary>
/// Adapter-specific implementation of ISetIdsItrEntity.
/// 
/// This entity is used internally by the adapter for creating set ID requests
/// during recursive operations within the adapter layer. It provides the
/// necessary implementation to support internal method calls.
/// </summary>
internal sealed class SetIdsItrEntity : ISetIdsItrEntity
{
    public IReadOnlyCollection<string> SetIds { get; init; } = [];
}