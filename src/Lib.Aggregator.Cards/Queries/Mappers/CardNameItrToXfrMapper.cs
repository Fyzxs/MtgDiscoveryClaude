using System.Threading.Tasks;
using Lib.Adapter.Cards.Apis.Entities;
using Lib.Aggregator.Cards.Queries.Entities;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.Cards.Queries.Mappers;

internal sealed class CardNameItrToXfrMapper : ICardNameItrToXfrMapper
{
    public Task<ICardNameXfrEntity> Map(ICardNameItrEntity source)
    {
        return Task.FromResult<ICardNameXfrEntity>(new CardNameXfrEntity
        {
            CardName = source.CardName
        });
    }
}