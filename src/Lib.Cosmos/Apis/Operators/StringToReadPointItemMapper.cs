using System.Threading.Tasks;
using Lib.Cosmos.Apis.Ids;

namespace Lib.Cosmos.Apis.Operators;

/// <summary>
/// Maps a string value to a ReadPointItem where both Id and Partition use the same value.
/// </summary>
public sealed class StringToReadPointItemMapper : IStringToReadPointItemMapper
{
    public Task<ReadPointItem> Map(string value)
    {
        ReadPointItem readPointItem = new()
        {
            Id = new ProvidedCosmosItemId(value),
            Partition = new ProvidedPartitionKeyValue(value)
        };

        return Task.FromResult(readPointItem);
    }
}
