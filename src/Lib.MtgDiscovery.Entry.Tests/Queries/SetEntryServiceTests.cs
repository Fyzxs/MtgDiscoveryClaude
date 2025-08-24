using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lib.MtgDiscovery.Entry.Queries;

namespace Lib.MtgDiscovery.Entry.Tests.Queries;

[TestClass]
public sealed class SetEntryServiceTests
{
    [TestMethod, TestCategory("unit")]
    public void Constructor_Default_CreatesInstance()
    {
        // Arrange & Act
        SetEntryService _ = new();

        // Assert
        // Constructor should create instance without throwing
    }
}