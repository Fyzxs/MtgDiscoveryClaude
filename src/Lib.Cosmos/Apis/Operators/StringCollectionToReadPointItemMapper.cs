using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.Cosmos.Apis.Ids;

namespace Lib.Cosmos.Apis.Operators;

public sealed class StringCollectionToReadPointItemMapper : IStringCollectionToReadPointItemMapper
{
    public Task<ICollection<ReadPointItem>> Map(IEnumerable<string> values)
    {
        IEnumerable<ReadPointItem> readPointItems = values.Select(value => new ReadPointItem
        {
            Id = new ProvidedCosmosItemId(value),
            Partition = new ProvidedPartitionKeyValue(value)
        });

        return Task.FromResult<ICollection<ReadPointItem>>([.. readPointItems]);
    }
}
