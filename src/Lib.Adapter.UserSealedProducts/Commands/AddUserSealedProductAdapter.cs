using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.Mappers.UserSealedProducts;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Gophers;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Scribes;
using Lib.Adapter.UserSealedProducts.Apis.Entities;
using Lib.Adapter.UserSealedProducts.Exceptions;
using Lib.Adapter.UserSealedProducts.Mappers;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.DataModels.Entities.Oufs.UserSealedProducts;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.UserSealedProducts.Commands;

/// <summary>
/// Adds or updates a sealed product in a user's collection with intelligent merging logic.
/// Fetches product details for denormalization and uses the Scribe's merge functionality.
/// </summary>
public sealed class AddUserSealedProductAdapter : IAddUserSealedProductAdapter
{
    private readonly ICosmosGopher _sealedProductsGopher;
    private readonly UserSealedProductsScribe _userSealedProductsScribe;
    private readonly IUserSealedProductReadPointMapper _readPointMapper;
    private readonly IUserSealedProductOufMapper _oufMapper;

    public AddUserSealedProductAdapter(ILogger logger)
        : this(
            new SealedProductsGopher(logger),
            new UserSealedProductsScribe(logger),
            new UserSealedProductReadPointMapper(),
            new UserSealedProductOufMapper())
    { }

    private AddUserSealedProductAdapter(
        ICosmosGopher sealedProductsGopher,
        UserSealedProductsScribe userSealedProductsScribe,
        IUserSealedProductReadPointMapper readPointMapper,
        IUserSealedProductOufMapper oufMapper)
    {
        _sealedProductsGopher = sealedProductsGopher;
        _userSealedProductsScribe = userSealedProductsScribe;
        _readPointMapper = readPointMapper;
        _oufMapper = oufMapper;
    }

    public async Task<IOperationResponse<IUserSealedProductOufEntity>> Execute(
        [NotNull] IUserSealedProductXfrEntity input)
    {
        ReadPointItem productReadPoint = await _readPointMapper.Map(input.ProductUuid, input.SetId).ConfigureAwait(false);

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

        IUserSealedProductOufEntity oufEntity = await _oufMapper.Map(result).ConfigureAwait(false);

        return new SuccessOperationResponse<IUserSealedProductOufEntity>(oufEntity);
    }
}
