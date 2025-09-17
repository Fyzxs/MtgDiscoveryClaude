using System.Threading.Tasks;
using Lib.Adapter.UserCards.Apis;
using Lib.Adapter.UserCards.Tests.Fakes;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;
using TestConvenience.Core.Fakes;

namespace Lib.Adapter.UserCards.Tests.Apis;

[TestClass]
public sealed class UserCardsAdapterServiceTests
{
    [TestMethod, TestCategory("unit")]
    public void Constructor_WithLogger_CreatesInstance()
    {
        // Arrange
        ILogger logger = new LoggerFake();

        // Act
        UserCardsAdapterService actual = new(logger);

        // Assert
        actual.Should().NotBeNull();
    }

    [TestMethod, TestCategory("unit")]
    public void Constructor_ImplementsIUserCardsAdapterService()
    {
        // Arrange
        ILogger logger = new LoggerFake();

        // Act
        UserCardsAdapterService actual = new(logger);

        // Assert
        actual.Should().BeAssignableTo<IUserCardsAdapterService>();
    }

    [TestMethod, TestCategory("unit")]
    public async Task AddUserCardAsync_WithValidUserCard_DelegatesToCommandAdapter()
    {
        // Arrange
        ILogger logger = new LoggerFake();
        UserCardsAdapterService service = new(logger);

        IUserCardDetailsItrEntity collectedCard = new UserCardDetailsItrEntityFake
        {
            Finish = "nonfoil",
            Special = "none",
            Count = 1
        };

        IUserCardItrEntity userCard = new UserCardItrEntityFake
        {
            UserId = "user123",
            CardId = "card456",
            SetId = "set789",
            CollectedList = new[] { collectedCard }
        };

        // Act
        IOperationResponse<IUserCardItrEntity> actual = await service.AddUserCardAsync(userCard).ConfigureAwait(false);

        // Assert
        actual.Should().NotBeNull();
        actual.IsSuccess.Should().BeTrue();
        actual.ResponseData.Should().NotBeNull();
        actual.ResponseData.UserId.Should().Be("user123");
        actual.ResponseData.CardId.Should().Be("card456");
        actual.ResponseData.SetId.Should().Be("set789");
    }
}
