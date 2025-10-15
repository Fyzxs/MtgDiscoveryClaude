using Azure.Core;
using Azure.ResourceManager;
using Azure.ResourceManager.Resources;
using Lib.Cosmos.Adapters;
using Lib.Cosmos.Tests.Fakes;
using TestConvenience.Core.Reflection;

namespace Lib.Cosmos.Tests.Adapters;

[TestClass]
public sealed class CosmosArmClientAdapterTests
{
    private sealed class TestableCosmosArmClientAdapter : TypeWrapper<CosmosArmClientAdapter>
    {
        public TestableCosmosArmClientAdapter(ArmClient armClient) : base(armClient) { }
    }

    [TestMethod, TestCategory("unit")]
    public void Constructor_WithDependencies_CreatesInstance()
    {
        // Arrange
        CosmosContainerDefinitionFake containerDefinition = new()
        {
            AccountNameResult = new CosmosAccountNameFake("FakeAccount"),
            DatabaseNameResult = new CosmosDatabaseNameFake("FakeDatabase"),
            ContainerNameResult = new CosmosContainerNameFake("FakeContainer")
        };
        TokenCredential tokenCredential = new TokenCredentialFake();
        CosmosConnectionConvenienceFake connectionConvenience = new()
        {
            AccountEntraCredentialResult = tokenCredential
        };

        // Act
        CosmosArmClientAdapter _ = new(containerDefinition, connectionConvenience);

        // Assert
        connectionConvenience.AccountEntraCredentialInvokeCount.Should().Be(1);
    }

    [TestMethod, TestCategory("unit")]
    public void GetSubscriptionResource_CallsArmClient()
    {
        // Arrange
        ResourceIdentifier resourceId = SubscriptionResource.CreateResourceIdentifier("fake-subscription-id");
        SubscriptionResource expectedResource = new SubscriptionResourceFake();

        ArmClientFake armClientFake = new()
        {
            GetSubscriptionResourceResult = expectedResource
        };

        CosmosArmClientAdapter subject = new TestableCosmosArmClientAdapter(armClientFake);

        // Act
        SubscriptionResource result = subject.GetSubscriptionResource(resourceId);

        // Assert
        result.Should().BeSameAs(expectedResource);
        armClientFake.GetSubscriptionResourceInvokeCount.Should().Be(1);
    }
}
