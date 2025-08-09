namespace TestConvenience.Core.Reflection;

internal sealed class Asserter : IAsserter
{
    /// <inheritdoc/>
    public void AssertIf(bool condition, string exceptionMsg)
    {
        if (!condition) return;
        throw new AsserterException(exceptionMsg);
    }
}