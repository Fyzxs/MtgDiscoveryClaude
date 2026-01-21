using System.Threading.Tasks;
using Lib.Aggregator.UserSetCards.Commands.Entities;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;
using Lib.Shared.DataModels.Entities.Xfrs.UserSetCards;

namespace Lib.Aggregator.UserSetCards.Commands.Mappers;

internal sealed class FinishCountsItrToXfrMapper : IFinishCountsItrToXfrMapper
{
    public Task<IFinishCountsXfrEntity> Map(IFinishCountsItrEntity source)
    {
        return Task.FromResult<IFinishCountsXfrEntity>(new FinishCountsXfrEntity
        {
            Total = source.Total,
            NonFoil = source.NonFoil,
            Foil = source.Foil,
            Etched = source.Etched
        });
    }
}
