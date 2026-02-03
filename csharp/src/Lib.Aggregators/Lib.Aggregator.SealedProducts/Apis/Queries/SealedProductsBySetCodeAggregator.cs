using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.SealedProducts.Apis;
using Lib.Adapter.SealedProducts.Apis.Entities;
using Lib.Aggregator.SealedProducts.Exceptions;
using Lib.Aggregator.SealedProducts.Queries.Mappers;
using Lib.Shared.DataModels.Entities.Itrs.SealedProducts;
using Lib.Shared.DataModels.Entities.Oufs.SealedProducts;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.SealedProducts.Apis.Queries;

internal sealed class SealedProductsBySetCodeAggregator : ISealedProductsBySetCodeAggregator
{
    private readonly ISealedProductsAdapterService _adapterService;
    private readonly ISealedProductsBySetCodeItrToXfrMapper _mapper;

    public SealedProductsBySetCodeAggregator(ILogger logger) : this(
        new SealedProductsAdapterService(logger),
        new SealedProductsBySetCodeItrToXfrMapper())
    { }

    private SealedProductsBySetCodeAggregator(
        ISealedProductsAdapterService adapterService,
        ISealedProductsBySetCodeItrToXfrMapper mapper)
    {
        _adapterService = adapterService;
        _mapper = mapper;
    }

    public async Task<IOperationResponse<IEnumerable<ISealedProductOufEntity>>> Execute(
        ISealedProductsBySetCodeItrEntity input,
        CancellationToken cancellationToken)
    {
        ISealedProductsBySetCodeXfrEntity xfrEntity = await _mapper.Map(input).ConfigureAwait(false);
        IOperationResponse<IEnumerable<ISealedProductOufEntity>> response = await _adapterService
            .GetBySetCodeAsync(xfrEntity, cancellationToken)
            .ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<IEnumerable<ISealedProductOufEntity>>(
                new SealedProductsAggregatorException($"Failed to retrieve sealed products for set '{input.SetCode}'", response.OuterException));
        }

        return new SuccessOperationResponse<IEnumerable<ISealedProductOufEntity>>(response.ResponseData);
    }
}
