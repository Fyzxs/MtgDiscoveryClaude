using System.Threading;
using System.Threading.Tasks;
using Lib.Aggregator.Sets.Apis;
using Lib.Shared.DataModels.Entities.Itrs.Sets;
using Lib.Shared.DataModels.Entities.Oufs.Sets;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.Sets.Queries;

/// <summary>
/// Single-method service for retrieving sets by code collection.
/// Delegates to aggregator layer for data retrieval.
/// </summary>
internal sealed class SetsByCodeDomainService : ISetsByCodeDomainService
{
    private readonly ISetAggregatorService _setAggregatorService;

    public SetsByCodeDomainService(ILogger logger) : this(new SetAggregatorService(logger))
    { }

    private SetsByCodeDomainService(ISetAggregatorService setAggregatorService) => _setAggregatorService = setAggregatorService;

    public async Task<IOperationResponse<ISetItemCollectionOufEntity>> Execute(
        ISetCodesItrEntity input,
        CancellationToken cancellationToken)
        => await _setAggregatorService.SetsByCodeAsync(input, cancellationToken).ConfigureAwait(false);
}
