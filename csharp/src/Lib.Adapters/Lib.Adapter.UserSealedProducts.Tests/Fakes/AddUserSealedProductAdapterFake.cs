#nullable enable
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserSealedProducts;
using Lib.Adapter.UserSealedProducts.Apis.Entities;
using Lib.Adapter.UserSealedProducts.Commands;
using Lib.Adapter.UserSealedProducts.Exceptions;
using Lib.Shared.Invocation.Operations;

namespace Lib.Adapter.UserSealedProducts.Tests.Fakes;

public sealed class AddUserSealedProductAdapterFake : IAddUserSealedProductAdapter
{
    public IOperationResponse<UserSealedProductExtEntity> ExecuteResult { get; set; } =
        new SuccessOperationResponse<UserSealedProductExtEntity>(new UserSealedProductExtEntity
        {
            UserId = "user123",
            ProductUuid = "product-uuid-456",
            SetId = "set789",
            Count = 1
        });

    public bool ShouldReturnFailure { get; set; }

    public int ExecuteInvokeCount { get; private set; }

    public IUserSealedProductXfrEntity? LastExecuteInput { get; private set; }

    public Task<IOperationResponse<UserSealedProductExtEntity>> Execute(
        IUserSealedProductXfrEntity input,
        CancellationToken cancellationToken)
    {
        ExecuteInvokeCount++;
        LastExecuteInput = input;

        if (ShouldReturnFailure)
        {
            return Task.FromResult<IOperationResponse<UserSealedProductExtEntity>>(
                new FailureOperationResponse<UserSealedProductExtEntity>(new UserSealedProductsAdapterException("Adapter failed")));
        }

        return Task.FromResult(ExecuteResult);
    }
}
