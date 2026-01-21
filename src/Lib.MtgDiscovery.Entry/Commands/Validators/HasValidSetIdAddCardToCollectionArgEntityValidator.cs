using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Lib.Universal.Extensions;

namespace Lib.MtgDiscovery.Entry.Commands.Validators;

internal sealed class HasValidSetIdAddCardToCollectionArgEntityValidator : OperationResponseValidator<IAddCardToCollectionArgsEntity, IUserCardOufEntity>
{
    public HasValidSetIdAddCardToCollectionArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IAddCardToCollectionArgsEntity>
    {
        public Task<bool> IsValid(IAddCardToCollectionArgsEntity arg) => Task.FromResult(arg.AddUserCard.SetId.IzNotNullOrWhiteSpace());
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Set ID cannot be empty";
    }
}