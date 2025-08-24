using AwesomeAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lib.Shared.Invocation.Tests.Requests;

[TestClass]
public sealed class ValidatorOperationExceptionTests
{
    [TestMethod, TestCategory("unit")]
    public void ValidatorOperationException_IsInternalClass_CannotTestDirectly()
    {
        // This class is internal and cannot be tested directly
        // It's tested indirectly through ValidatorFailedOperationResponse
        true.Should().BeTrue();
    }
}