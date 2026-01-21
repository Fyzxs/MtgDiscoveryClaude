using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.UserSealedProducts.Apis;
using Lib.Adapter.UserSealedProducts.Apis.Entities;
using Lib.Aggregator.UserSealedProducts.Commands.Mappers;
using Lib.Shared.DataModels.Entities.Itrs.UserSealedProducts;
using Lib.Shared.DataModels.Entities.Oufs.UserSealedProducts;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.UserSealedProducts.Commands;

internal sealed class AddUserSealedProductAggregatorService : IAddUserSealedProductAggregatorService
{
    private readonly IUserSealedProductsCommandAdapter _adapter;
    private readonly IUserSealedProductOufMapper _oufMapper;

    public AddUserSealedProductAggregatorService(ILogger logger) : this(
        new UserSealedProductsAdapterService(logger),
        new UserSealedProductOufMapper())
    { }

    private AddUserSealedProductAggregatorService(
        IUserSealedProductsCommandAdapter adapter,
        IUserSealedProductOufMapper oufMapper)
    {
        _adapter = adapter;
        _oufMapper = oufMapper;
    }

    public async Task<IOperationResponse<IUserSealedProductOufEntity>> Execute(IAddUserSealedProductItrEntity input)
    {
        IUserSealedProductXfrEntity xfrEntity = new UserSealedProductXfrEntity
        {
            CollectionId = input.CollectionId,
            ProductUuid = input.ProductUuid,
            SetId = input.SetId,
            CountDelta = input.CountDelta,
            ProductName = string.Empty,
            SetCode = string.Empty,
            Category = string.Empty,
            ImageUrl = string.Empty
        };

        IOperationResponse<UserSealedProductExtEntity> extResponse = await _adapter.AddUserSealedProductAsync(xfrEntity).ConfigureAwait(false);

        if (extResponse.IsFailure)
        {
            return new FailureOperationResponse<IUserSealedProductOufEntity>(extResponse.OuterException);
        }

        IUserSealedProductOufEntity oufEntity = await _oufMapper.Map(extResponse.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<IUserSealedProductOufEntity>(oufEntity);
    }

    private sealed class UserSealedProductXfrEntity : IUserSealedProductXfrEntity
    {
        public required string CollectionId { get; init; }
        public required string ProductUuid { get; init; }
        public required string SetId { get; init; }
        public required int CountDelta { get; init; }
        public required string ProductName { get; init; }
        public required string SetCode { get; init; }
        public required string Category { get; init; }
        public required string ImageUrl { get; init; }
    }
}
