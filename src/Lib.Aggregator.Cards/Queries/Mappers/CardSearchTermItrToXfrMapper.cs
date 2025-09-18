using System.Threading.Tasks;
using Lib.Adapter.Cards.Apis.Entities;
using Lib.Aggregator.Cards.Queries.Entities;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.Cards.Queries.Mappers;

internal sealed class CardSearchTermItrToXfrMapper : ICardSearchTermItrToXfrMapper
{
    public Task<ICardSearchTermXfrEntity> Map(ICardSearchTermItrEntity source)
    {
        return Task.FromResult<ICardSearchTermXfrEntity>(new CardSearchTermXfrEntity
        {
            SearchTerm = source.SearchTerm
        });
    }
}