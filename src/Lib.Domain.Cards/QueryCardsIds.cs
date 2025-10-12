using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Domain.Cards;

internal sealed class QueryCardsIds : ICardIdsItrEntity
{
    public QueryCardsIds(ICollection<string> cardIds) => CardIds = cardIds;

    public ICollection<string> CardIds { get; }
}
