using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Cosmos.Containers;
using Lib.Cosmos.Apis.Adapters;
using Lib.Cosmos.Apis.Operators;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Janitors;

public sealed class CollectionJanitor : ICosmosContainerDeleteOperator
{
    private readonly ICosmosContainerAdapter _container;

    public CollectionJanitor(ILogger logger) : this(new CollectionsCosmosContainer(logger))
    { }

    private CollectionJanitor(ICosmosContainerAdapter container) => _container = container;

    public async Task<OpResponse<T>> DeleteAsync<T>(DeletePointItem item) => await _container.DeleteAsync<T>(item).ConfigureAwait(false);
}
