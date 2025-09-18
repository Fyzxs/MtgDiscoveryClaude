using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.MtgDiscovery.Entry.Queries.Mappers;

internal sealed class CardNameArgsToItrMapper : ICardNameArgsToItrMapper
{
    public Task<ICardNameItrEntity> Map(ICardNameArgEntity source)
    {
        ICardNameItrEntity result = new CardNameItrEntity
        {
            CardName = source.CardName
        };

        return Task.FromResult(result);
    }
}
