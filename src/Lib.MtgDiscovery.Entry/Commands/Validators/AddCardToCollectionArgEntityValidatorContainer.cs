using System.Threading.Tasks;
using Lib.Shared.Abstractions.Actions;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Lib.Universal.Extensions;

namespace Lib.MtgDiscovery.Entry.Commands.Validators;

internal sealed class AddCardToCollectionArgEntityValidatorContainer : ValidatorActionContainer<IUserCardArgEntity, IOperationResponse<IUserCardItrEntity>>, IAddCardToCollectionArgEntityValidator
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

internal sealed class HasValidCardIdAddCardToCollectionArgEntityValidator : OperationResponseValidator<IUserCardArgEntity, IUserCardItrEntity>
{
    public HasValidCardIdAddCardToCollectionArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IUserCardArgEntity>
    {
        public Task<bool> IsValid(IUserCardArgEntity arg) => Task.FromResult(arg.CardId.IzNotNullOrWhiteSpace());
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Card ID cannot be empty";
    }
}

internal sealed class HasValidSetIdAddCardToCollectionArgEntityValidator : OperationResponseValidator<IUserCardArgEntity, IUserCardItrEntity>
{
    public HasValidSetIdAddCardToCollectionArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IUserCardArgEntity>
    {
        public Task<bool> IsValid(IUserCardArgEntity arg) => Task.FromResult(arg.SetId.IzNotNullOrWhiteSpace());
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Set ID cannot be empty";
    }
}

