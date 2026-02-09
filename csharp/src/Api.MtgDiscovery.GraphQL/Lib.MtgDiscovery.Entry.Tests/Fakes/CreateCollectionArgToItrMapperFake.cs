using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Commands.Collections.Mappers;
using Lib.Shared.DataModels.Entities.Args.Collections;
using Lib.Shared.DataModels.Entities.Itrs.Collections;

namespace Lib.MtgDiscovery.Entry.Tests.Fakes;

internal sealed class CreateCollectionArgToItrMapperFake : ICreateCollectionArgToItrMapper
{
    public ICollectionItrEntity MapResult { get; init; }
    public int MapInvokeCount { get; private set; }
    public ICreateCollectionArgEntity MapLastSource { get; private set; }
    public string MapLastUserId { get; private set; }

    public Task<ICollectionItrEntity> Map(ICreateCollectionArgEntity source1, string userId)
    {
        MapInvokeCount++;
        MapLastSource = source1;
        MapLastUserId = userId;
        return Task.FromResult(MapResult);
    }
}
