using System.Linq;
using System.Threading.Tasks;
using Lib.Shared.Abstractions.Actions;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Lib.Universal.Extensions;

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

/// <summary>
/// Validates search term argument is not null.
/// Standard null check following the typed validator pattern.
/// </summary>
internal sealed class IsNotNullCardSearchTermArgEntityValidator : OperationResponseValidator<ICardSearchTermArgEntity, ICardNameSearchCollectionOufEntity>
{
    public IsNotNullCardSearchTermArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<ICardSearchTermArgEntity>
    {
        public Task<bool> IsValid(ICardSearchTermArgEntity arg) => Task.FromResult(arg is not null);
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Search term argument cannot be null";
    }
}

internal sealed class HasValidSearchTermArgEntityValidator : OperationResponseValidator<ICardSearchTermArgEntity, ICardNameSearchCollectionOufEntity>
{
    public HasValidSearchTermArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<ICardSearchTermArgEntity>
    {
        public Task<bool> IsValid(ICardSearchTermArgEntity arg) => Task.FromResult(arg.SearchTerm.IzNotNullOrWhiteSpace());
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Search term cannot be empty";
    }
}

/// <summary>
/// Validates search term meets minimum length requirement after normalization.
/// 
/// This demonstrates how business logic fits into the validator pattern:
///   - Complex logic (normalization) is encapsulated in the Validator class
///   - Business rule (3 letters minimum) is explicit and testable
///   - Error message clearly states the business requirement
/// 
/// The normalization logic (lowercase, letters only) and the business rule
/// (minimum 3 letters) are coupled here by design - they represent a single
/// validation concern: "valid searchable term length".
/// </summary>
internal sealed class HasMinimumLengthSearchTermArgEntityValidator : OperationResponseValidator<ICardSearchTermArgEntity, ICardNameSearchCollectionOufEntity>
{
    public HasMinimumLengthSearchTermArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<ICardSearchTermArgEntity>
    {
        public Task<bool> IsValid(ICardSearchTermArgEntity arg)
        {
            if (arg?.SearchTerm == null) return Task.FromResult(false);

            // Normalize the search term to check minimum length
            string normalized = new([.. arg.SearchTerm
                .ToLowerInvariant()
                .Where(char.IsLetter)]);

            return Task.FromResult(normalized.Length >= 3);
        }
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Search term must contain at least 3 letters";
    }
}
