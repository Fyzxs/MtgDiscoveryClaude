using Lib.Shared.DataModels.Entities.Itrs.Cards;

namespace Lib.Aggregator.Cards.Entities;

internal sealed class CardNameSearchResultItrEntity : ICardNameSearchResultItrEntity
{
    public string Name { get; init; }
}
