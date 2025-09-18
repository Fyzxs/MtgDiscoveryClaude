using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.MtgDiscovery.Entry.Entities;

internal sealed class CardSearchTermItrEntity : ICardSearchTermItrEntity
{
    public string SearchTerm { get; init; }
}
