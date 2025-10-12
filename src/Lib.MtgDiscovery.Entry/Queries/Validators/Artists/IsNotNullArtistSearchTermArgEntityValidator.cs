using System.Threading.Tasks;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.Validators.Artists;

internal sealed class IsNotNullArtistSearchTermArgEntityValidator : OperationResponseValidator<IArtistSearchTermArgEntity, IArtistSearchResultCollectionOufEntity>
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