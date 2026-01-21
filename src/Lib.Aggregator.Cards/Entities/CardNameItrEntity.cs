using Lib.Shared.DataModels.Entities.Itrs.Cards;

namespace Lib.Aggregator.Cards.Entities;

internal sealed class CardNameItrEntity : ICardNameItrEntity
{
    public string CardName { get; init; }
}
