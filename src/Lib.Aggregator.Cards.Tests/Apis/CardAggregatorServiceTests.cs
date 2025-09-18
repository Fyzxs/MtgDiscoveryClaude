using System.Threading.Tasks;
using Lib.Aggregator.Cards.Apis;
using Lib.Aggregator.Cards.Tests.Fakes;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;
using TestConvenience.Core.Fakes;
using TestConvenience.Core.Reflection;

namespace Lib.Aggregator.Cards.Tests.Apis;

[TestClass]
public sealed class CardAggregatorServiceTests
{
    private sealed class TestableCardAggregatorService : TypeWrapper<CardAggregatorService>
    {
        public TestableCardAggregatorService(ICardAggregatorService cardAggregatorOperations) : base(cardAggregatorOperations) { }
    }

    [TestMethod, TestCategory("unit")]
    public void Constructor_WithLogger_CreatesInstance()
    {
        // Arrange
        ILogger logger = new LoggerFake();

        // Act
        CardAggregatorService _ = new(logger);

        // Assert
        // Constructor should create instance without throwing
    }

    [TestMethod, TestCategory("unit")]
    public async Task CardsByIdsAsync_CallsDependency()
    {
        // Arrange
        CardIdsItrEntityFake args = new()
        {
            CardIds = ["id1", "id2"]
        };

        CardItemCollectionItrEntityFake expectedCollection = new();
        OperationResponseFake<ICardItemCollectionItrEntity> expectedResponse = new()
        {
            IsSuccess = true,
            ResponseData = expectedCollection
        };

        CardAggregatorServiceFake fakeOperations = new()
        {
            CardsByIdsAsyncResult = expectedResponse
        };

        CardAggregatorService subject = new TestableCardAggregatorService(fakeOperations);

        // Act
        IOperationResponse<ICardItemCollectionItrEntity> actual = await subject.CardsByIdsAsync(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeSameAs(expectedResponse);
        fakeOperations.CardsByIdsAsyncInvokeCount.Should().Be(1);
        fakeOperations.CardsByIdsAsyncArgsInput.Should().BeSameAs(args);
    }

}
