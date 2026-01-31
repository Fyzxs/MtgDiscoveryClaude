using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Oufs.UserCards;
using Lib.Shared.Invocation.Operations;
using Lib.Universal.Extensions;

namespace Lib.MtgDiscovery.Entry.Commands.Actions.Validators;

internal sealed class HasValidCardIdAddCardToCollectionArgEntityValidator : OperationResponseValidator<IAddCardToCollectionArgsEntity, IUserCardOufEntity>
{
    public HasValidCardIdAddCardToCollectionArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IAddCardToCollectionArgsEntity>
    {
        public Task<bool> IsValid(IAddCardToCollectionArgsEntity arg) => Task.FromResult(arg.AddUserCard.CardId.IzNotNullOrWhiteSpace());
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Card ID cannot be empty";
    }
}
