using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;

namespace Lib.MtgDiscovery.Entry.Commands.UserSetCards.Mappers;

internal sealed class AddCardToSetArgToItrMapper : IAddCardToSetArgToItrMapper
{
    public Task<IAddCardToSetItrEntity> Map(IAddCardToSetArgsEntity source)
    {
        IAddCardToSetItrEntity result = new AddCardToSetItrEntity
        {
            UserId = source.AddCardToSet.UserId,
            SetId = source.AddCardToSet.SetId,
            CardId = source.AddCardToSet.CardId,
            SetGroupId = source.AddCardToSet.SetGroupId,
            FinishType = source.AddCardToSet.FinishType,
            Count = source.AddCardToSet.Count
        };

        return Task.FromResult(result);
    }

    private sealed class AddCardToSetItrEntity : IAddCardToSetItrEntity
    {
        public required string UserId { get; init; }
        public required string SetId { get; init; }
        public required string CardId { get; init; }
        public required string SetGroupId { get; init; }
        public required string FinishType { get; init; }
        public required int Count { get; init; }
    }
}
