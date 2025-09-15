using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.UserCards.Apis;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;
using TestConvenience.Core.AwesomeAssertions;

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

        ICollectedCardItrEntity collectedCard = new CollectedCardItrEntityFake
        {
            Finish = "nonfoil",
            Special = "none",
            Count = 1
        };

        IUserCardCollectionItrEntity userCard = new UserCardCollectionItrEntityFake
        {
            UserId = "user123",
            CardId = "card456",
            SetId = "set789",
            CollectedList = new[] { collectedCard }
        };

        // Act
        IOperationResponse<IUserCardCollectionItrEntity> actual = await service.AddUserCardAsync(userCard).ConfigureAwait(false);

        // Assert
        actual.Should().NotBeNull();
        actual.IsSuccess.Should().BeTrue();
        actual.ResponseData.Should().NotBeNull();
        actual.ResponseData.UserId.Should().Be("user123");
        actual.ResponseData.CardId.Should().Be("card456");
        actual.ResponseData.SetId.Should().Be("set789");
    }
}

internal sealed class LoggerFake : ILogger
{
    public System.IDisposable BeginScope<TState>(TState state) where TState : notnull => null;
    public bool IsEnabled(LogLevel logLevel) => true;
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, System.Exception exception, System.Func<TState, System.Exception, string> formatter) { }
}

internal sealed class CollectedCardItrEntityFake : ICollectedCardItrEntity
{
    public string Finish { get; init; }
    public string Special { get; init; }
    public int Count { get; init; }
}

internal sealed class UserCardCollectionItrEntityFake : IUserCardCollectionItrEntity
{
    public string UserId { get; init; }
    public string CardId { get; init; }
    public string SetId { get; init; }
    public IEnumerable<ICollectedCardItrEntity> CollectedList { get; init; }
}
