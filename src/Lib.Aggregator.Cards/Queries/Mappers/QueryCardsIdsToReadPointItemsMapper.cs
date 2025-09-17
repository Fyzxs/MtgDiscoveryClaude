using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.Cosmos.Apis.Ids;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.DataModels.Entities;

namespace Lib.Aggregator.Cards.Queries.Mappers;

internal sealed class QueryCardsIdsToReadPointItemsMapper : IQueryCardsIdsToReadPointItemsMapper
{
    public Task<IEnumerable<ReadPointItem>> Map(ICardIdsItrEntity args)
    {
        return Task.FromResult(args.CardIds.Select(id => new ReadPointItem
        {
            Id = new ProvidedCosmosItemId(id),
            Partition = new ProvidedPartitionKeyValue(id)
        }));
    }
}
