using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.SealedProducts;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.SetCodeIndex;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Gophers;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions.Entities;
using Lib.Adapter.SealedProducts.Apis.Entities;
using Lib.Adapter.SealedProducts.Exceptions;
using Lib.Cosmos.Apis.Ids;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.SealedProducts.Queries;

internal sealed class SealedProductsBySetCodeAdapter : ISealedProductsBySetCodeAdapter
{
    private readonly ICosmosGopher _setCodeIndexGopher;
    private readonly ICosmosInquisition<SealedProductsBySetIdArgs> _sealedProductsInquisition;

    public SealedProductsBySetCodeAdapter(ILogger logger) : this(
        new ScryfallSetCodeIndexGopher(logger),
        new SealedProductsBySetIdInquisition(logger))
    { }

    private SealedProductsBySetCodeAdapter(
        ICosmosGopher setCodeIndexGopher,
        ICosmosInquisition<SealedProductsBySetIdArgs> sealedProductsInquisition)
    {
        _setCodeIndexGopher = setCodeIndexGopher;
        _sealedProductsInquisition = sealedProductsInquisition;
    }

    public async Task<IOperationResponse<IEnumerable<SealedProductExtEntity>>> Execute(
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
            .ReadAsync<ScryfallSetCodeIndexExtEntity>(readPoint, cancellationToken)
            .ConfigureAwait(false);

        if (indexResponse.IsNotSuccessful() || indexResponse.Value == null)
        {
            return new FailureOperationResponse<IEnumerable<SealedProductExtEntity>>(
                new SealedProductsAdapterException($"Set code '{setCodeValue}' not found"));
        }

        string setId = indexResponse.Value.SetId;

        SealedProductsBySetIdArgs args = new() { SetId = setId };

        OpResponse<IEnumerable<SealedProductExtEntity>> productsResponse = await _sealedProductsInquisition
            .QueryAsync<SealedProductExtEntity>(args, cancellationToken)
            .ConfigureAwait(false);

        if (productsResponse.IsNotSuccessful())
        {
            return new FailureOperationResponse<IEnumerable<SealedProductExtEntity>>(
                new SealedProductsAdapterException($"Failed to retrieve sealed products for set '{setCodeValue}'", productsResponse.Exception()));
        }

        return new SuccessOperationResponse<IEnumerable<SealedProductExtEntity>>(productsResponse.Value);
    }
}
