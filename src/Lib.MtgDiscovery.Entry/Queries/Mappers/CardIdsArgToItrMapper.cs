using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.MtgDiscovery.Entry.Queries.Entities;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.MtgDiscovery.Entry.Queries.Mappers;

internal sealed class CardIdsArgToItrMapper : ICardIdsArgToItrMapper
{
    public Task<ICardIdsItrEntity> Map(ICardIdsArgEntity arg)
    {
        return Task.FromResult<ICardIdsItrEntity>(new EntryCardIdsItrEntity { CardIds = arg.CardIds });
    }
}
