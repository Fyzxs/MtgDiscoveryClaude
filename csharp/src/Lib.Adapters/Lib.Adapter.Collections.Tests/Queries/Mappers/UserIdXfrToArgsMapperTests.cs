using System.Threading.Tasks;
using Lib.Adapter.Collections.Queries.Mappers;
using Lib.Adapter.Collections.Tests.Fakes;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions.Entities;

namespace Lib.Adapter.Collections.Tests.Queries.Mappers;

[TestClass]
public sealed class UserIdXfrToArgsMapperTests
{
    [TestMethod, TestCategory("unit")]
    public async Task Map_WithUserId_ReturnsArgsWithUserId()
    {
        // Arrange
        const string expectedUserId = "user-123";
        UserIdXfrEntityFake input = new() { UserId = expectedUserId };
        UserIdXfrToArgsMapper subject = new();

        // Act
        UserIdExtEntity actual = await subject.Map(input).ConfigureAwait(false);

        // Assert
        actual.UserId.Should().Be(expectedUserId);
    }

    [TestMethod, TestCategory("unit")]
    public async Task Map_WithEmptyUserId_ReturnsArgsWithEmptyUserId()
    {
        // Arrange
        UserIdXfrEntityFake input = new() { UserId = string.Empty };
        UserIdXfrToArgsMapper subject = new();

        // Act
        UserIdExtEntity actual = await subject.Map(input).ConfigureAwait(false);

        // Assert
        actual.UserId.Should().BeEmpty();
    }

    [TestMethod, TestCategory("unit")]
    public async Task Map_ReturnsNewInstanceEachTime()
    {
        // Arrange
        UserIdXfrEntityFake input = new() { UserId = "user-123" };
        UserIdXfrToArgsMapper subject = new();

        // Act
        UserIdExtEntity firstCall = await subject.Map(input).ConfigureAwait(false);
        UserIdExtEntity secondCall = await subject.Map(input).ConfigureAwait(false);

        // Assert
        firstCall.Should().NotBeSameAs(secondCall);
    }
}
