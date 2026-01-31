using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Commands.Collections.Mappers;
using Lib.MtgDiscovery.Entry.Entities.Outs.Collections;
using Lib.Shared.DataModels.Entities.Oufs.Collections;

namespace Lib.MtgDiscovery.Entry.Tests.Commands.Collections.Fakes;

internal sealed class CollectionOufToOutMapperFake : ICollectionOufToOutMapper
{
    public CollectionOutEntity MapResult { get; init; }
    public int MapInvokeCount { get; private set; }
    public ICollectionOufEntity MapLastSource { get; private set; }

    public Task<CollectionOutEntity> Map(ICollectionOufEntity source)
    {
        MapInvokeCount++;
        MapLastSource = source;
        return Task.FromResult(MapResult);
    }
}
