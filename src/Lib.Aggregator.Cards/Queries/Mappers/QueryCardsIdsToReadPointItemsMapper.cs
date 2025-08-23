using System.Collections.Generic;
using System.Linq;
using Lib.Cosmos.Apis.Ids;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.DataModels.Entities;

namespace Lib.Aggregator.Cards.Queries.Mappers;

internal sealed class QueryCardsIdsToReadPointItemsMapper
{
    public IEnumerable<ReadPointItem> Map(ICardIdsItrEntity args)
    {
        return args.CardIds.Select(id => new ReadPointItem
        {
            Id = new ProvidedCosmosItemId(id),
            Partition = new ProvidedPartitionKeyValue(id)
        });
    }
}
