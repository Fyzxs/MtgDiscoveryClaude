using Lib.Shared.Invocation.Primitives;

namespace Lib.Shared.Invocation.Tests.Primitives;

[TestClass]
public sealed class PrincipalIdTests
{
    private sealed class TestPrincipalId : PrincipalId
    {
        private readonly string _value;

        public TestPrincipalId(string value) => _value = value;

        public override string AsSystemType() => _value;
    }

    [TestMethod, TestCategory("unit")]
    public void AsSystemType_ReturnsExpectedValue()
    {
        // Arrange
        string expectedValue = "principal123";
        TestPrincipalId subject = new(expectedValue);

        // Act
        string actual = subject.AsSystemType();

        // Assert
        actual.Should().Be(expectedValue);
    }

    [TestMethod, TestCategory("unit")]
    public void Equals_WithSameValue_ReturnsTrue()
    {
        // Arrange
        string value = "principal456";
        TestPrincipalId subject1 = new(value);
        TestPrincipalId subject2 = new(value);

        // Act
        bool actual = subject1.Equals(subject2);

        // Assert
        actual.Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public void Equals_WithDifferentValue_ReturnsFalse()
    {
        // Arrange
        TestPrincipalId subject1 = new("principal1");
        TestPrincipalId subject2 = new("principal2");

        // Act
        bool actual = subject1.Equals(subject2);

        // Assert
        actual.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public void GetHashCode_WithSameValue_ReturnsSameHashCode()
    {
        // Arrange
        string value = "principal789";
        TestPrincipalId subject1 = new(value);
        TestPrincipalId subject2 = new(value);

        // Act
        int hashCode1 = subject1.GetHashCode();
        int hashCode2 = subject2.GetHashCode();

        // Assert
        hashCode1.Should().Be(hashCode2);
    }
}
