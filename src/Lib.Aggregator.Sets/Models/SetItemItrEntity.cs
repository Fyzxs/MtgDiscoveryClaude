using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.Sets.Models;

internal sealed class SetItemItrEntity : ISetItemItrEntity
{
    public string Id { get; init; }
    public string Code { get; init; }
    public int TcgPlayerId { get; init; }
    public string Name { get; init; }
    public string Uri { get; init; }
    public string ScryfallUri { get; init; }
    public string SearchUri { get; init; }
    public string ReleasedAt { get; init; }
    public string SetType { get; init; }
    public int CardCount { get; init; }
    public bool Digital { get; init; }
    public bool NonFoilOnly { get; init; }
    public bool FoilOnly { get; init; }
    public string BlockCode { get; init; }
    public string Block { get; init; }
    public string IconSvgUri { get; init; }
    public int PrintedSize { get; init; }
    public ICollection<ISetGroupingItrEntity> Groupings { get; init; }
}
