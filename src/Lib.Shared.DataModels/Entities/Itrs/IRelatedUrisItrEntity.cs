#pragma warning disable CA1056, CA1819
namespace Lib.Shared.DataModels.Entities;

public interface IRelatedUrisItrEntity
{
    string Gatherer { get; }
    string TcgPlayerInfiniteArticles { get; }
    string TcgPlayerInfiniteDecks { get; }
    string EdhRec { get; }
}
