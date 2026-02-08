using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.SealedProducts;
using Lib.Adapter.SealedProducts.Apis;
using Lib.Adapter.SealedProducts.Apis.Entities;
using Lib.Aggregator.SealedProducts.Exceptions;
using Lib.Aggregator.SealedProducts.Queries.Mappers;
using Lib.Shared.DataModels.Entities.Itrs.SealedProducts;
using Lib.Shared.DataModels.Entities.Oufs.SealedProducts;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.SealedProducts.Queries;

internal sealed class SealedProductsBySetCodeAggregator : ISealedProductsBySetCodeAggregator
{
    private readonly ISealedProductsAdapterService _adapterService;
    private readonly ISealedProductsBySetCodeItrToXfrMapper _itrToXfrMapper;
    private readonly ISealedProductExtToOufMapper _extToOufMapper;

    public SealedProductsBySetCodeAggregator(ILogger logger) : this(
        new SealedProductsAdapterService(logger),
        new SealedProductsBySetCodeItrToXfrMapper(),
        new SealedProductExtToOufMapper())
    { }

    private SealedProductsBySetCodeAggregator(
        ISealedProductsAdapterService adapterService,
        ISealedProductsBySetCodeItrToXfrMapper itrToXfrMapper,
        ISealedProductExtToOufMapper extToOufMapper)
    {
        _adapterService = adapterService;
        _itrToXfrMapper = itrToXfrMapper;
        _extToOufMapper = extToOufMapper;
    }

    public async Task<IOperationResponse<IEnumerable<ISealedProductOufEntity>>> Execute(
        ISealedProductsBySetCodeItrEntity input,
        CancellationToken cancellationToken)
    {
        ISealedProductsBySetCodeXfrEntity xfrEntity = await _itrToXfrMapper.Map(input).ConfigureAwait(false);
        IOperationResponse<IEnumerable<SealedProductExtEntity>> response = await _adapterService
            .GetBySetCodeAsync(xfrEntity, cancellationToken)
            .ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<IEnumerable<ISealedProductOufEntity>>(
                new SealedProductsAggregatorException($"Failed to retrieve sealed products for set '{input.SetCode}'", response.OuterException));
        }

        IEnumerable<ISealedProductOufEntity> oufEntities = await Task.WhenAll(
            response.ResponseData.Select(ext => _extToOufMapper.Map(ext))).ConfigureAwait(false);

        return new SuccessOperationResponse<IEnumerable<ISealedProductOufEntity>>(oufEntities);
    }
}
