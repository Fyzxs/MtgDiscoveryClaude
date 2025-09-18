using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.MtgDiscovery.Entry.Queries.Entities;
using Lib.Shared.Abstractions.Mappers;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.MtgDiscovery.Entry.Queries.Mappers;

internal interface ICardsArgsToItrMapper : ICreateMapper<ICardIdsArgEntity, ICardIdsItrEntity>;

internal sealed class CardsArgsToItrMapper : ICardsArgsToItrMapper
{
    public Task<ICardIdsItrEntity> Map(ICardIdsArgEntity arg)
    {
        return Task.FromResult<ICardIdsItrEntity>(new EntryCardIdsItrEntity { CardIds = arg.CardIds });
    }
}
