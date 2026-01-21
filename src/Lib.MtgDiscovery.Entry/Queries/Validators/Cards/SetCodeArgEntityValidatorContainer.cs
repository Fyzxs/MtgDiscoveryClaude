using System.Threading.Tasks;
using Lib.Shared.Abstractions.Actions;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Lib.Universal.Extensions;

namespace Lib.MtgDiscovery.Entry.Queries.Validators.Cards;

internal sealed class SetCodeArgEntityValidatorContainer : ValidatorActionContainer<ISetCodeArgEntity, IOperationResponse<ICardItemCollectionItrEntity>>, ISetCodeArgEntityValidator
{
    public SetCodeArgEntityValidatorContainer() : base([
            new IsNotNullSetCodeArgEntityValidator(),
            new HasValidSetCodeArgEntityValidator(),
        ])
    { }
}

internal sealed class IsNotNullSetCodeArgEntityValidator : OperationResponseValidator<ISetCodeArgEntity, ICardItemCollectionItrEntity>
{
    public IsNotNullSetCodeArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<ISetCodeArgEntity>
    {
        public Task<bool> IsValid(ISetCodeArgEntity arg) => Task.FromResult(arg is not null);
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Set code argument cannot be null";
    }
}

internal sealed class HasValidSetCodeArgEntityValidator : OperationResponseValidator<ISetCodeArgEntity, ICardItemCollectionItrEntity>
{
    public HasValidSetCodeArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<ISetCodeArgEntity>
    {
        public Task<bool> IsValid(ISetCodeArgEntity arg) => Task.FromResult(arg.SetCode.IzNotNullOrWhiteSpace());
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Set code cannot be empty";
    }
}
