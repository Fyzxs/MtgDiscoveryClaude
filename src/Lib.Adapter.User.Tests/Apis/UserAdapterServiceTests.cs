using System.Threading.Tasks;
using AwesomeAssertions;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.User.Apis;
using Lib.Adapter.User.Tests.Fakes;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestConvenience.Core.Reflection;

namespace Lib.Adapter.User.Tests.Apis;

[TestClass]
public sealed class UserAdapterServiceTests
{
    [TestMethod, TestCategory("unit")]
    public void Constructor_ImplementsInterface()
    {
        // Arrange
        UserCommandAdapterFake commandAdapterFake = new();

        // Act
        UserAdapterService subject = new InstanceWrapper(commandAdapterFake);

        // Assert
        subject.Should().BeAssignableTo<IUserAdapterService>();
    }

    [TestMethod, TestCategory("unit")]
    public async Task RegisterUserAsync_WithValidUserInfo_DelegatesToCommandAdapter()
    {
        // Arrange
        UserInfoExtEntity expectedResult = new()
        {
            UserId = "user123",
            DisplayName = "Test User",
            SourceId = "auth0123"
        };

        IOperationResponse<UserInfoExtEntity> operationResponse = new OperationResponseFake<UserInfoExtEntity>
        {
            IsSuccess = true,
            ResponseData = expectedResult
        };

        UserCommandAdapterFake commandAdapterFake = new() { RegisterUserAsyncResult = operationResponse };
        UserAdapterService subject = new InstanceWrapper(commandAdapterFake);

        UserInfoItrEntityFake userInfo = new()
        {
            UserId = "user123",
            UserSourceId = "auth0123",
            UserNickname = "Test User"
        };

        // Act
        IOperationResponse<UserInfoExtEntity> actual = await subject.RegisterUserAsync(userInfo).ConfigureAwait(false);

        // Assert
        actual.Should().Be(operationResponse);
        commandAdapterFake.RegisterUserAsyncInvokeCount.Should().Be(1);
    }

    private sealed class InstanceWrapper : TypeWrapper<UserAdapterService>
    {
        public InstanceWrapper(IUserCommandAdapter commandAdapter)
            : base(commandAdapter) { }
    }
}
