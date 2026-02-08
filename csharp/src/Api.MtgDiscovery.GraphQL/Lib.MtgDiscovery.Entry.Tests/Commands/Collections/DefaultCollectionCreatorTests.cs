using System.Threading;
using System.Threading.Tasks;
using AwesomeAssertions;
using Lib.Domain.Collections.Apis;
using Lib.MtgDiscovery.Entry.Commands.Collections;
using Lib.MtgDiscovery.Entry.Commands.Collections.Apis;
using Lib.MtgDiscovery.Entry.Commands.Collections.Mappers;
using Lib.MtgDiscovery.Entry.Tests.Fakes;
using Lib.Shared.DataModels.Entities.Args.User;
using Lib.Shared.DataModels.Entities.Itrs.Collections;
using Lib.Shared.DataModels.Entities.Oufs.Collections;
using Lib.Shared.Invocation.Operations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestConvenience.Core.Reflection;

namespace Lib.MtgDiscovery.Entry.Tests.Commands.Collections;

[TestClass]
public sealed class DefaultCollectionCreatorTests
{
    [TestMethod, TestCategory("unit")]
    public void Constructor_ImplementsInterface()
    {
        // Arrange
        CollectionsDomainServiceFake domainFake = new()
        {
            GetDefaultCollectionAsyncResult = new OperationResponseFake<ICollectionOufEntity> { IsSuccess = false },
            CreateCollectionAsyncResult = new OperationResponseFake<ICollectionOufEntity>
            {
                IsSuccess = true,
                ResponseData = new CollectionOufEntityFake()
            }
        };

        // Act
        DefaultCollectionCreator subject = new InstanceWrapper(domainFake, new DefaultCollectionArgToItrMapper());

        // Assert
        subject.Should().BeAssignableTo<IDefaultCollectionCreator>();
    }

    [TestMethod, TestCategory("unit")]
    public async Task CreateDefaultCollectionAsync_WithUserId_CallsDomainService()
    {
        // Arrange
        CollectionsDomainServiceFake domainFake = new()
        {
            GetDefaultCollectionAsyncResult = new OperationResponseFake<ICollectionOufEntity> { IsSuccess = false },
            CreateCollectionAsyncResult = new OperationResponseFake<ICollectionOufEntity>
            {
                IsSuccess = true,
                ResponseData = new CollectionOufEntityFake()
            }
        };
        DefaultCollectionCreator subject = new InstanceWrapper(domainFake, new DefaultCollectionArgToItrMapper());

        // Act
        await subject.CreateDefaultCollectionAsync(new UserIdArgEntityFake { UserId = "user-123" }, CancellationToken.None).ConfigureAwait(false);

        // Assert
        domainFake.GetDefaultCollectionAsyncInvokeCount.Should().Be(1);
        domainFake.CreateCollectionAsyncInvokeCount.Should().Be(1);
        domainFake.CreateCollectionAsyncLastEntity.OwnerId.Should().Be("user-123");
        domainFake.CreateCollectionAsyncLastEntity.Name.Should().Be("My Collection");
        domainFake.CreateCollectionAsyncLastEntity.Type.Should().Be("default");
        domainFake.CreateCollectionAsyncLastEntity.Visibility.Should().Be("public");
        domainFake.CreateCollectionAsyncLastEntity.IsDefault.Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task CreateDefaultCollectionAsync_SetsCollectionId()
    {
        // Arrange
        CollectionsDomainServiceFake domainFake = new()
        {
            GetDefaultCollectionAsyncResult = new OperationResponseFake<ICollectionOufEntity> { IsSuccess = false },
            CreateCollectionAsyncResult = new OperationResponseFake<ICollectionOufEntity>
            {
                IsSuccess = true,
                ResponseData = new CollectionOufEntityFake()
            }
        };
        DefaultCollectionCreator subject = new InstanceWrapper(domainFake, new DefaultCollectionArgToItrMapper());

        // Act
        await subject.CreateDefaultCollectionAsync(new UserIdArgEntityFake { UserId = "user-456" }, CancellationToken.None).ConfigureAwait(false);

        // Assert
        domainFake.CreateCollectionAsyncLastEntity.CollectionId.Should().NotBeNullOrWhiteSpace();
    }

    [TestMethod, TestCategory("unit")]
    public async Task CreateDefaultCollectionAsync_SetsTimestamps()
    {
        // Arrange
        CollectionsDomainServiceFake domainFake = new()
        {
            GetDefaultCollectionAsyncResult = new OperationResponseFake<ICollectionOufEntity> { IsSuccess = false },
            CreateCollectionAsyncResult = new OperationResponseFake<ICollectionOufEntity>
            {
                IsSuccess = true,
                ResponseData = new CollectionOufEntityFake()
            }
        };
        DefaultCollectionCreator subject = new InstanceWrapper(domainFake, new DefaultCollectionArgToItrMapper());

        // Act
        await subject.CreateDefaultCollectionAsync(new UserIdArgEntityFake { UserId = "user-789" }, CancellationToken.None).ConfigureAwait(false);

        // Assert
        domainFake.CreateCollectionAsyncLastEntity.CreatedAt.Should().NotBeNullOrWhiteSpace();
        domainFake.CreateCollectionAsyncLastEntity.UpdatedAt.Should().NotBeNullOrWhiteSpace();
        domainFake.CreateCollectionAsyncLastEntity.CreatedAt.Should()
            .Be(domainFake.CreateCollectionAsyncLastEntity.UpdatedAt);
    }

    [TestMethod, TestCategory("unit")]
    public async Task CreateDefaultCollectionAsync_SetsEmptyAuthorizedUsers()
    {
        // Arrange
        CollectionsDomainServiceFake domainFake = new()
        {
            GetDefaultCollectionAsyncResult = new OperationResponseFake<ICollectionOufEntity> { IsSuccess = false },
            CreateCollectionAsyncResult = new OperationResponseFake<ICollectionOufEntity>
            {
                IsSuccess = true,
                ResponseData = new CollectionOufEntityFake()
            }
        };
        DefaultCollectionCreator subject = new InstanceWrapper(domainFake, new DefaultCollectionArgToItrMapper());

        // Act
        await subject.CreateDefaultCollectionAsync(new UserIdArgEntityFake { UserId = "user-abc" }, CancellationToken.None).ConfigureAwait(false);

        // Assert
        domainFake.CreateCollectionAsyncLastEntity.AuthorizedUsers.Should().BeEmpty();
    }

    [TestMethod, TestCategory("unit")]
    public async Task CreateDefaultCollectionAsync_WhenDefaultAlreadyExists_DoesNotCreateAnother()
    {
        // Arrange
        CollectionsDomainServiceFake domainFake = new()
        {
            GetDefaultCollectionAsyncResult = new OperationResponseFake<ICollectionOufEntity>
            {
                IsSuccess = true,
                ResponseData = new CollectionOufEntityFake()
            },
            CreateCollectionAsyncResult = new OperationResponseFake<ICollectionOufEntity>
            {
                IsSuccess = true,
                ResponseData = new CollectionOufEntityFake()
            }
        };
        DefaultCollectionCreator subject = new InstanceWrapper(domainFake, new DefaultCollectionArgToItrMapper());

        // Act
        await subject.CreateDefaultCollectionAsync(new UserIdArgEntityFake { UserId = "user-xyz" }, CancellationToken.None).ConfigureAwait(false);

        // Assert
        domainFake.GetDefaultCollectionAsyncInvokeCount.Should().Be(1);
        domainFake.CreateCollectionAsyncInvokeCount.Should().Be(0);
    }

    private sealed class InstanceWrapper : TypeWrapper<DefaultCollectionCreator>
    {
        public InstanceWrapper(ICollectionsDomainService domainService, IDefaultCollectionArgToItrMapper mapper) : base(domainService, mapper) { }
    }

    private sealed class UserIdArgEntityFake : IUserIdArgEntity
    {
        public required string UserId { get; init; }
    }

    private sealed class DefaultCollectionArgToItrMapperFake : IDefaultCollectionArgToItrMapper
    {
        public ICollectionItrEntity MapResult { get; init; }

        public Task<ICollectionItrEntity> Map(IUserIdArgEntity source) => Task.FromResult(MapResult);
    }
}
