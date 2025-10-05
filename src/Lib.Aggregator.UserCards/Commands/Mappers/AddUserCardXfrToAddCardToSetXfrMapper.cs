using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.UserCards.Apis.Entities;
using Lib.Adapter.UserSetCards.Apis.Entities;

namespace Lib.Aggregator.UserCards.Commands.Mappers;

/// <summary>
/// Maps IAddUserCardXfrEntity to IAddCardToSetXfrEntity.
/// Creates AddCardToSetXfrEntity for UserSetCards aggregation update.
/// </summary>
internal sealed class AddUserCardXfrToAddCardToSetXfrMapper : IAddUserCardXfrToAddCardToSetXfrMapper
{
    public Task<IAddCardToSetXfrEntity> Map(IAddUserCardXfrEntity source)
    {
        IAddCardToSetXfrEntity result = new AddCardToSetXfrEntity
        {
            UserId = source.UserId,
            SetId = source.SetId,
            CardId = source.CardId,
            SetGroupId = source.Details.SetGroupId,
            FinishType = source.Details.Finish,
            Count = source.Details.Count
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
    }
}
