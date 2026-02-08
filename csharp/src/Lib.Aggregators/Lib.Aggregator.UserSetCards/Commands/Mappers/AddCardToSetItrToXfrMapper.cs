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
        public string UserId { get; init; }
        public string SetId { get; init; }
        public string CardId { get; init; }
        public string SetGroupId { get; init; }
        public string FinishType { get; init; }
        public int Count { get; init; }
        public int RemainingFinishCount { get; init; }
        public string CacheKey => $"add_card_to_set:{UserId}:{SetId}:{CardId}";
    }
}
