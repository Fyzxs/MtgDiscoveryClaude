using System.Threading.Tasks;
using Lib.Shared.Abstractions.Actions;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Lib.Universal.Extensions;

namespace Lib.MtgDiscovery.Entry.Queries.Validators.Cards;

internal sealed class CardNameArgEntityValidatorContainer : ValidatorActionContainer<ICardNameArgEntity, IOperationResponse<ICardItemCollectionItrEntity>>, ICardNameArgEntityValidator
{
    public CardNameArgEntityValidatorContainer() : base([
            new IsNotNullCardNameArgEntityValidator(),
            new HasValidCardNameArgEntityValidator(),
        ])
    { }
}
internal sealed class IsNotNullCardNameArgEntityValidator : OperationResponseValidator<ICardNameArgEntity, ICardItemCollectionItrEntity>
{
    public IsNotNullCardNameArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<ICardNameArgEntity>
    {
        public Task<bool> IsValid(ICardNameArgEntity arg) => Task.FromResult(arg is not null);
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Card name argument cannot be null";
    }
}

internal sealed class HasValidCardNameArgEntityValidator : OperationResponseValidator<ICardNameArgEntity, ICardItemCollectionItrEntity>
{
    public HasValidCardNameArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<ICardNameArgEntity>
    {
        public Task<bool> IsValid(ICardNameArgEntity arg) => Task.FromResult(arg.CardName.IzNotNullOrWhiteSpace());
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Card name cannot be empty";
    }
}
