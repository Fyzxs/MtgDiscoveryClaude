using System.Linq;
using System.Threading.Tasks;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args.Artists;
using Lib.Shared.DataModels.Entities.Oufs.Artists;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Validators.Artists;

internal sealed class HasMinimumLengthArtistSearchTermArgEntityValidator : OperationResponseValidator<IArtistSearchTermArgEntity, IArtistSearchResultCollectionOufEntity>
{
    public HasMinimumLengthArtistSearchTermArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IArtistSearchTermArgEntity>
    {
        public Task<bool> IsValid(IArtistSearchTermArgEntity arg)
        {
            if (arg?.SearchTerm == null)
                return Task.FromResult(false);

            // Normalize the search term to check minimum length
            string normalized = new([.. arg.SearchTerm
                .ToLowerInvariant()
                .Where(char.IsLetter)]);

            return Task.FromResult(3 <= normalized.Length);
        }
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Artist search term must contain at least 3 letters";
    }
}
