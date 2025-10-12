using System.Linq;
using System.Threading.Tasks;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.Validators.Cards;

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