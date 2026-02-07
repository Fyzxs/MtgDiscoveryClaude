using System.Threading;
using System.Threading.Tasks;
using AwesomeAssertions;
using Lib.Aggregator.Collections.Apis;
using Lib.Domain.Collections.Commands;
using Lib.Domain.Collections.Tests.Fakes;
using Lib.Shared.DataModels.Entities.Itrs.Collections;
using Lib.Shared.DataModels.Entities.Oufs.Collections;
using Lib.Shared.Invocation.Operations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestConvenience.Core.Reflection;

namespace Lib.Domain.Collections.Tests.Commands;

[TestClass]
public sealed class CollectionCommandDomainServiceTests
{
    [TestMethod, TestCategory("unit")]
    public async Task CreateCollectionAsync_WithValidEntity_DelegatesToAggregator()
    {
        // Arrange
        CollectionOufEntityFake expectedOuf = new()
        {
            CollectionId = "col-123",
            OwnerId = "user-123",
            Name = "Test Collection",
            Type = "custom"
        };

        CollectionsAggregatorServiceFake aggregatorFake = new()
        {
            CreateCollectionAsyncResult = new OperationResponseFake<ICollectionOufEntity>
            {
                IsSuccess = true,
                ResponseData = expectedOuf
            }
        };

        CollectionCommandDomainService subject = new InstanceWrapper(aggregatorFake);

        CollectionItrEntityFake itrEntity = new()
        {
            CollectionId = "col-123",
            OwnerId = "user-123",
            Name = "Test Collection",
            Type = "custom"
        };

        // Act
        IOperationResponse<ICollectionOufEntity> actual = await subject
            .CreateCollectionAsync(itrEntity, CancellationToken.None)
            .ConfigureAwait(false);

        // Assert
        actual.IsSuccess.Should().BeTrue();
        actual.ResponseData.CollectionId.Should().Be("col-123");
        aggregatorFake.CreateCollectionAsyncInvokeCount.Should().Be(1);
    }

    [TestMethod, TestCategory("unit")]
    public async Task CreateCollectionAsync_WhenAggregatorFails_ReturnsFailure()
    {
        // Arrange
        CollectionsAggregatorServiceFake aggregatorFake = new()
        {
            CreateCollectionAsyncResult = new OperationResponseFake<ICollectionOufEntity>
            {
                IsSuccess = false,
                OuterException = new Lib.Shared.Invocation.Exceptions.BadRequestOperationException("Aggregator error")
            }
        };

        CollectionCommandDomainService subject = new InstanceWrapper(aggregatorFake);

        CollectionItrEntityFake itrEntity = new()
        {
            CollectionId = "col-123",
            Name = "Test",
            Type = "custom"
        };

        // Act
        IOperationResponse<ICollectionOufEntity> actual = await subject
            .CreateCollectionAsync(itrEntity, CancellationToken.None)
            .ConfigureAwait(false);

        // Assert
        actual.IsFailure.Should().BeTrue();
        aggregatorFake.CreateCollectionAsyncInvokeCount.Should().Be(1);
    }

    private sealed class InstanceWrapper : TypeWrapper<CollectionCommandDomainService>
    {
        public InstanceWrapper(ICollectionsAggregatorService aggregatorService) : base(aggregatorService) { }
    }
}
