using System.Threading.Tasks;
using Lib.Shared.Abstractions.Actions;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;
using Lib.Universal.Extensions;

namespace Lib.MtgDiscovery.Entry.Commands.Validators;

internal sealed class AddCardToCollectionArgEntityValidatorContainer : ValidatorActionContainer<IAddCardToCollectionArgEntity, IOperationResponse<IUserCardCollectionItrEntity>>, IAddCardToCollectionArgEntityValidator
{
    public AddCardToCollectionArgEntityValidatorContainer() : base([
            new HasValidCardIdAddCardToCollectionArgEntityValidator(),
            new HasValidSetIdAddCardToCollectionArgEntityValidator(),
            new HasCollectedListAddCardToCollectionArgEntityValidator(),
        ])
    { }
}

internal sealed class HasValidCardIdAddCardToCollectionArgEntityValidator : OperationResponseValidator<IAddCardToCollectionArgEntity, IUserCardCollectionItrEntity>
{
    public HasValidCardIdAddCardToCollectionArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IAddCardToCollectionArgEntity>
    {
        public Task<bool> IsValid(IAddCardToCollectionArgEntity arg) => Task.FromResult(arg.CardId.IzNotNullOrWhiteSpace());
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Card ID cannot be empty";
    }
}

internal sealed class HasValidSetIdAddCardToCollectionArgEntityValidator : OperationResponseValidator<IAddCardToCollectionArgEntity, IUserCardCollectionItrEntity>
{
    public HasValidSetIdAddCardToCollectionArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IAddCardToCollectionArgEntity>
    {
        public Task<bool> IsValid(IAddCardToCollectionArgEntity arg) => Task.FromResult(arg.SetId.IzNotNullOrWhiteSpace());
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Set ID cannot be empty";
    }
}

internal sealed class HasCollectedListAddCardToCollectionArgEntityValidator : OperationResponseValidator<IAddCardToCollectionArgEntity, IUserCardCollectionItrEntity>
{
    public HasCollectedListAddCardToCollectionArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IAddCardToCollectionArgEntity>
    {
        public Task<bool> IsValid(IAddCardToCollectionArgEntity arg) => Task.FromResult(arg.CollectedItem is not null);
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Collected item cannot be null";
    }
}
