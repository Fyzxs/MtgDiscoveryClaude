using System.Linq;
using System.Threading.Tasks;
using Lib.Shared.Abstractions.Actions;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;
using Lib.Universal.Extensions;

namespace Lib.MtgDiscovery.Entry.Queries.Validators;

internal sealed class CardSearchTermArgEntityValidatorContainer : ValidatorActionContainer<ICardSearchTermArgEntity, IOperationResponse<ICardNameSearchResultCollectionItrEntity>>, ICardSearchTermArgEntityValidator
{
    public CardSearchTermArgEntityValidatorContainer() : base([
            new IsNotNullCardSearchTermArgEntityValidator(),
            new HasValidSearchTermArgEntityValidator(),
            new HasMinimumLengthSearchTermArgEntityValidator(),
        ])
    { }
}
internal sealed class IsNotNullCardSearchTermArgEntityValidator : OperationResponseValidator<ICardSearchTermArgEntity, ICardNameSearchResultCollectionItrEntity>
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

internal sealed class HasValidSearchTermArgEntityValidator : OperationResponseValidator<ICardSearchTermArgEntity, ICardNameSearchResultCollectionItrEntity>
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

internal sealed class HasMinimumLengthSearchTermArgEntityValidator : OperationResponseValidator<ICardSearchTermArgEntity, ICardNameSearchResultCollectionItrEntity>
{
    public HasMinimumLengthSearchTermArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<ICardSearchTermArgEntity>
    {
        public Task<bool> IsValid(ICardSearchTermArgEntity arg)
        {
            if (arg?.SearchTerm == null) return Task.FromResult(false);

            // Normalize the search term to check minimum length
            string normalized = new(arg.SearchTerm
                .ToLowerInvariant()
                .Where(char.IsLetter)
                .ToArray());

            return Task.FromResult(normalized.Length >= 3);
        }
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Search term must contain at least 3 letters";
    }
}
