using System.Threading.Tasks;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args.Artists;
using Lib.Shared.DataModels.Entities.Oufs.Cards;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Validators.Artists;

internal sealed class IsNotNullArtistIdArgEntityValidator : OperationResponseValidator<IArtistIdArgEntity, ICardItemCollectionOufEntity>
{
    public IsNotNullArtistIdArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IArtistIdArgEntity>
    {
        public Task<bool> IsValid(IArtistIdArgEntity arg) => Task.FromResult(arg is not null);
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Artist ID argument cannot be null";
    }
}
