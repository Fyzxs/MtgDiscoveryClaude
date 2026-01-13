using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitors;
using Lib.Adapter.UserSealedProducts.Exceptions;
using Lib.Adapter.UserSealedProducts.Mappers;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.DataModels.Entities.Itrs.UserSealedProducts;
using Lib.Shared.Invocation.Operations;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.UserSealedProducts.Queries;

internal sealed class UserSealedProductsByUserIdAdapter : IUserSealedProductsByUserIdAdapter
{
    private readonly ICosmosInquisitor _userSealedProductsInquisitor;
    private readonly IUserSealedProductItrMapper _mapper;

    public UserSealedProductsByUserIdAdapter(ILogger logger)
        : this(new UserSealedProductsInquisitor(logger), new UserSealedProductItrMapper())
    { }

    private UserSealedProductsByUserIdAdapter(
        ICosmosInquisitor userSealedProductsInquisitor,
        IUserSealedProductItrMapper mapper)
    {
        _userSealedProductsInquisitor = userSealedProductsInquisitor;
        _mapper = mapper;
    }

    public async Task<IOperationResponse<IEnumerable<IUserSealedProductItrEntity>>> Execute(
        [NotNull] UserSealedProductsByUserIdXfrArgs args)
    {
        string userId = args.UserId;

        QueryDefinition query = new QueryDefinition("SELECT * FROM c WHERE c.partition = @userId")
            .WithParameter("@userId", userId);

        PartitionKey partitionKey = new PartitionKey(userId);

        OpResponse<IEnumerable<UserSealedProductExtEntity>> response =
            await _userSealedProductsInquisitor.QueryAsync<UserSealedProductExtEntity>(
                query,
                partitionKey).ConfigureAwait(false);

        if (response.IsNotSuccessful())
        {
            return new FailureOperationResponse<IEnumerable<IUserSealedProductItrEntity>>(
                new UserSealedProductsAdapterException(
                    $"Failed to retrieve sealed products for user [{userId}]",
                    response.Exception()));
        }

        List<IUserSealedProductItrEntity> itrEntities = [];
        foreach (UserSealedProductExtEntity extEntity in response.Value)
        {
            IUserSealedProductItrEntity itrEntity = await _mapper.Map(extEntity).ConfigureAwait(false);
            itrEntities.Add(itrEntity);
        }

        return new SuccessOperationResponse<IEnumerable<IUserSealedProductItrEntity>>(itrEntities);
    }
}
