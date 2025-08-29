using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.Shared.DataModels.Entities;

namespace Lib.MtgDiscovery.Entry.Queries.Mappers;

internal sealed class CardSearchTermArgsToItrMapper : ICardSearchTermArgsToItrMapper
{
    public async Task<ICardSearchTermItrEntity> Map(ICardSearchTermArgEntity args)
    {
        await Task.CompletedTask.ConfigureAwait(false);
        return new CardSearchTermItrEntity { SearchTerm = args.SearchTerm };
    }
}