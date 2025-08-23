using System.Collections.Generic;
using Lib.Shared.DataModels.Entities;

namespace Lib.Domain.Cards;

internal sealed class QueryCardsIds : ICardIdsItrEntity
{
    private readonly ICollection<string> _cardIds;

    public QueryCardsIds(ICollection<string> cardIds)
    {
        _cardIds = cardIds;
    }

    public ICollection<string> CardIds => _cardIds;
}
