using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
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
    private readonly IUserSealedProductExtToSealedProductOufMapper _oufMapper;

    public AddUserSealedProductAggregatorService(ILogger logger) : this(
        new UserSealedProductsAdapterService(logger),
        new UserSealedProductExtToSealedProductOufMapper())
    { }

    private AddUserSealedProductAggregatorService(
        IUserSealedProductsCommandAdapter adapter,
        IUserSealedProductExtToSealedProductOufMapper oufMapper)
    {
        _adapter = adapter;
        _oufMapper = oufMapper;
    }

    public async Task<IOperationResponse<List<ISealedProductOufEntity>>> Execute(IAddUserSealedProductItrEntity input)
    {
        IUserSealedProductXfrEntity xfrEntity = new UserSealedProductXfrEntity
        {
            UserId = input.UserId,
            ProductUuid = input.ProductUuid,
            SetId = input.SetId,
            CountDelta = input.CountDelta
        };

        IOperationResponse<UserSealedProductExtEntity> extResponse = await _adapter.AddUserSealedProductAsync(xfrEntity).ConfigureAwait(false);

        if (extResponse.IsFailure)
        {
            return new FailureOperationResponse<List<ISealedProductOufEntity>>(extResponse.OuterException);
        }

        ISealedProductOufEntity oufEntity = await _oufMapper.Map(extResponse.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<List<ISealedProductOufEntity>>([oufEntity]);
    }

    private sealed class UserSealedProductXfrEntity : IUserSealedProductXfrEntity
    {
        public required string UserId { get; init; }
        public required string ProductUuid { get; init; }
        public required string SetId { get; init; }
        public required int CountDelta { get; init; }
    }
}
