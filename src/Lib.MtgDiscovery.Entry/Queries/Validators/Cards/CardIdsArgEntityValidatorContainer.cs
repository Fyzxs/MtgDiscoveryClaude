using System.Linq;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.Shared.Abstractions.Actions;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Lib.Universal.Extensions;

namespace Lib.MtgDiscovery.Entry.Queries.Validators.Cards;

/// <summary>
/// Validator interface for CardIds argument entities.
/// </summary>
internal interface ICardIdsArgEntityValidator : IValidatorAction<ICardIdsArgEntity, IOperationResponse<ICardItemCollectionOufEntity>>;

/// <summary>
/// Container composing multiple validation rules for CardIds arguments.
/// 
/// Design Decision: Multiple small validator classes over consolidated validation logic
/// Reasoning:
///   - Each validator is independently testable with single failure reason
///   - Compile-time safety for error messages (no magic strings)
///   - Follows Open/Closed Principle - new validations are new classes
///   - Tests remain simple with no complex configuration
///   - Clear separation of concerns - each class validates one thing
/// 
/// Tradeoffs Accepted:
///   - More files (4 validators × 3 classes = 12 classes)
///   - Appears verbose at first glance
///   - Requires understanding the pattern
/// 
/// Alternatives Considered:
///   - Func delegates: Would lose type safety and testability
///   - Consolidated validator: Would move complexity to test configuration
///   - String messages: Would violate No Primitives principle
/// 
/// Pattern Validated: 2025-01-08
/// Next Review: When adding new validation requirements
/// </summary>
internal sealed class CardIdsArgEntityValidatorContainer : ValidatorActionContainer<ICardIdsArgEntity, IOperationResponse<ICardItemCollectionOufEntity>>, ICardIdsArgEntityValidator
{
    public CardIdsArgEntityValidatorContainer() : base([
            new IsNotNullCardIdsArgEntityValidator(),
            new IdsNotNullCardIdsArgEntityValidator(),
            new HasIdsCardIdsArgEntityValidator(),
            new ValidCardIdsArgEntityValidator(),
        ])
    { }
}

/// <summary>
/// Validates that the CardIds argument entity is not null.
/// 
/// Structure Pattern: Validator + Nested Types
///   - Validator: Typed behavior (not Func) for OOP and testability
///   - Message: Typed message (not string) following No Primitives
///   - Both are immutable and never change after creation
/// 
/// This class should NEVER change. If different validation is needed, create a new class.
/// </summary>
internal sealed class IsNotNullCardIdsArgEntityValidator : OperationResponseValidator<ICardIdsArgEntity, ICardItemCollectionOufEntity>
{
    public IsNotNullCardIdsArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<ICardIdsArgEntity>
    {
        public Task<bool> IsValid(ICardIdsArgEntity arg) => Task.FromResult(arg is not null);
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Provided object is null";
    }
}

internal sealed class IdsNotNullCardIdsArgEntityValidator : OperationResponseValidator<ICardIdsArgEntity, ICardItemCollectionOufEntity>
{
    public IdsNotNullCardIdsArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<ICardIdsArgEntity>
    {
        public Task<bool> IsValid(ICardIdsArgEntity arg) => Task.FromResult(arg.CardIds is not null);
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Provided list is null";
    }
}

internal sealed class HasIdsCardIdsArgEntityValidator : OperationResponseValidator<ICardIdsArgEntity, ICardItemCollectionOufEntity>
{
    public HasIdsCardIdsArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<ICardIdsArgEntity>
    {
        public Task<bool> IsValid(ICardIdsArgEntity arg) => Task.FromResult(0 < arg.CardIds.Count);
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Provided list is empty";
    }
}

internal sealed class ValidCardIdsArgEntityValidator : OperationResponseValidator<ICardIdsArgEntity, ICardItemCollectionOufEntity>
{
    public ValidCardIdsArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<ICardIdsArgEntity>
    {
        public Task<bool> IsValid(ICardIdsArgEntity arg) => Task.FromResult(arg.CardIds.All(id => id.IzNotNullOrWhiteSpace()));
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Provided list has invalid entries";
    }
}
