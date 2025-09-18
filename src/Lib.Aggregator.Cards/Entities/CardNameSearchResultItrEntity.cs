using Lib.Shared.DataModels.Entities;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.Cards.Entities;

internal sealed class CardNameSearchResultItrEntity : ICardNameSearchResultItrEntity
{
    public string Name { get; init; }
}
