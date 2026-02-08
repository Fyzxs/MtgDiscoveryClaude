using System.Threading.Tasks;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args.UserCards;
using Lib.Shared.DataModels.Entities.Oufs.UserCards.Signing;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Validators.UserCards;

internal sealed class ArtistIdsNotEmptyUserCardsForSigningArgEntityValidator : OperationResponseValidator<IUserCardsForSigningArgEntity, ISigningResultOufEntity>
{
    public ArtistIdsNotEmptyUserCardsForSigningArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IUserCardsForSigningArgEntity>
    {
        public Task<bool> IsValid(IUserCardsForSigningArgEntity arg) => Task.FromResult(0 < arg.ArtistIds.Count);
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Artist Ids cannot be empty";
    }
}
