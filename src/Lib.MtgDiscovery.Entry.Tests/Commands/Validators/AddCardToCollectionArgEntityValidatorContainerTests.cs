using System.Threading.Tasks;
using AwesomeAssertions;
using Lib.MtgDiscovery.Entry.Commands.Validators;
using Lib.Shared.Abstractions.Actions;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lib.MtgDiscovery.Entry.Tests.Commands.Validators;

[TestClass]
public sealed class AddCardToCollectionArgEntityValidatorContainerTests
{
    [TestMethod, TestCategory("unit")]
    public async Task Validate_WithValidArgs_ReturnsValidResult()
    {
        // Arrange
        UserCardArgEntityFake args = new()
        {
            CardId = "valid-card-id",
            SetId = "valid-set-id",
            UserCardDetails = new UserCardDetailsArgEntityFake
            {
                Count = 1,
                Finish = "normal",
                Special = "none"
            }
        };
        AddCardToCollectionArgEntityValidatorContainer subject = new();

        // Act
        IValidatorActionResult<IOperationResponse<IUserCardItrEntity>> actual =
            await subject.Validate(args).ConfigureAwait(false);

        // Assert
        actual.Should().NotBeNull();
        actual.IsValid().Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task Validate_WithZeroCount_ReturnsInvalidResult()
    {
        // Arrange
        UserCardArgEntityFake args = new()
        {
            CardId = "valid-card-id",
            SetId = "valid-set-id",
            UserCardDetails = new UserCardDetailsArgEntityFake
            {
                Count = 0,
                Finish = "normal",
                Special = "none"
            }
        };
        AddCardToCollectionArgEntityValidatorContainer subject = new();

        // Act
        IValidatorActionResult<IOperationResponse<IUserCardItrEntity>> actual =
            await subject.Validate(args).ConfigureAwait(false);

        // Assert
        actual.Should().NotBeNull();
        actual.IsValid().Should().BeFalse();
        actual.FailureStatus().Should().NotBeNull();
        actual.FailureStatus().Message.Should().Contain("Count cannot be 0");
    }

    [TestMethod, TestCategory("unit")]
    public async Task Validate_WithNegativeCount_ReturnsValidResult()
    {
        // Arrange
        UserCardArgEntityFake args = new()
        {
            CardId = "valid-card-id",
            SetId = "valid-set-id",
            UserCardDetails = new UserCardDetailsArgEntityFake
            {
                Count = -1,
                Finish = "normal",
                Special = "none"
            }
        };
        AddCardToCollectionArgEntityValidatorContainer subject = new();

        // Act
        IValidatorActionResult<IOperationResponse<IUserCardItrEntity>> actual =
            await subject.Validate(args).ConfigureAwait(false);

        // Assert
        actual.Should().NotBeNull();
        actual.IsValid().Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task Validate_WithLargeNegativeCount_ReturnsValidResult()
    {
        // Arrange
        UserCardArgEntityFake args = new()
        {
            CardId = "valid-card-id",
            SetId = "valid-set-id",
            UserCardDetails = new UserCardDetailsArgEntityFake
            {
                Count = -100,
                Finish = "normal",
                Special = "none"
            }
        };
        AddCardToCollectionArgEntityValidatorContainer subject = new();

        // Act
        IValidatorActionResult<IOperationResponse<IUserCardItrEntity>> actual =
            await subject.Validate(args).ConfigureAwait(false);

        // Assert
        actual.Should().NotBeNull();
        actual.IsValid().Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task Validate_WithNullUserCardDetails_ReturnsInvalidResult()
    {
        // Arrange
        UserCardArgEntityFake args = new()
        {
            CardId = "valid-card-id",
            SetId = "valid-set-id",
            UserCardDetails = null!
        };
        AddCardToCollectionArgEntityValidatorContainer subject = new();

        // Act
        IValidatorActionResult<IOperationResponse<IUserCardItrEntity>> actual =
            await subject.Validate(args).ConfigureAwait(false);

        // Assert
        actual.Should().NotBeNull();
        actual.IsValid().Should().BeFalse();
        actual.FailureStatus().Should().NotBeNull();
        actual.FailureStatus().Message.Should().Contain("Collected item cannot be null");
    }

    [TestMethod, TestCategory("unit")]
    public async Task Validate_WithEmptyCardId_ReturnsInvalidResult()
    {
        // Arrange
        UserCardArgEntityFake args = new()
        {
            CardId = "",
            SetId = "valid-set-id",
            UserCardDetails = new UserCardDetailsArgEntityFake
            {
                Count = 1,
                Finish = "normal",
                Special = "none"
            }
        };
        AddCardToCollectionArgEntityValidatorContainer subject = new();

        // Act
        IValidatorActionResult<IOperationResponse<IUserCardItrEntity>> actual =
            await subject.Validate(args).ConfigureAwait(false);

        // Assert
        actual.Should().NotBeNull();
        actual.IsValid().Should().BeFalse();
        actual.FailureStatus().Should().NotBeNull();
        actual.FailureStatus().Message.Should().Contain("Card ID cannot be empty");
    }

    private sealed class UserCardArgEntityFake : IUserCardArgEntity
    {
        public string CardId { get; init; } = string.Empty;
        public string SetId { get; init; } = string.Empty;
        public IUserCardDetailsArgEntity UserCardDetails { get; init; } = default!;
    }

    private sealed class UserCardDetailsArgEntityFake : IUserCardDetailsArgEntity
    {
        public string Finish { get; init; } = string.Empty;
        public string Special { get; init; } = string.Empty;
        public int Count { get; init; }
    }
}