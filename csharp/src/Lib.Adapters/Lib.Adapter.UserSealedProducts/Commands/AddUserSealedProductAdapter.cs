using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.Mappers.UserSealedProducts;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Gophers;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Janitors;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Scribes;
using Lib.Adapter.UserSealedProducts.Apis.Entities;
using Lib.Adapter.UserSealedProducts.Exceptions;
using Lib.Cosmos.Apis.Ids;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.UserSealedProducts.Commands;

internal sealed class AddUserSealedProductAdapter : IAddUserSealedProductAdapter
{
    private readonly ICosmosGopher _sealedProductsGopher;
    private readonly ICosmosGopher _userSealedProductsGopher;
    private readonly ICosmosScribe _userSealedProductsScribe;
    private readonly ICosmosJanitor _userSealedProductsJanitor;
    private readonly IUserSealedProductReadPointMapper _readPointMapper;

    public AddUserSealedProductAdapter(ILogger logger)
        : this(
            new SealedProductsGopher(logger),
            new UserSealedProductsGopher(logger),
            new UserSealedProductsScribe(logger),
            new UserSealedProductsJanitor(logger),
            new UserSealedProductReadPointMapper())
    { }

    private AddUserSealedProductAdapter(
        ICosmosGopher sealedProductsGopher,
        ICosmosGopher userSealedProductsGopher,
        ICosmosScribe userSealedProductsScribe,
        ICosmosJanitor userSealedProductsJanitor,
        IUserSealedProductReadPointMapper readPointMapper)
    {
        _sealedProductsGopher = sealedProductsGopher;
        _userSealedProductsGopher = userSealedProductsGopher;
        _userSealedProductsScribe = userSealedProductsScribe;
        _userSealedProductsJanitor = userSealedProductsJanitor;
        _readPointMapper = readPointMapper;
    }

    public async Task<IOperationResponse<UserSealedProductExtEntity>> Execute(
        [NotNull] IUserSealedProductXfrEntity input, CancellationToken cancellationToken)
    {
        ReadPointItem productReadPoint = await _readPointMapper.Map(input.ProductUuid, input.SetId).ConfigureAwait(false);

        OpResponse<SealedProductExtEntity> productResponse =
            await _sealedProductsGopher.ReadAsync<SealedProductExtEntity>(productReadPoint, cancellationToken).ConfigureAwait(false);

        if (productResponse.IsNotSuccessful())
        {
            return new FailureOperationResponse<UserSealedProductExtEntity>(
                new UserSealedProductsAdapterException(
                    $"Failed to fetch sealed product details: {productResponse.StatusCode}"));
        }

        SealedProductExtEntity product = productResponse.Value;

        ReadPointItem userProductReadPoint = new()
        {
            Id = new ProvidedCosmosItemId(input.ProductUuid),
            Partition = new ProvidedPartitionKeyValue(input.UserId)
        };
        OpResponse<UserSealedProductExtEntity> existingResponse =
            await _userSealedProductsGopher.ReadAsync<UserSealedProductExtEntity>(userProductReadPoint, cancellationToken).ConfigureAwait(false);

        int currentCount = existingResponse.IsSuccessful() ? existingResponse.Value.Count : 0;
        int newCount = currentCount + input.CountDelta;

        if (newCount < 1)
        {
            if (existingResponse.IsSuccessful())
            {
                DeletePointItem deletePoint = new()
                {
                    Id = new ProvidedCosmosItemId(input.ProductUuid),
                    Partition = new ProvidedPartitionKeyValue(input.UserId)
                };
                OpResponse<UserSealedProductExtEntity> deleteResponse =
                    await _userSealedProductsJanitor.DeleteAsync<UserSealedProductExtEntity>(deletePoint, cancellationToken).ConfigureAwait(false);

                if (deleteResponse.IsNotSuccessful())
                {
                    return new FailureOperationResponse<UserSealedProductExtEntity>(
                        new UserSealedProductsAdapterException(
                            $"Failed to delete user sealed product: {deleteResponse.StatusCode}"));
                }
            }

            UserSealedProductExtEntity deletedResult = new()
            {
                UserId = input.UserId,
                ProductUuid = input.ProductUuid,
                SetId = input.SetId,
                Count = 0,
                ProductName = product.Name,
                SetCode = product.SetCode,
                SetName = product.SetName,
                Category = product.Category,
                Subtype = product.Subtype,
                ImageUrl = product.ImageUrl,
                ReleaseDate = product.ReleaseDate,
                TcgplayerProductId = product.TcgplayerProductId,
                PurchaseUrlTcgplayer = product.PurchaseUrlTcgplayer,
                PurchaseUrlCardmarket = product.PurchaseUrlCardmarket,
                PurchaseUrlCardKingdom = product.PurchaseUrlCardKingdom,
                CardCount = product.CardCount,
                UpdatedAt = DateTime.UtcNow.ToString("O")
            };
            return new SuccessOperationResponse<UserSealedProductExtEntity>(deletedResult);
        }

        UserSealedProductExtEntity itemToUpsert = new()
        {
            UserId = input.UserId,
            ProductUuid = input.ProductUuid,
            SetId = input.SetId,
            Count = newCount,
            ProductName = product.Name,
            SetCode = product.SetCode,
            SetName = product.SetName,
            Category = product.Category,
            Subtype = product.Subtype,
            ImageUrl = product.ImageUrl,
            ReleaseDate = product.ReleaseDate,
            TcgplayerProductId = product.TcgplayerProductId,
            PurchaseUrlTcgplayer = product.PurchaseUrlTcgplayer,
            PurchaseUrlCardmarket = product.PurchaseUrlCardmarket,
            PurchaseUrlCardKingdom = product.PurchaseUrlCardKingdom,
            CardCount = product.CardCount,
            UpdatedAt = DateTime.UtcNow.ToString("O")
        };

        OpResponse<UserSealedProductExtEntity> upsertResponse =
            await _userSealedProductsScribe.UpsertAsync(itemToUpsert, cancellationToken).ConfigureAwait(false);

        if (upsertResponse.IsNotSuccessful())
        {
            return new FailureOperationResponse<UserSealedProductExtEntity>(
                new UserSealedProductsAdapterException(
                    $"Failed to add user sealed product: {upsertResponse.StatusCode}"));
        }

        return new SuccessOperationResponse<UserSealedProductExtEntity>(upsertResponse.Value);
    }
}
