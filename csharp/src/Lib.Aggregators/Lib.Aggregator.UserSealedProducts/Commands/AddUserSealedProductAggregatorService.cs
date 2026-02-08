using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserSealedProducts;
using Lib.Adapter.UserSealedProducts.Apis;
using Lib.Adapter.UserSealedProducts.Apis.Entities;
using Lib.Aggregator.UserSealedProducts.Commands.Mappers;
using Lib.Shared.DataModels.Entities.Itrs.UserSealedProducts;
using Lib.Shared.DataModels.Entities.Oufs.SealedProducts;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.UserSealedProducts.Commands;

internal sealed class AddUserSealedProductAggregatorService : IAddUserSealedProductAggregatorService
{
    private readonly IUserSealedProductsCommandAdapter _adapter;
    private readonly IAddUserSealedProductItrToXfrMapper _itrToXfrMapper;
    private readonly IUserSealedProductExtToOufMapper _oufMapper;

    public AddUserSealedProductAggregatorService(ILogger logger) : this(
        new UserSealedProductsAdapterService(logger),
        new AddUserSealedProductItrToXfrMapper(),
        new UserSealedProductExtToOufMapper())
    { }

    private AddUserSealedProductAggregatorService(
        IUserSealedProductsCommandAdapter adapter,
        IAddUserSealedProductItrToXfrMapper itrToXfrMapper,
        IUserSealedProductExtToOufMapper oufMapper)
    {
        _adapter = adapter;
        _itrToXfrMapper = itrToXfrMapper;
        _oufMapper = oufMapper;
    }

    public async Task<IOperationResponse<List<ISealedProductOufEntity>>> Execute(IAddUserSealedProductItrEntity input, CancellationToken cancellationToken)
    {
        IUserSealedProductXfrEntity xfrEntity = await _itrToXfrMapper.Map(input).ConfigureAwait(false);

        IOperationResponse<UserSealedProductExtEntity> extResponse = await _adapter.AddUserSealedProductAsync(xfrEntity, cancellationToken).ConfigureAwait(false);

        if (extResponse.IsFailure)
        {
            return new FailureOperationResponse<List<ISealedProductOufEntity>>(extResponse.OuterException);
        }

        ISealedProductOufEntity oufEntity = await _oufMapper.Map(extResponse.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<List<ISealedProductOufEntity>>([oufEntity]);
    }
}
