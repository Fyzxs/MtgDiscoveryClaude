using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserSealedProducts;
using Lib.Adapter.UserSealedProducts.Apis;
using Lib.Aggregator.UserSealedProducts.Queries.Mappers;
using Lib.Shared.DataModels.Entities.Itrs.UserSealedProducts;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.UserSealedProducts.Queries;

internal sealed class UserSealedProductsByUserIdAggregatorService : IUserSealedProductsByUserIdAggregatorService
{
    private readonly IUserSealedProductsQueryAdapter _adapter;
    private readonly IUserSealedProductItrMapper _itrMapper;

    public UserSealedProductsByUserIdAggregatorService(ILogger logger) : this(
        new UserSealedProductsAdapterService(logger),
        new UserSealedProductItrMapper())
    { }

    private UserSealedProductsByUserIdAggregatorService(
        IUserSealedProductsQueryAdapter adapter,
        IUserSealedProductItrMapper itrMapper)
    {
        _adapter = adapter;
        _itrMapper = itrMapper;
    }

    public async Task<IOperationResponse<IEnumerable<IUserSealedProductItrEntity>>> Execute(IUserIdItrEntity input, CancellationToken cancellationToken)
    {
        IOperationResponse<IEnumerable<UserSealedProductExtEntity>> extResponse = await _adapter.UserSealedProductsByUserIdAsync(input.UserId, cancellationToken).ConfigureAwait(false);

        if (extResponse.IsFailure)
        {
            return new FailureOperationResponse<IEnumerable<IUserSealedProductItrEntity>>(extResponse.OuterException);
        }

        List<IUserSealedProductItrEntity> itrEntities = [];
        foreach (UserSealedProductExtEntity extEntity in extResponse.ResponseData)
        {
            IUserSealedProductItrEntity itrEntity = await _itrMapper.Map(extEntity).ConfigureAwait(false);
            itrEntities.Add(itrEntity);
        }

        return new SuccessOperationResponse<IEnumerable<IUserSealedProductItrEntity>>(itrEntities);
    }
}
