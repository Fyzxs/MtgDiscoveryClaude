using System.Threading.Tasks;
using Lib.Shared.Abstractions.Actions;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Lib.Universal.Extensions;

namespace Lib.MtgDiscovery.Entry.Commands.Validators;

internal sealed class AddCardToCollectionArgEntityValidatorContainer : ValidatorActionContainer<IAddUserCardArgEntity, IOperationResponse<IUserCardOufEntity>>, IAddCardToCollectionArgEntityValidator
{
    public AddCardToCollectionArgEntityValidatorContainer() : base([
            new HasValidCardIdAddCardToCollectionArgEntityValidator(),
            new HasValidSetIdAddCardToCollectionArgEntityValidator(),
            new CollectedItemNotNullValidator(),
            new CollectedItemCountValidator(),
            new CollectedItemFinishValidator(),
            new CollectedItemSpecialValidator(),
        ])
    { }
}

internal sealed class HasValidCardIdAddCardToCollectionArgEntityValidator : OperationResponseValidator<IAddUserCardArgEntity, IUserCardOufEntity>
{
    public HasValidCardIdAddCardToCollectionArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IAddUserCardArgEntity>
    {
        public Task<bool> IsValid(IAddUserCardArgEntity arg) => Task.FromResult(arg.CardId.IzNotNullOrWhiteSpace());
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Card ID cannot be empty";
    }
}

internal sealed class HasValidSetIdAddCardToCollectionArgEntityValidator : OperationResponseValidator<IAddUserCardArgEntity, IUserCardOufEntity>
{
    public HasValidSetIdAddCardToCollectionArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IAddUserCardArgEntity>
    {
        public Task<bool> IsValid(IAddUserCardArgEntity arg) => Task.FromResult(arg.SetId.IzNotNullOrWhiteSpace());
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Set ID cannot be empty";
    }
}

