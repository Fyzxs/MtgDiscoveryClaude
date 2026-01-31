using Lib.Shared.DataModels.Entities.Itrs.Cards;

namespace Lib.MtgDiscovery.Entry.Queries.Entities;

internal sealed class CardNameItrEntity : ICardNameItrEntity
{
    public string CardName { get; init; }
}
