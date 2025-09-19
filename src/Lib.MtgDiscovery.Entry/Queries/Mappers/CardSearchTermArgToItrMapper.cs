using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Queries.Entities;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.MtgDiscovery.Entry.Queries.Mappers;

internal sealed class CardSearchTermArgToItrMapper : ICardSearchTermArgToItrMapper
{
    public Task<ICardSearchTermItrEntity> Map(ICardSearchTermArgEntity args)
    {
        return Task.FromResult<ICardSearchTermItrEntity>(new CardSearchTermItrEntity { SearchTerm = args.SearchTerm });
    }
}
