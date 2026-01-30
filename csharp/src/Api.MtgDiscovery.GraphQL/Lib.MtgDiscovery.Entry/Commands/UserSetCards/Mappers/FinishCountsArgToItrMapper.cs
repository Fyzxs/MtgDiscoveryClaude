using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Commands.UserSetCards.Entities;
using Lib.Shared.DataModels.Entities.Args.UserSetCards;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;

namespace Lib.MtgDiscovery.Entry.Commands.UserSetCards.Mappers;

internal sealed class FinishCountsArgToItrMapper : IFinishCountsArgToItrMapper
{
    public Task<IFinishCountsItrEntity> Map(IFinishCountsArgEntity source)
    {
        return Task.FromResult<IFinishCountsItrEntity>(new FinishCountsItrEntity
        {
            Total = source.Total,
            NonFoil = source.NonFoil,
            Foil = source.Foil,
            Etched = source.Etched
        });
    }
}
