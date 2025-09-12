using System.Linq;
using System.Threading.Tasks;
using Lib.Shared.Abstractions.Actions;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;
using Lib.Universal.Extensions;

namespace Lib.MtgDiscovery.Entry.Queries.Validators;

internal sealed class ArtistSearchTermArgEntityValidatorContainer : ValidatorActionContainer<IArtistSearchTermArgEntity, IOperationResponse<IArtistSearchResultCollectionItrEntity>>, IArtistSearchTermArgEntityValidator
{
    public ArtistSearchTermArgEntityValidatorContainer() : base([
            new IsNotNullArtistSearchTermArgEntityValidator(),
            new HasValidArtistSearchTermArgEntityValidator(),
            new HasMinimumLengthArtistSearchTermArgEntityValidator(),
        ])
    { }
}
internal sealed class IsNotNullArtistSearchTermArgEntityValidator : OperationResponseValidator<IArtistSearchTermArgEntity, IArtistSearchResultCollectionItrEntity>
{
    public IsNotNullArtistSearchTermArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IArtistSearchTermArgEntity>
    {
        public Task<bool> IsValid(IArtistSearchTermArgEntity arg) => Task.FromResult(arg is not null);
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Artist search term argument cannot be null";
    }
}

internal sealed class HasValidArtistSearchTermArgEntityValidator : OperationResponseValidator<IArtistSearchTermArgEntity, IArtistSearchResultCollectionItrEntity>
{
    public HasValidArtistSearchTermArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IArtistSearchTermArgEntity>
    {
        public Task<bool> IsValid(IArtistSearchTermArgEntity arg) => Task.FromResult(arg.SearchTerm.IzNotNullOrWhiteSpace());
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Artist search term cannot be empty";
    }
}

internal sealed class HasMinimumLengthArtistSearchTermArgEntityValidator : OperationResponseValidator<IArtistSearchTermArgEntity, IArtistSearchResultCollectionItrEntity>
{
    public HasMinimumLengthArtistSearchTermArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IArtistSearchTermArgEntity>
    {
        public Task<bool> IsValid(IArtistSearchTermArgEntity arg)
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
        public override string AsSystemType() => "Artist search term must contain at least 3 letters";
    }
}
