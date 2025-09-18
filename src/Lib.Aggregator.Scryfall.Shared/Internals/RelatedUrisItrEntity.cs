using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.Scryfall.Shared.Internals;

internal sealed class RelatedUrisItrEntity : IRelatedUrisItrEntity
{
    private readonly dynamic _data;

    public RelatedUrisItrEntity(dynamic data) => _data = data;

    public string Gatherer => _data?.gatherer;
    public string TcgPlayerInfiniteArticles => _data?.tcgplayer_infinite_articles;
    public string TcgPlayerInfiniteDecks => _data?.tcgplayer_infinite_decks;
    public string EdhRec => _data?.edhrec;
}
