using System.Threading.Tasks;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args.UserCards;
using Lib.Shared.DataModels.Entities.Oufs.UserCards.Signing;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Validators.UserCards;

internal sealed class UserIdNotNullUserCardsForSigningArgEntityValidator : OperationResponseValidator<IUserCardsForSigningArgEntity, ISigningResultOufEntity>
{
    public UserIdNotNullUserCardsForSigningArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IUserCardsForSigningArgEntity>
    {
        public Task<bool> IsValid(IUserCardsForSigningArgEntity arg) => Task.FromResult(arg.UserId is not null);
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "User Id cannot be null";
    }
}
