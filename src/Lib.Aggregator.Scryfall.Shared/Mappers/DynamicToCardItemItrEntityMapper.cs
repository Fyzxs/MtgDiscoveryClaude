using System.Threading.Tasks;
using Lib.Aggregator.Scryfall.Shared.Entities;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.Scryfall.Shared.Mappers;

public sealed class DynamicToCardItemItrEntityMapper : IDynamicToCardItemItrEntityMapper
{
    public Task<ICardItemItrEntity> Map(dynamic source)
    {
        return Task.FromResult<ICardItemItrEntity>(new CardItemItrEntity(source));
    }
}
