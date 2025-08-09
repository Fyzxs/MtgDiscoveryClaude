using System;
using Lib.Universal.Primitives;

namespace Lib.Universal.Tests;

public abstract class BaseToSystemTypeTests<TType, TSystemType>
{
    [TestMethod, TestCategory("unit")]
    public void Class_ShouldBeAbstract()
    {
        // Arrange
        Type subject = typeof(TType);

        // Act
        bool actual = subject.IsAbstract;

        // Assert
        _ = actual.Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public void ClassShouldDeriveFromToSystemType()
    {
        // Arrange
        Type subject = typeof(TType);

        // Act
        bool actual = subject.IsSubclassOf(typeof(ToSystemType<TSystemType>));

        // Assert
        _ = actual.Should().BeTrue();
    }
}
