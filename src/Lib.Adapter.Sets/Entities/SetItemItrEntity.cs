using System.Collections.Generic;
using Lib.Shared.DataModels.Entities;

namespace Lib.Adapter.Sets.Entities;

/// <summary>
/// Adapter-specific implementation of ISetItemItrEntity.
/// 
/// This internal entity provides the implementation for set data within the adapter.
/// It is created by the mapper and used internally for data transformation operations.
/// </summary>
internal sealed class SetItemItrEntity : ISetItemItrEntity
{
    public string Id { get; init; } = string.Empty;
    public string Code { get; init; } = string.Empty;
    public int TcgPlayerId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Uri { get; init; } = string.Empty;
    public string ScryfallUri { get; init; } = string.Empty;
    public string SearchUri { get; init; } = string.Empty;
    public string ReleasedAt { get; init; } = string.Empty;
    public string SetType { get; init; } = string.Empty;
    public int CardCount { get; init; }
    public bool Digital { get; init; }
    public bool NonFoilOnly { get; init; }
    public bool FoilOnly { get; init; }
    public string BlockCode { get; init; } = string.Empty;
    public string Block { get; init; } = string.Empty;
    public string IconSvgUri { get; init; } = string.Empty;
    public int PrintedSize { get; init; }
    public ICollection<ISetGroupingItrEntity> Groupings { get; init; } = [];
}
