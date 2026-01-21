using System.Threading.Tasks;
using Lib.Adapter.UserSealedProducts.Apis.Entities;
using Lib.Adapter.UserSealedProducts.Commands;
using Lib.Shared.DataModels.Entities.Itrs.UserSealedProducts;
using Lib.Shared.DataModels.Entities.Oufs.UserSealedProducts;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.UserSealedProducts.Commands;

internal sealed class AddUserSealedProductAggregator : IAddUserSealedProductAggregator
{
    private readonly IAddUserSealedProductAdapter _adapter;

    public AddUserSealedProductAggregator(ILogger logger) : this(
        new AddUserSealedProductAdapter(logger))
    { }

    private AddUserSealedProductAggregator(IAddUserSealedProductAdapter adapter) =>
        _adapter = adapter;

    public async Task<IOperationResponse<IUserSealedProductOufEntity>> Execute(IAddUserSealedProductItrEntity input)
    {
        IUserSealedProductXfrEntity xfrEntity = new UserSealedProductXfrEntity
        {
            UserId = input.UserId,
            ProductUuid = input.ProductUuid,
            SetId = input.SetId,
            CountDelta = input.CountDelta,
            ProductName = string.Empty,
            SetCode = string.Empty,
            Category = string.Empty,
            ImageUrl = string.Empty
        };

        return await _adapter.Execute(xfrEntity).ConfigureAwait(false);
    }

    private sealed class UserSealedProductXfrEntity : IUserSealedProductXfrEntity
    {
        public required string UserId { get; init; }
        public required string ProductUuid { get; init; }
        public required string SetId { get; init; }
        public required int CountDelta { get; init; }
        public required string ProductName { get; init; }
        public required string SetCode { get; init; }
        public required string Category { get; init; }
        public required string ImageUrl { get; init; }
    }
}
