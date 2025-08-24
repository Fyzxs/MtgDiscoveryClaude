using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lib.MtgDiscovery.Data.Queries;

namespace Lib.MtgDiscovery.Data.Tests.Queries;

[TestClass]
public sealed class SetDataServiceTests
{
    [TestMethod, TestCategory("unit")]
    public void Constructor_Default_CreatesInstance()
    {
        // Arrange & Act
        SetDataService _ = new();

        // Assert
        // Constructor should create instance without throwing
    }
}