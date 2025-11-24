using System.Threading.Tasks;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args.Artists;
using Lib.Shared.DataModels.Entities.Oufs.Artists;
using Lib.Shared.Invocation.Operations;
using Lib.Universal.Extensions;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Validators.Artists;

internal sealed class HasValidArtistSearchTermArgEntityValidator : OperationResponseValidator<IArtistSearchTermArgEntity, IArtistSearchResultCollectionOufEntity>
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
