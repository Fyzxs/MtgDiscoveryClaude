using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Aggregator.UserSetCards.Queries.Entities;
using Lib.Shared.DataModels.Entities.Oufs.UserSetCards;

namespace Lib.Aggregator.UserSetCards.Queries.Mappers;

internal sealed class FinishCountsExtToOufMapper : IFinishCountsExtToOufMapper
{
    public Task<IFinishCountsOufEntity> Map(FinishCountsExtEntity source)
    {
        if (source == null) return Task.FromResult<IFinishCountsOufEntity>(new FinishCountsOufEntity());

        return Task.FromResult<IFinishCountsOufEntity>(new FinishCountsOufEntity
        {
            Total = source.Total,
            NonFoil = source.NonFoil,
            Foil = source.Foil,
            Etched = source.Etched
        });
    }
}
