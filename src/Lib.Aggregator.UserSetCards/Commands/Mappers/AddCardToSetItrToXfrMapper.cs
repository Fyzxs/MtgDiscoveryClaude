using System.Threading.Tasks;
using Lib.Adapter.UserSetCards.Apis.Entities;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;

namespace Lib.Aggregator.UserSetCards.Commands.Mappers;

internal sealed class AddCardToSetItrToXfrMapper : IAddCardToSetItrToXfrMapper
{
    public Task<IAddCardToSetXfrEntity> Map(IAddCardToSetItrEntity source)
    {
        IAddCardToSetXfrEntity result = new AddCardToSetXfrEntity
        {
            UserId = source.UserId,
            SetId = source.SetId,
            CardId = source.CardId,
            SetGroupId = source.SetGroupId,
            FinishType = source.FinishType,
            Count = source.Count
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
