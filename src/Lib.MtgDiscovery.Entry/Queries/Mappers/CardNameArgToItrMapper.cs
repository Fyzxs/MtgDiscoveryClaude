using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Queries.Entities;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.MtgDiscovery.Entry.Queries.Mappers;

internal sealed class CardNameArgToItrMapper : ICardNameArgToItrMapper
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
