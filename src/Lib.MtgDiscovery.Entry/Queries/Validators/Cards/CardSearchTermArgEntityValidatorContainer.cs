using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.Validators.Cards;

/// <summary>
/// Container composing validation rules for card search term arguments.
/// 
/// Design Decision: Each validation concern is a separate class
/// This validator demonstrates the pattern with business logic validation
/// (minimum search length) alongside standard null checks.
/// 
/// Validation Sequence:
///   1. Not null check (fail fast on null argument)
///   2. Not empty check (fail fast on empty string)
///   3. Business rule check (minimum 3 letters after normalization)
/// 
/// Pattern Benefits Demonstrated:
///   - Business rules (min length) isolated from basic validation
///   - Each rule independently testable
///   - Complex logic (normalization) contained in single class
///   - Easy to add new rules without touching existing ones
/// 
/// See CardIdsArgEntityValidatorContainer for full pattern documentation.
/// Pattern Validated: 2025-01-08
/// </summary>
internal sealed class CardSearchTermArgEntityValidatorContainer : ValidatorActionContainer<ICardSearchTermArgEntity, IOperationResponse<ICardNameSearchCollectionOufEntity>>, ICardSearchTermArgEntityValidator
{
    public CardSearchTermArgEntityValidatorContainer() : base([
            new IsNotNullCardSearchTermArgEntityValidator(),
            new HasValidSearchTermArgEntityValidator(),
            new HasMinimumLengthSearchTermArgEntityValidator(),
        ])
    { }
}
