using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserSealedProducts;
using Lib.Adapter.UserSealedProducts.Apis.Entities;
using Lib.Adapter.UserSealedProducts.Commands;
using Lib.Adapter.UserSealedProducts.Exceptions;
using Lib.Adapter.UserSealedProducts.Tests.Fakes;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;
using TestConvenience.Core.Fakes;
using TestConvenience.Core.Reflection;

namespace Lib.Adapter.UserSealedProducts.Tests.Commands;

[TestClass]
public sealed class UserSealedProductsCommandAdapterTests
{
    [TestMethod, TestCategory("unit")]
    public void Constructor_WithLogger_CreatesInstance()
    {
        // Arrange
        ILogger logger = new LoggerFake();

        // Act
        UserSealedProductsCommandAdapter actual = new(logger);

        // Assert
        actual.Should().NotBeNull();
    }

    [TestMethod, TestCategory("unit")]
    public async Task AddUserSealedProductAsync_WithAddAdapter_DelegatesToAdapter()
    {
        // Arrange
        AddUserSealedProductAdapterFake fakeAdapter = new()
        {
            ExecuteResult = new SuccessOperationResponse<UserSealedProductExtEntity>(new UserSealedProductExtEntity
            {
                UserId = "user123",
                ProductUuid = "product-uuid-456",
                SetId = "set789",
                Count = 1
            })
        };

        UserSealedProductsCommandAdapter adapter = new InstanceWrapper(fakeAdapter);

        IUserSealedProductXfrEntity input = new UserSealedProductXfrEntityFake
        {
            UserId = "user123",
            ProductUuid = "product-uuid-456",
            SetId = "set789",
            CountDelta = 1
        };

        // Act
        IOperationResponse<UserSealedProductExtEntity> actual = await adapter.AddUserSealedProductAsync(input, CancellationToken.None).ConfigureAwait(false);

        // Assert
        actual.Should().NotBeNull();
        actual.IsSuccess.Should().BeTrue();
        fakeAdapter.ExecuteInvokeCount.Should().Be(1);
    }

    [TestMethod, TestCategory("unit")]
    public async Task AddUserSealedProductAsync_WithValidInput_ReturnsSuccessResponse()
    {
        // Arrange
        AddUserSealedProductAdapterFake fakeAdapter = new()
        {
            ExecuteResult = new SuccessOperationResponse<UserSealedProductExtEntity>(new UserSealedProductExtEntity
            {
                UserId = "user123",
                ProductUuid = "product-uuid-456",
                SetId = "set789",
                Count = 3,
                ProductName = "Draft Booster",
                SetCode = "mkm",
                SetName = "Murders at Karlov Manor"
            })
        };

        UserSealedProductsCommandAdapter adapter = new InstanceWrapper(fakeAdapter);

        IUserSealedProductXfrEntity input = new UserSealedProductXfrEntityFake
        {
            UserId = "user123",
            ProductUuid = "product-uuid-456",
            SetId = "set789",
            CountDelta = 3
        };

        // Act
        IOperationResponse<UserSealedProductExtEntity> actual = await adapter.AddUserSealedProductAsync(input, CancellationToken.None).ConfigureAwait(false);

        // Assert
        actual.Should().NotBeNull();
        actual.IsSuccess.Should().BeTrue();
        actual.ResponseData.Should().NotBeNull();
        actual.ResponseData.UserId.Should().Be("user123");
        actual.ResponseData.ProductUuid.Should().Be("product-uuid-456");
        actual.ResponseData.SetId.Should().Be("set789");
        actual.ResponseData.Count.Should().Be(3);
        fakeAdapter.ExecuteInvokeCount.Should().Be(1);
    }

    [TestMethod, TestCategory("unit")]
    public async Task AddUserSealedProductAsync_WhenAdapterFails_ReturnsFailureResponse()
    {
        // Arrange
        AddUserSealedProductAdapterFake fakeAdapter = new() { ShouldReturnFailure = true };
        UserSealedProductsCommandAdapter adapter = new InstanceWrapper(fakeAdapter);

        IUserSealedProductXfrEntity input = new UserSealedProductXfrEntityFake
        {
            UserId = "user123",
            ProductUuid = "product-uuid-456",
            SetId = "set789",
            CountDelta = 1
        };

        // Act
        IOperationResponse<UserSealedProductExtEntity> actual = await adapter.AddUserSealedProductAsync(input, CancellationToken.None).ConfigureAwait(false);

        // Assert
        actual.Should().NotBeNull();
        actual.IsFailure.Should().BeTrue();
        actual.OuterException.Should().BeOfType<UserSealedProductsAdapterException>();
        fakeAdapter.ExecuteInvokeCount.Should().Be(1);
    }

    [TestMethod, TestCategory("unit")]
    public async Task AddUserSealedProductAsync_WithDecrementDelta_ReturnsSuccessResponse()
    {
        // Arrange
        AddUserSealedProductAdapterFake fakeAdapter = new()
        {
            ExecuteResult = new SuccessOperationResponse<UserSealedProductExtEntity>(new UserSealedProductExtEntity
            {
                UserId = "user123",
                ProductUuid = "product-uuid-456",
                SetId = "set789",
                Count = 2
            })
        };

        UserSealedProductsCommandAdapter adapter = new InstanceWrapper(fakeAdapter);

        IUserSealedProductXfrEntity input = new UserSealedProductXfrEntityFake
        {
            UserId = "user123",
            ProductUuid = "product-uuid-456",
            SetId = "set789",
            CountDelta = -1
        };

        // Act
        IOperationResponse<UserSealedProductExtEntity> actual = await adapter.AddUserSealedProductAsync(input, CancellationToken.None).ConfigureAwait(false);

        // Assert
        actual.Should().NotBeNull();
        actual.IsSuccess.Should().BeTrue();
        actual.ResponseData.Should().NotBeNull();
        fakeAdapter.ExecuteInvokeCount.Should().Be(1);
    }
}

internal sealed class InstanceWrapper : TypeWrapper<UserSealedProductsCommandAdapter>
{
    public InstanceWrapper(IAddUserSealedProductAdapter addUserSealedProductAdapter) : base(addUserSealedProductAdapter) { }
}
