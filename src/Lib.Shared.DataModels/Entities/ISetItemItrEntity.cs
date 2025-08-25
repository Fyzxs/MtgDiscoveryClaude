namespace Lib.Shared.DataModels.Entities;

public interface ISetItemItrEntity
{
    string Id { get; }
    string Code { get; }
    int TcgPlayerId { get; }
    string Name { get; }
    string Uri { get; }
    string ScryfallUri { get; }
    string SearchUri { get; }
    string ReleasedAt { get; }
    string SetType { get; }
    int CardCount { get; }
    bool Digital { get; }
    bool NonFoilOnly { get; }
    bool FoilOnly { get; }
    string BlockCode { get; }
    string Block { get; }
    string IconSvgUri { get; }
}
