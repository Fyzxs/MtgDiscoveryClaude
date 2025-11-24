using Lib.Shared.DataModels.Entities.Itrs.Cards;

namespace Lib.MtgDiscovery.Entry.Queries.Entities;

internal sealed class CardSearchTermItrEntity : ICardSearchTermItrEntity
{
    public string SearchTerm { get; init; }
}
