using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities.Outs.UserSetCards;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Mappers;

internal sealed class FinishCountsOufToOutMapper : IFinishCountsOufToOutMapper
{
    public Task<FinishCountsOutEntity> Map(IFinishCountsOufEntity source)
    {
        return Task.FromResult(new FinishCountsOutEntity
        {
            Total = source.Total,
            NonFoil = source.NonFoil,
            Foil = source.Foil,
            Etched = source.Etched
        });
    }
}
