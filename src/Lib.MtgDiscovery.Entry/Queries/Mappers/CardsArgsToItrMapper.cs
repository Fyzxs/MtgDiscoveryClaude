using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.Shared.Abstractions.Mappers;
using Lib.Shared.DataModels.Entities;

namespace Lib.MtgDiscovery.Entry.Queries.Mappers;

internal sealed class CardsArgsToItrMapper : ICreateMapper<ICardIdsArgsEntity, ICardIdsItrEntity>
{
    public Task<ICardIdsItrEntity> Map(ICardIdsArgsEntity args)
    {
        return Task.FromResult<ICardIdsItrEntity>(new EntryCardIdsItrEntity { CardIds = args.CardIds });
    }
}
