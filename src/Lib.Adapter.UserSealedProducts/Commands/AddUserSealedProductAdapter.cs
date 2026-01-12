using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Gophers;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Scribes;
using Lib.Adapter.UserSealedProducts.Apis.Entities;
using Lib.Adapter.UserSealedProducts.Exceptions;
using Lib.Cosmos.Apis.Ids;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.DataModels.Entities.Oufs.UserSealedProducts;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.UserSealedProducts.Commands;

/// <summary>
/// Adds or updates a sealed product in a user's collection with intelligent merging logic.
/// Fetches product details for denormalization and uses the Scribe's merge functionality.
/// </summary>
internal sealed class AddUserSealedProductAdapter : IAddUserSealedProductAdapter
{
    private readonly ICosmosGopher _sealedProductsGopher;
    private readonly UserSealedProductsScribe _userSealedProductsScribe;

    public AddUserSealedProductAdapter(ILogger logger)
        : this(new SealedProductsGopher(logger), new UserSealedProductsScribe(logger))
    { }

    private AddUserSealedProductAdapter(
        ICosmosGopher sealedProductsGopher,
        UserSealedProductsScribe userSealedProductsScribe)
    {
        _sealedProductsGopher = sealedProductsGopher;
        _userSealedProductsScribe = userSealedProductsScribe;
    }

    public async Task<IOperationResponse<IUserSealedProductOufEntity>> Execute(
        [NotNull] IUserSealedProductXfrEntity input)
    {
        ReadPointItem productReadPoint = new ReadPointItem
        {
            Id = new ProvidedCosmosItemId(input.ProductUuid),
            Partition = new ProvidedPartitionKeyValue(input.SetId)
        };

        OpResponse<SealedProductExtEntity> productResponse =
            await _sealedProductsGopher.ReadAsync<SealedProductExtEntity>(productReadPoint).ConfigureAwait(false);

        if (productResponse.IsNotSuccessful())
        {
            return new FailureOperationResponse<IUserSealedProductOufEntity>(
                new UserSealedProductsAdapterException(
                    $"Failed to fetch sealed product details: {productResponse.StatusCode}"));
        }

        SealedProductExtEntity product = productResponse.Value;

        UserSealedProductExtEntity itemToUpsert = new UserSealedProductExtEntity
        {
            UserId = input.UserId,
            ProductUuid = input.ProductUuid,
            Count = 0,
            ProductName = product.Name,
            SetCode = product.SetCode,
            Category = product.Category,
            ImageUrl = product.ImageUrl,
            UpdatedAt = DateTime.UtcNow.ToString("O")
        };

        OpResponse<UserSealedProductExtEntity> upsertResponse =
            await _userSealedProductsScribe.UpsertWithMergeAsync(itemToUpsert, input.CountDelta).ConfigureAwait(false);

        if (upsertResponse.IsNotSuccessful())
        {
            return new FailureOperationResponse<IUserSealedProductOufEntity>(
                new UserSealedProductsAdapterException(
                    $"Failed to add user sealed product: {upsertResponse.StatusCode}"));
        }

        UserSealedProductExtEntity result = upsertResponse.Value;

        IUserSealedProductOufEntity oufEntity = new UserSealedProductOufEntity
        {
            ProductUuid = result.ProductUuid,
            Count = result.Count
        };

        return new SuccessOperationResponse<IUserSealedProductOufEntity>(oufEntity);
    }
}

internal sealed class UserSealedProductOufEntity : IUserSealedProductOufEntity
{
    public string ProductUuid { get; init; }
    public int Count { get; init; }
}
