using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AwesomeAssertions;
using Lib.Domain.Cards.Apis;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.MtgDiscovery.Entry.Queries;
using Lib.MtgDiscovery.Entry.Queries.Mappers;
using Lib.MtgDiscovery.Entry.Queries.Validators;
using Lib.Shared.Abstractions.Actions;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Exceptions;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestConvenience.Core.Fakes;
using TestConvenience.Core.Reflection;

namespace Lib.MtgDiscovery.Entry.Tests.Queries;

[TestClass]
public sealed class CardEntryServiceTests
{
    private sealed class TestableCardEntryService : TypeWrapper<CardEntryService>
    {
        public TestableCardEntryService(ICardDomainService cardDomainService, ICardIdsArgEntityValidator validator, ISetCodeArgEntityValidator setCodeValidator, ICardNameArgEntityValidator cardNameValidator, ICardSearchTermArgEntityValidator searchTermValidator, ICardsArgsToItrMapper mapper, ISetCodeArgsToItrMapper setCodeMapper, ICardNameArgsToItrMapper cardNameMapper, ICardSearchTermArgsToItrMapper searchTermMapper)
            : base(cardDomainService, validator, setCodeValidator, cardNameValidator, searchTermValidator, mapper, setCodeMapper, cardNameMapper, searchTermMapper) { }
    }

    [TestMethod, TestCategory("unit")]
    public void Constructor_WithLogger_CreatesInstance()
    {
        // Arrange
        ILogger logger = new LoggerFake();

        // Act
        CardEntryService _ = new(logger);

        // Assert
        // Constructor should create instance without throwing
    }

    [TestMethod, TestCategory("unit")]
    public async Task CardsByIdsAsync_WithValidArgs_CallsValidatorMapperAndDataService()
    {
        // Arrange
        CardIdsArgEntityFake args = new() { CardIds = ["id1", "id2"] };
        CardIdsItrEntityFake mappedArgs = new() { CardIds = ["id1", "id2"] };
        CardItemCollectionItrEntityFake expectedResponse = new();

        CardIdsArgEntityValidatorFake fakeValidator = new()
        {
            ValidateResult = new ValidatorActionResultFake { IsValidValue = true }
        };

        CardsArgsToItrMapperFake fakeMapper = new()
        {
            MapResult = mappedArgs
        };

        CardDomainServiceFake fakeCardDomainService = new()
        {
            CardsByIdsAsyncResult = new SuccessOperationResponse<ICardItemCollectionItrEntity>(expectedResponse)
        };

        SetCodeArgEntityValidatorFake fakeSetCodeValidator = new();
        CardNameArgEntityValidatorFake fakeCardNameValidator = new();
        CardSearchTermArgEntityValidatorFake fakeSearchTermValidator = new();
        SetCodeArgsToItrMapperFake fakeSetCodeMapper = new();
        CardNameArgsToItrMapperFake fakeCardNameMapper = new();
        CardSearchTermArgsToItrMapperFake fakeSearchTermMapper = new();

        CardEntryService subject = new TestableCardEntryService(fakeCardDomainService, fakeValidator, fakeSetCodeValidator, fakeCardNameValidator, fakeSearchTermValidator, fakeMapper, fakeSetCodeMapper, fakeCardNameMapper, fakeSearchTermMapper);

        // Act
        IOperationResponse<ICardItemCollectionItrEntity> actual = await subject.CardsByIdsAsync(args).ConfigureAwait(false);

        // Assert
        actual.Should().NotBeNull();
        actual.IsSuccess.Should().BeTrue();
        actual.ResponseData.Should().BeSameAs(expectedResponse);
        fakeValidator.ValidateInvokeCount.Should().Be(1);
        fakeValidator.ValidateInput.Should().BeSameAs(args);
        fakeMapper.MapInvokeCount.Should().Be(1);
        fakeMapper.MapInput.Should().BeSameAs(args);
        fakeCardDomainService.CardsByIdsAsyncInvokeCount.Should().Be(1);
        fakeCardDomainService.CardsByIdsAsyncInput.Should().BeSameAs(mappedArgs);
    }

    [TestMethod, TestCategory("unit")]
    public async Task CardsByIdsAsync_WithInvalidArgs_ReturnsFailureResponse()
    {
        // Arrange
        CardIdsArgEntityFake args = new() { CardIds = [] };
        OperationResponseFake failureResponse = new() { IsSuccessValue = false };

        CardIdsArgEntityValidatorFake fakeValidator = new()
        {
            ValidateResult = new ValidatorActionResultFake
            {
                IsValidValue = false,
                FailureStatusValue = failureResponse
            }
        };

        CardsArgsToItrMapperFake fakeMapper = new();
        CardDomainServiceFake fakeCardDomainService = new();

        SetCodeArgEntityValidatorFake fakeSetCodeValidator = new();
        CardNameArgEntityValidatorFake fakeCardNameValidator = new();
        CardSearchTermArgEntityValidatorFake fakeSearchTermValidator = new();
        SetCodeArgsToItrMapperFake fakeSetCodeMapper = new();
        CardNameArgsToItrMapperFake fakeCardNameMapper = new();
        CardSearchTermArgsToItrMapperFake fakeSearchTermMapper = new();

        CardEntryService subject = new TestableCardEntryService(fakeCardDomainService, fakeValidator, fakeSetCodeValidator, fakeCardNameValidator, fakeSearchTermValidator, fakeMapper, fakeSetCodeMapper, fakeCardNameMapper, fakeSearchTermMapper);

        // Act
        IOperationResponse<ICardItemCollectionItrEntity> actual = await subject.CardsByIdsAsync(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeSameAs(failureResponse);
        fakeValidator.ValidateInvokeCount.Should().Be(1);
        fakeMapper.MapInvokeCount.Should().Be(0);
        fakeCardDomainService.CardsByIdsAsyncInvokeCount.Should().Be(0);
    }


    private sealed class CardIdsArgEntityValidatorFake : ICardIdsArgEntityValidator
    {
        public IValidatorActionResult<IOperationResponse<ICardItemCollectionItrEntity>> ValidateResult { get; init; } = new ValidatorActionResultFake();
        public int ValidateInvokeCount { get; private set; }
        public ICardIdsArgEntity ValidateInput { get; private set; } = default!;

        public Task<IValidatorActionResult<IOperationResponse<ICardItemCollectionItrEntity>>> Validate(ICardIdsArgEntity arg)
        {
            ValidateInvokeCount++;
            ValidateInput = arg;
            return Task.FromResult(ValidateResult);
        }
    }

    private sealed class CardsArgsToItrMapperFake : ICardsArgsToItrMapper
    {
        public ICardIdsItrEntity MapResult { get; init; } = new CardIdsItrEntityFake();
        public int MapInvokeCount { get; private set; }
        public ICardIdsArgEntity MapInput { get; private set; } = default!;

        public Task<ICardIdsItrEntity> Map(ICardIdsArgEntity arg)
        {
            MapInvokeCount++;
            MapInput = arg;
            return Task.FromResult(MapResult);
        }
    }

    private sealed class CardDomainServiceFake : ICardDomainService
    {
        public IOperationResponse<ICardItemCollectionItrEntity> CardsByIdsAsyncResult { get; init; } =
            new SuccessOperationResponse<ICardItemCollectionItrEntity>(new CardItemCollectionItrEntityFake());
        public int CardsByIdsAsyncInvokeCount { get; private set; }
        public ICardIdsItrEntity CardsByIdsAsyncInput { get; private set; } = default!;

        public IOperationResponse<ICardItemCollectionItrEntity> CardsBySetCodeAsyncResult { get; init; } = new SuccessOperationResponse<ICardItemCollectionItrEntity>(new CardItemCollectionItrEntityFake());
        public int CardsBySetCodeAsyncInvokeCount { get; private set; }
        public ISetCodeItrEntity CardsBySetCodeAsyncInput { get; private set; } = default!;

        public IOperationResponse<ICardItemCollectionItrEntity> CardsByNameAsyncResult { get; init; } = new SuccessOperationResponse<ICardItemCollectionItrEntity>(new CardItemCollectionItrEntityFake());
        public int CardsByNameAsyncInvokeCount { get; private set; }
        public ICardNameItrEntity CardsByNameAsyncInput { get; private set; } = default!;

        public IOperationResponse<ICardNameSearchResultCollectionItrEntity> CardNameSearchAsyncResult { get; init; } = new SuccessOperationResponse<ICardNameSearchResultCollectionItrEntity>(new CardNameSearchResultCollectionItrEntityFake());
        public int CardNameSearchAsyncInvokeCount { get; private set; }
        public ICardSearchTermItrEntity CardNameSearchAsyncInput { get; private set; } = default!;

        public Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsByIdsAsync(ICardIdsItrEntity args)
        {
            CardsByIdsAsyncInvokeCount++;
            CardsByIdsAsyncInput = args;
            return Task.FromResult(CardsByIdsAsyncResult);
        }

        public Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsBySetCodeAsync(ISetCodeItrEntity setCode)
        {
            CardsBySetCodeAsyncInvokeCount++;
            CardsBySetCodeAsyncInput = setCode;
            return Task.FromResult(CardsBySetCodeAsyncResult);
        }

        public Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsByNameAsync(ICardNameItrEntity cardName)
        {
            CardsByNameAsyncInvokeCount++;
            CardsByNameAsyncInput = cardName;
            return Task.FromResult(CardsByNameAsyncResult);
        }

        public Task<IOperationResponse<ICardNameSearchResultCollectionItrEntity>> CardNameSearchAsync(ICardSearchTermItrEntity searchTerm)
        {
            CardNameSearchAsyncInvokeCount++;
            CardNameSearchAsyncInput = searchTerm;
            return Task.FromResult(CardNameSearchAsyncResult);
        }
    }

    private sealed class ValidatorActionResultFake : IValidatorActionResult<IOperationResponse<ICardItemCollectionItrEntity>>
    {
        public bool IsValidValue { get; init; }
        public IOperationResponse<ICardItemCollectionItrEntity> FailureStatusValue { get; init; } = new OperationResponseFake();

        public bool IsValid() => IsValidValue;
        public bool IsNotValid() => !IsValidValue;
        public IOperationResponse<ICardItemCollectionItrEntity> FailureStatus() => FailureStatusValue;
    }

    private sealed class OperationResponseFake : IOperationResponse<ICardItemCollectionItrEntity>
    {
        public bool IsSuccessValue { get; init; }
        public bool IsSuccess => IsSuccessValue;
        public bool IsFailure => !IsSuccessValue;
        public ICardItemCollectionItrEntity ResponseData => new CardItemCollectionItrEntityFake();
        public OperationException OuterException => new BadRequestOperationException("Test exception");
        public HttpStatusCode Status => HttpStatusCode.OK;
    }

    private sealed class CardIdsArgEntityFake : ICardIdsArgEntity
    {
        public ICollection<string> CardIds { get; init; } = [];
    }

    private sealed class CardIdsItrEntityFake : ICardIdsItrEntity
    {
        public ICollection<string> CardIds { get; init; } = [];
    }

    private sealed class CardItemCollectionItrEntityFake : ICardItemCollectionItrEntity
    {
        public ICollection<ICardItemItrEntity> Data { get; init; } = [];
    }

    private sealed class CardNameSearchResultCollectionItrEntityFake : ICardNameSearchResultCollectionItrEntity
    {
        public ICollection<ICardNameSearchResultItrEntity> Names { get; init; } = [];
    }

    private sealed class SetCodeArgEntityValidatorFake : ISetCodeArgEntityValidator
    {
        public IValidatorActionResult<IOperationResponse<ICardItemCollectionItrEntity>> ValidateResult { get; init; } = new ValidatorActionResultFake();
        public int ValidateInvokeCount { get; private set; }
        public ISetCodeArgEntity ValidateInput { get; private set; } = default!;

        public Task<IValidatorActionResult<IOperationResponse<ICardItemCollectionItrEntity>>> Validate(ISetCodeArgEntity arg)
        {
            ValidateInvokeCount++;
            ValidateInput = arg;
            return Task.FromResult(ValidateResult);
        }
    }

    private sealed class CardNameArgEntityValidatorFake : ICardNameArgEntityValidator
    {
        public IValidatorActionResult<IOperationResponse<ICardItemCollectionItrEntity>> ValidateResult { get; init; } = new ValidatorActionResultFake();
        public int ValidateInvokeCount { get; private set; }
        public ICardNameArgEntity ValidateInput { get; private set; } = default!;

        public Task<IValidatorActionResult<IOperationResponse<ICardItemCollectionItrEntity>>> Validate(ICardNameArgEntity arg)
        {
            ValidateInvokeCount++;
            ValidateInput = arg;
            return Task.FromResult(ValidateResult);
        }
    }

    private sealed class CardSearchTermArgEntityValidatorFake : ICardSearchTermArgEntityValidator
    {
        public IValidatorActionResult<IOperationResponse<ICardNameSearchResultCollectionItrEntity>> ValidateResult { get; init; } = new SearchValidatorActionResultFake();
        public int ValidateInvokeCount { get; private set; }
        public ICardSearchTermArgEntity ValidateInput { get; private set; } = default!;

        public Task<IValidatorActionResult<IOperationResponse<ICardNameSearchResultCollectionItrEntity>>> Validate(ICardSearchTermArgEntity arg)
        {
            ValidateInvokeCount++;
            ValidateInput = arg;
            return Task.FromResult(ValidateResult);
        }
    }

    private sealed class SearchValidatorActionResultFake : IValidatorActionResult<IOperationResponse<ICardNameSearchResultCollectionItrEntity>>
    {
        public bool IsValidValue { get; init; }
        public IOperationResponse<ICardNameSearchResultCollectionItrEntity> FailureStatusValue { get; init; } = new SearchOperationResponseFake();

        public bool IsValid() => IsValidValue;
        public bool IsNotValid() => !IsValidValue;
        public IOperationResponse<ICardNameSearchResultCollectionItrEntity> FailureStatus() => FailureStatusValue;
    }

    private sealed class SearchOperationResponseFake : IOperationResponse<ICardNameSearchResultCollectionItrEntity>
    {
        public bool IsSuccessValue { get; init; }
        public bool IsSuccess => IsSuccessValue;
        public bool IsFailure => !IsSuccessValue;
        public ICardNameSearchResultCollectionItrEntity ResponseData => new CardNameSearchResultCollectionItrEntityFake();
        public OperationException OuterException => new BadRequestOperationException("Test exception");
        public HttpStatusCode Status => HttpStatusCode.OK;
    }

    private sealed class SetCodeArgsToItrMapperFake : ISetCodeArgsToItrMapper
    {
        public ISetCodeItrEntity MapResult { get; init; } = new SetCodeItrEntityFake();
        public int MapInvokeCount { get; private set; }
        public ISetCodeArgEntity MapInput { get; private set; } = default!;

        public Task<ISetCodeItrEntity> Map(ISetCodeArgEntity arg)
        {
            MapInvokeCount++;
            MapInput = arg;
            return Task.FromResult(MapResult);
        }
    }

    private sealed class CardNameArgsToItrMapperFake : ICardNameArgsToItrMapper
    {
        public ICardNameItrEntity MapResult { get; init; } = new CardNameItrEntityFake();
        public int MapInvokeCount { get; private set; }
        public ICardNameArgEntity MapInput { get; private set; } = default!;

        public Task<ICardNameItrEntity> Map(ICardNameArgEntity arg)
        {
            MapInvokeCount++;
            MapInput = arg;
            return Task.FromResult(MapResult);
        }
    }

    private sealed class CardSearchTermArgsToItrMapperFake : ICardSearchTermArgsToItrMapper
    {
        public ICardSearchTermItrEntity MapResult { get; init; } = new CardSearchTermItrEntityFake();
        public int MapInvokeCount { get; private set; }
        public ICardSearchTermArgEntity MapInput { get; private set; } = default!;

        public Task<ICardSearchTermItrEntity> Map(ICardSearchTermArgEntity arg)
        {
            MapInvokeCount++;
            MapInput = arg;
            return Task.FromResult(MapResult);
        }
    }

    private sealed class SetCodeItrEntityFake : ISetCodeItrEntity
    {
        public string SetCode { get; init; } = string.Empty;
    }

    private sealed class CardNameItrEntityFake : ICardNameItrEntity
    {
        public string CardName { get; init; } = string.Empty;
    }

    private sealed class CardSearchTermItrEntityFake : ICardSearchTermItrEntity
    {
        public string SearchTerm { get; init; } = string.Empty;
    }
}
