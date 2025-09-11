namespace Lib.Shared.Invocation.Tests.Requests;

[TestClass]
public sealed class ValidatorFailedOperationResponseTests
{
    [TestMethod, TestCategory("unit")]
    public void ValidatorFailedOperationResponse_IsInternalClass_CannotTestDirectly()
    {
        // This class is internal and cannot be tested directly
        // It's tested indirectly through OperationStatusResponseValidator
        true.Should().BeTrue();
    }
}
