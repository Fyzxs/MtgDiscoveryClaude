using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AwesomeAssertions;
using Lib.MtgDiscovery.Data.Apis;
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
using TestConvenience.Core.Reflection;

namespace Lib.MtgDiscovery.Entry.Tests.Queries;

[TestClass]
public sealed class CardEntryServiceTests
{
    private sealed class TestableCardEntryService : TypeWrapper<CardEntryService>
    {
        public TestableCardEntryService(ICardDataService cardDataService, ICardIdsArgEntityValidator validator, ICardsArgsToItrMapper mapper) 
            : base(cardDataService, validator, mapper) { }
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
        FakeCardIdsArgEntity args = new() { CardIds = ["id1", "id2"] };
        FakeCardIdsItrEntity mappedArgs = new() { CardIds = ["id1", "id2"] };
        FakeCardItemCollectionItrEntity expectedResponse = new();
        
        FakeCardIdsArgEntityValidator fakeValidator = new()
        {
            ValidateResult = new FakeValidatorActionResult { IsValidValue = true }
        };
        
        FakeCardsArgsToItrMapper fakeMapper = new()
        {
            MapResult = mappedArgs
        };
        
        FakeCardDataService fakeCardDataService = new()
        {
            CardsByIdsAsyncResult = new SuccessOperationResponse<ICardItemCollectionItrEntity>(expectedResponse)
        };

        CardEntryService subject = new TestableCardEntryService(fakeCardDataService, fakeValidator, fakeMapper);

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
        fakeCardDataService.CardsByIdsAsyncInvokeCount.Should().Be(1);
        fakeCardDataService.CardsByIdsAsyncInput.Should().BeSameAs(mappedArgs);
    }

    [TestMethod, TestCategory("unit")]
    public async Task CardsByIdsAsync_WithInvalidArgs_ReturnsFailureResponse()
    {
        // Arrange
        FakeCardIdsArgEntity args = new() { CardIds = [] };
        FakeOperationResponse failureResponse = new() { IsSuccessValue = false };
        
        FakeCardIdsArgEntityValidator fakeValidator = new()
        {
            ValidateResult = new FakeValidatorActionResult 
            { 
                IsValidValue = false,
                FailureStatusValue = failureResponse
            }
        };
        
        FakeCardsArgsToItrMapper fakeMapper = new();
        FakeCardDataService fakeCardDataService = new();

        CardEntryService subject = new TestableCardEntryService(fakeCardDataService, fakeValidator, fakeMapper);

        // Act
        IOperationResponse<ICardItemCollectionItrEntity> actual = await subject.CardsByIdsAsync(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeSameAs(failureResponse);
        fakeValidator.ValidateInvokeCount.Should().Be(1);
        fakeMapper.MapInvokeCount.Should().Be(0);
        fakeCardDataService.CardsByIdsAsyncInvokeCount.Should().Be(0);
    }

    private sealed class LoggerFake : ILogger
    {
        public IDisposable BeginScope<TState>(TState state) where TState : notnull => new DisposableFake();
        public bool IsEnabled(LogLevel logLevel) => false;
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter) { }

        private sealed class DisposableFake : IDisposable
        {
            public void Dispose() { }
        }
    }

    private sealed class FakeCardIdsArgEntityValidator : ICardIdsArgEntityValidator
    {
        public IValidatorActionResult<IOperationResponse<ICardItemCollectionItrEntity>> ValidateResult { get; init; } = new FakeValidatorActionResult();
        public int ValidateInvokeCount { get; private set; }
        public ICardIdsArgEntity ValidateInput { get; private set; } = default!;

        public Task<IValidatorActionResult<IOperationResponse<ICardItemCollectionItrEntity>>> Validate(ICardIdsArgEntity arg)
        {
            ValidateInvokeCount++;
            ValidateInput = arg;
            return Task.FromResult(ValidateResult);
        }
    }

    private sealed class FakeCardsArgsToItrMapper : ICardsArgsToItrMapper
    {
        public ICardIdsItrEntity MapResult { get; init; } = new FakeCardIdsItrEntity();
        public int MapInvokeCount { get; private set; }
        public ICardIdsArgEntity MapInput { get; private set; } = default!;

        public Task<ICardIdsItrEntity> Map(ICardIdsArgEntity arg)
        {
            MapInvokeCount++;
            MapInput = arg;
            return Task.FromResult(MapResult);
        }
    }

    private sealed class FakeCardDataService : ICardDataService
    {
        public IOperationResponse<ICardItemCollectionItrEntity> CardsByIdsAsyncResult { get; init; } = 
            new SuccessOperationResponse<ICardItemCollectionItrEntity>(new FakeCardItemCollectionItrEntity());
        public int CardsByIdsAsyncInvokeCount { get; private set; }
        public ICardIdsItrEntity CardsByIdsAsyncInput { get; private set; } = default!;

        public Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsByIdsAsync(ICardIdsItrEntity args)
        {
            CardsByIdsAsyncInvokeCount++;
            CardsByIdsAsyncInput = args;
            return Task.FromResult(CardsByIdsAsyncResult);
        }
    }

    private sealed class FakeValidatorActionResult : IValidatorActionResult<IOperationResponse<ICardItemCollectionItrEntity>>
    {
        public bool IsValidValue { get; init; }
        public IOperationResponse<ICardItemCollectionItrEntity> FailureStatusValue { get; init; } = new FakeOperationResponse();

        public bool IsValid() => IsValidValue;
        public bool IsNotValid() => !IsValidValue;
        public IOperationResponse<ICardItemCollectionItrEntity> FailureStatus() => FailureStatusValue;
    }

    private sealed class FakeOperationResponse : IOperationResponse<ICardItemCollectionItrEntity>
    {
        public bool IsSuccessValue { get; init; }
        public bool IsSuccess => IsSuccessValue;
        public bool IsFailure => !IsSuccessValue;
        public ICardItemCollectionItrEntity ResponseData => new FakeCardItemCollectionItrEntity();
        public OperationException OuterException => new BadRequestOperationException("Test exception");
        public HttpStatusCode Status => HttpStatusCode.OK;
    }

    private sealed class FakeCardIdsArgEntity : ICardIdsArgEntity
    {
        public ICollection<string> CardIds { get; init; } = [];
    }

    private sealed class FakeCardIdsItrEntity : ICardIdsItrEntity
    {
        public ICollection<string> CardIds { get; init; } = [];
    }

    private sealed class FakeCardItemCollectionItrEntity : ICardItemCollectionItrEntity
    {
        public ICollection<ICardItemItrEntity> Data { get; init; } = [];
    }
}