using Lib.Shared.DataModels.Entities;

namespace Lib.Aggregator.Cards.Entities;

internal sealed class CardNameItrEntity : ICardNameItrEntity
{
    public string CardName { get; init; }
}