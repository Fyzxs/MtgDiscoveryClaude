using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Gophers;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions.Entities;
using Lib.Adapter.SealedProducts.Apis.Entities;
using Lib.Adapter.SealedProducts.Exceptions;
using Lib.Adapter.SealedProducts.Queries.Mappers;
using Lib.Cosmos.Apis.Ids;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.DataModels.Entities.Oufs.SealedProducts;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.SealedProducts.Apis.Queries;

internal sealed class SealedProductsBySetCodeAdapter : ISealedProductsBySetCodeAdapter
{
    private readonly ICosmosGopher _setCodeIndexGopher;
    private readonly ICosmosInquisition<SealedProductsBySetIdArgs> _sealedProductsInquisition;
    private readonly ISealedProductExtToOufMapper _mapper;

    public SealedProductsBySetCodeAdapter(ILogger logger) : this(
        new ScryfallSetCodeIndexGopher(logger),
        new SealedProductsBySetIdInquisition(logger),
        new SealedProductExtToOufMapper())
    { }

    private SealedProductsBySetCodeAdapter(
        ICosmosGopher setCodeIndexGopher,
        ICosmosInquisition<SealedProductsBySetIdArgs> sealedProductsInquisition,
        ISealedProductExtToOufMapper mapper)
    {
        _setCodeIndexGopher = setCodeIndexGopher;
        _sealedProductsInquisition = sealedProductsInquisition;
        _mapper = mapper;
    }

    public async Task<IOperationResponse<IEnumerable<ISealedProductOufEntity>>> Execute(
        ISealedProductsBySetCodeXfrEntity input,
        CancellationToken cancellationToken)
    {
        string setCodeValue = input.SetCode;
        ReadPointItem readPoint = new()
        {
            Id = new ProvidedCosmosItemId(setCodeValue),
            Partition = new ProvidedPartitionKeyValue(setCodeValue)
        };

        OpResponse<ScryfallSetCodeIndexExtEntity> indexResponse = await _setCodeIndexGopher
            .ReadAsync<ScryfallSetCodeIndexExtEntity>(readPoint)
            .ConfigureAwait(false);

        if (indexResponse.IsNotSuccessful() || indexResponse.Value == null)
        {
            return new FailureOperationResponse<IEnumerable<ISealedProductOufEntity>>(
                new SealedProductsAdapterException($"Set code '{setCodeValue}' not found"));
        }

        string setId = indexResponse.Value.SetId;

        SealedProductsBySetIdArgs args = new() { SetId = setId };

        OpResponse<IEnumerable<SealedProductExtEntity>> productsResponse = await _sealedProductsInquisition
            .QueryAsync<SealedProductExtEntity>(args, cancellationToken)
            .ConfigureAwait(false);

        if (productsResponse.IsNotSuccessful())
        {
            return new FailureOperationResponse<IEnumerable<ISealedProductOufEntity>>(
                new SealedProductsAdapterException($"Failed to retrieve sealed products for set '{setCodeValue}'", productsResponse.Exception()));
        }

        IEnumerable<ISealedProductOufEntity> mappedProducts = await Task.WhenAll(
            productsResponse.Value.Select(p => _mapper.Map(p))).ConfigureAwait(false);

        return new SuccessOperationResponse<IEnumerable<ISealedProductOufEntity>>(mappedProducts);
    }
}
