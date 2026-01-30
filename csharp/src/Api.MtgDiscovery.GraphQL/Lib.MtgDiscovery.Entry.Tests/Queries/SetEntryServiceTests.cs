using System;
using Lib.MtgDiscovery.Entry.Queries;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestConvenience.Core.Fakes;

namespace Lib.MtgDiscovery.Entry.Tests.Queries;

[TestClass]
public sealed class SetEntryServiceTests
{
    [TestMethod, TestCategory("unit")]
    public void Constructor_WithLogger_CreatesInstance()
    {
        // Arrange
        ILogger logger = new LoggerFake();

        // Act
        SetEntryService _ = new(logger);

        // Assert
        // Constructor should create instance without throwing
    }

}
