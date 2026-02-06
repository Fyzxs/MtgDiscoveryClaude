using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using AwesomeAssertions;
using Lib.Scryfall.Ingestion.Aggregation;
using Lib.Scryfall.Shared.Apis.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lib.Scryfall.Ingestion.Tests.Aggregation;

[TestClass]
public sealed class RulingsAggregatorTests
{
    [TestMethod, TestCategory("unit")]
    public void AggregateByOracleId_EmptyInput_ReturnsEmptyDictionary()
    {
        // Arrange
        RulingsAggregator subject = new();
        List<dynamic> rulings = [];

        // Act
        Dictionary<string, IScryfallRuling> actual = subject.AggregateByOracleId(rulings);

        // Assert
        actual.Should().BeEmpty();
    }

    [TestMethod, TestCategory("unit")]
    public void AggregateByOracleId_SingleRuling_ReturnsSingleEntry()
    {
        // Arrange
        RulingsAggregator subject = new();
        dynamic ruling = new ExpandoObject();
        ruling.oracle_id = "oracle-1";
        ruling.comment = "This is a ruling.";
        List<dynamic> rulings = [ruling];

        // Act
        Dictionary<string, IScryfallRuling> actual = subject.AggregateByOracleId(rulings);

        // Assert
        actual.Should().HaveCount(1);
        actual["oracle-1"].OracleId.Should().Be("oracle-1");
        actual["oracle-1"].Rulings.Should().ContainSingle();
    }

    [TestMethod, TestCategory("unit")]
    public void AggregateByOracleId_MultipleRulingsSameOracleId_GroupsTogether()
    {
        // Arrange
        RulingsAggregator subject = new();
        dynamic ruling1 = new ExpandoObject();
        ruling1.oracle_id = "oracle-1";
        ruling1.comment = "First ruling.";
        dynamic ruling2 = new ExpandoObject();
        ruling2.oracle_id = "oracle-1";
        ruling2.comment = "Second ruling.";
        List<dynamic> rulings = [ruling1, ruling2];

        // Act
        Dictionary<string, IScryfallRuling> actual = subject.AggregateByOracleId(rulings);

        // Assert
        actual.Should().HaveCount(1);
        actual["oracle-1"].Rulings.Should().HaveCount(2);
    }

    [TestMethod, TestCategory("unit")]
    public void AggregateByOracleId_DifferentOracleIds_CreatesSeparateEntries()
    {
        // Arrange
        RulingsAggregator subject = new();
        dynamic ruling1 = new ExpandoObject();
        ruling1.oracle_id = "oracle-1";
        ruling1.comment = "Ruling for card 1.";
        dynamic ruling2 = new ExpandoObject();
        ruling2.oracle_id = "oracle-2";
        ruling2.comment = "Ruling for card 2.";
        List<dynamic> rulings = [ruling1, ruling2];

        // Act
        Dictionary<string, IScryfallRuling> actual = subject.AggregateByOracleId(rulings);

        // Assert
        actual.Should().HaveCount(2);
        actual["oracle-1"].OracleId.Should().Be("oracle-1");
        actual["oracle-2"].OracleId.Should().Be("oracle-2");
    }
}
