using Lib.MtgDiscovery.Entry.Apis;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.Validators.Cards;

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
