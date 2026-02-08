using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserSealedProducts;
using Lib.Adapter.UserSealedProducts.Apis;
using Lib.Aggregator.UserSealedProducts.Queries.Mappers;
using Lib.Shared.DataModels.Entities.Itrs.UserSealedProducts;
using Lib.Shared.DataModels.Entities.Oufs.UserSealedProducts;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.UserSealedProducts.Queries;

internal sealed class UserSealedProductsByUserIdAggregatorService : IUserSealedProductsByUserIdAggregatorService
{
    private readonly IUserSealedProductsQueryAdapter _adapter;
    private readonly IUserSealedProductExtToOufMapper _extToOufMapper;

    public UserSealedProductsByUserIdAggregatorService(ILogger logger) : this(
        new UserSealedProductsAdapterService(logger),
        new UserSealedProductExtToOufMapper())
    { }

    private UserSealedProductsByUserIdAggregatorService(
        IUserSealedProductsQueryAdapter adapter,
        IUserSealedProductExtToOufMapper extToOufMapper)
    {
        _adapter = adapter;
        _extToOufMapper = extToOufMapper;
    }

    public async Task<IOperationResponse<IEnumerable<IUserSealedProductOufEntity>>> Execute(IUserIdItrEntity input, CancellationToken cancellationToken)
    {
        IOperationResponse<IEnumerable<UserSealedProductExtEntity>> extResponse = await _adapter.UserSealedProductsByUserIdAsync(input.UserId, cancellationToken).ConfigureAwait(false);

        if (extResponse.IsFailure)
        {
            return new FailureOperationResponse<IEnumerable<IUserSealedProductOufEntity>>(extResponse.OuterException);
        }

        IEnumerable<IUserSealedProductOufEntity> oufEntities = await Task.WhenAll(
            extResponse.ResponseData.Select(ext => _extToOufMapper.Map(ext))).ConfigureAwait(false);

        return new SuccessOperationResponse<IEnumerable<IUserSealedProductOufEntity>>(oufEntities);
    }
}
