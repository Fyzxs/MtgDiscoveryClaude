using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Adapter.UserCards.Apis.Entities;
using Lib.Adapter.UserSetCards.Apis.Entities;

namespace Lib.Aggregator.UserCards.Commands.Mappers;

/// <summary>
/// Maps IAddUserCardXfrEntity and updated collected list to IAddCardToSetXfrEntity.
/// Computes the remaining finish count (ignoring special) to prevent
/// incorrect removal of cards from set groups when copies still exist.
/// </summary>
internal sealed class AddUserCardXfrToAddCardToSetXfrMapper : IAddUserCardXfrToAddCardToSetXfrMapper
{
    public Task<IAddCardToSetXfrEntity> Map(IAddUserCardXfrEntity source, IEnumerable<UserCardDetailsExtEntity> collectedList)
    {
        int remainingFinishCount = collectedList
            .Where(d => string.Equals(d.Finish, source.Details.Finish, StringComparison.OrdinalIgnoreCase))
            .Sum(d => d.Count);

        IAddCardToSetXfrEntity result = new AddCardToSetXfrEntity
        {
            UserId = source.UserId,
            SetId = source.SetId,
            CardId = source.CardId,
            SetGroupId = source.Details.SetGroupId,
            FinishType = source.Details.Finish,
            Count = source.Details.Count,
            RemainingFinishCount = remainingFinishCount
        };

        return Task.FromResult(result);
    }

    private sealed class AddCardToSetXfrEntity : IAddCardToSetXfrEntity
    {
        public required string UserId { get; init; }
        public required string SetId { get; init; }
        public required string CardId { get; init; }
        public required string SetGroupId { get; init; }
        public required string FinishType { get; init; }
        public required int Count { get; init; }
        public required int RemainingFinishCount { get; init; }
    }
}
