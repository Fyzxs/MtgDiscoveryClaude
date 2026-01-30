using System.Collections.Generic;
using System.Threading.Tasks;
using AwesomeAssertions;
using Lib.Domain.Collections.Apis;
using Lib.Domain.Collections.Tests.Fakes;
using Lib.Shared.DataModels.Entities.Itrs.Collections;
using Lib.Shared.DataModels.Entities.Oufs.Collections;
using Lib.Shared.Invocation.Operations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestConvenience.Core.Reflection;

namespace Lib.Domain.Collections.Tests.Apis;

[TestClass]
public sealed class CollectionsDomainServiceTests
{
    [TestMethod, TestCategory("unit")]
    public void Constructor_ImplementsInterface()
    {
        // Arrange
        CollectionCommandDomainServiceFake commandFake = new();
        CollectionQueryDomainServiceFake queryFake = new();

        // Act
        CollectionsDomainService subject = new InstanceWrapper(commandFake, queryFake);

        // Assert
        subject.Should().BeAssignableTo<ICollectionsDomainService>();
    }

    [TestMethod, TestCategory("unit")]
    public async Task CreateCollectionAsync_DelegatesToCommandService()
    {
        // Arrange
        CollectionOufEntityFake expectedOuf = new() { CollectionId = "col-123", Name = "Test" };
        OperationResponseFake<ICollectionOufEntity> expectedResponse = new()
        {
            IsSuccess = true,
            ResponseData = expectedOuf
        };

        CollectionCommandDomainServiceFake commandFake = new()
        {
            CreateCollectionAsyncResult = expectedResponse
        };
        CollectionQueryDomainServiceFake queryFake = new();
        CollectionsDomainService subject = new InstanceWrapper(commandFake, queryFake);

        CollectionItrEntityFake itrEntity = new()
        {
            CollectionId = "col-123",
            Name = "Test",
            Type = "custom"
        };

        // Act
        IOperationResponse<ICollectionOufEntity> actual = await subject
            .CreateCollectionAsync(itrEntity)
            .ConfigureAwait(false);

        // Assert
        actual.Should().Be(expectedResponse);
        commandFake.CreateCollectionAsyncInvokeCount.Should().Be(1);
    }

    [TestMethod, TestCategory("unit")]
    public async Task GetDefaultCollectionAsync_DelegatesToQueryService()
    {
        // Arrange
        CollectionOufEntityFake expectedOuf = new() { CollectionId = "col-default", IsDefault = true };
        OperationResponseFake<ICollectionOufEntity> expectedResponse = new()
        {
            IsSuccess = true,
            ResponseData = expectedOuf
        };

        CollectionCommandDomainServiceFake commandFake = new();
        CollectionQueryDomainServiceFake queryFake = new()
        {
            GetDefaultCollectionAsyncResult = expectedResponse
        };
        CollectionsDomainService subject = new InstanceWrapper(commandFake, queryFake);

        // Act
        IOperationResponse<ICollectionOufEntity> actual = await subject
            .GetDefaultCollectionAsync("user-123")
            .ConfigureAwait(false);

        // Assert
        actual.Should().Be(expectedResponse);
        queryFake.GetDefaultCollectionAsyncInvokeCount.Should().Be(1);
    }

    [TestMethod, TestCategory("unit")]
    public async Task GetCollectionsByOwnerAsync_DelegatesToQueryService()
    {
        // Arrange
        List<ICollectionOufEntity> expectedList = [new CollectionOufEntityFake { CollectionId = "col-1" }];
        OperationResponseFake<IEnumerable<ICollectionOufEntity>> expectedResponse = new()
        {
            IsSuccess = true,
            ResponseData = expectedList
        };

        CollectionCommandDomainServiceFake commandFake = new();
        CollectionQueryDomainServiceFake queryFake = new()
        {
            GetCollectionsByOwnerAsyncResult = expectedResponse
        };
        CollectionsDomainService subject = new InstanceWrapper(commandFake, queryFake);

        // Act
        IOperationResponse<IEnumerable<ICollectionOufEntity>> actual = await subject
            .GetCollectionsByOwnerAsync("user-123")
            .ConfigureAwait(false);

        // Assert
        actual.Should().Be(expectedResponse);
        queryFake.GetCollectionsByOwnerAsyncInvokeCount.Should().Be(1);
    }

    private sealed class InstanceWrapper : TypeWrapper<CollectionsDomainService>
    {
        public InstanceWrapper(
            ICollectionCommandDomainService commandService,
            ICollectionQueryDomainService queryService) : base(commandService, queryService) { }
    }
}
