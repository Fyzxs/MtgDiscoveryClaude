using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.Mappers.UserSealedProducts;
using Lib.Adapter.Scryfall.Cosmos.Cosmos.Containers;
using Lib.Cosmos.Apis.Ids;
using Lib.Cosmos.Apis.Operators;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Scribes;

public sealed class UserSealedProductsScribe : CosmosScribe
{
    private readonly ICosmosGopher _gopher;
    private readonly ICosmosJanitor _janitor;
    private readonly IUserSealedProductReadPointMapper _readPointMapper;

    public UserSealedProductsScribe(ILogger logger)
        : base(new UserSealedProductsCosmosContainer(logger))
    {
        _gopher = new Apis.Operators.Gophers.UserSealedProductsGopher(logger);
        _janitor = new Apis.Operators.Janitors.UserSealedProductsJanitor(logger);
        _readPointMapper = new UserSealedProductReadPointMapper();
    }

    public async Task<OpResponse<UserSealedProductExtEntity>> UpsertWithMergeAsync(
        [NotNull] UserSealedProductExtEntity input,
        int countDelta)
    {
        ReadPointItem readPoint = await _readPointMapper.Map(input.ProductUuid, input.UserId).ConfigureAwait(false);

        OpResponse<UserSealedProductExtEntity> existing =
            await _gopher.ReadAsync<UserSealedProductExtEntity>(readPoint).ConfigureAwait(false);

        int newCount = (existing.IsSuccessful() && existing.Value is not null)
            ? existing.Value.Count + countDelta
            : countDelta;

        if (newCount < 1)
        {
            DeletePointItem deletePoint = new DeletePointItem
            {
                Id = new ProvidedCosmosItemId(input.ProductUuid),
                Partition = new ProvidedPartitionKeyValue(input.UserId)
            };
            return await _janitor.DeleteAsync<UserSealedProductExtEntity>(deletePoint).ConfigureAwait(false);
        }

        UserSealedProductExtEntity updated = new UserSealedProductExtEntity
        {
            UserId = input.UserId,
            ProductUuid = input.ProductUuid,
            Count = newCount,
            ProductName = input.ProductName,
            SetCode = input.SetCode,
            Category = input.Category,
            ImageUrl = input.ImageUrl,
            UpdatedAt = DateTime.UtcNow.ToString("O")
        };

        return await UpsertAsync(updated).ConfigureAwait(false);
    }
}
