using System.Threading.Tasks;
using Lib.Adapter.Cards.Apis.Entities;
using Lib.Aggregator.Cards.Queries.Entities;
using Lib.Shared.DataModels.Entities;

namespace Lib.Aggregator.Cards.Queries.Mappers;

internal sealed class CardIdsItrToXfrMapper : ICardIdsItrToXfrMapper
{
    public Task<ICardIdsXfrEntity> Map(ICardIdsItrEntity source)
    {
        return Task.FromResult<ICardIdsXfrEntity>(new CardIdsXfrEntity
        {
            CardIds = source.CardIds
        });
    }
}