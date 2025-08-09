using System;
using Lib.Cosmos.Apis.Operators;

namespace Lib.Cosmos.Tests.Apis.Operators;

[TestClass]
public class ReadPointItemTests
{
    [TestMethod, TestCategory("unit")]
    public void Class_ShouldBeSealed()
    {
        // Arrange
        Type subject = typeof(ReadPointItem);

        // Act
        bool actual = subject.IsSealed;

        // Assert
        _ = actual.Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public void Class_ShouldInheritFromPointItem()
    {
        // Arrange
        Type subject = typeof(ReadPointItem);

        // Act
        bool actual = subject.IsSubclassOf(typeof(PointItem));

        // Assert
        _ = actual.Should().BeTrue();
    }
}