using System.Threading.Tasks;
using Lib.Cosmos.Adapters;

namespace Lib.Cosmos.Tests.Fakes;

internal sealed class CosmosClientAdapterFake : ICosmosClientAdapter
{
    public Container GetContainerResult { get; init; }
    public int GetContainerInvokeCount { get; private set; }

    public Task<Container> GetContainer()
    {
        GetContainerInvokeCount++;
        return Task.FromResult(GetContainerResult);
    }
}
