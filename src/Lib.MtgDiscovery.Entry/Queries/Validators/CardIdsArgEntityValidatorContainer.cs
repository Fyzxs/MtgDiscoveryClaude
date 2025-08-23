using System.Linq;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.Shared.Abstractions.Actions;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;
using Lib.Universal.Extensions;

namespace Lib.MtgDiscovery.Entry.Queries.Validators;

internal interface ICardIdsArgEntityValidator : IValidatorAction<ICardIdsArgEntity, IOperationResponse<ICardItemCollectionItrEntity>>;
internal sealed class CardIdsArgEntityValidatorContainer : ValidatorActionContainer<ICardIdsArgEntity, IOperationResponse<ICardItemCollectionItrEntity>>, ICardIdsArgEntityValidator
{
    public CardIdsArgEntityValidatorContainer() : base([
            new IsNotNullCardIdsArgEntityValidator(),
            new IdsNotNullCardIdsArgEntityValidator(),
            new HasIdsCardIdsArgEntityValidator(),
            new ValidCardIdsArgEntityValidator(),
        ])
    { }
}

internal sealed class IsNotNullCardIdsArgEntityValidator : OperationResponseValidator<ICardIdsArgEntity, ICardItemCollectionItrEntity>
{
    public IsNotNullCardIdsArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<ICardIdsArgEntity>
    {
        public Task<bool> IsValid(ICardIdsArgEntity arg) => Task.FromResult(arg is not null);
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Provided object is null";
    }
}

internal sealed class IdsNotNullCardIdsArgEntityValidator : OperationResponseValidator<ICardIdsArgEntity, ICardItemCollectionItrEntity>
{
    public IdsNotNullCardIdsArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<ICardIdsArgEntity>
    {
        public Task<bool> IsValid(ICardIdsArgEntity arg) => Task.FromResult(arg.CardIds is not null);
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Provided list is null";
    }
}

internal sealed class HasIdsCardIdsArgEntityValidator : OperationResponseValidator<ICardIdsArgEntity, ICardItemCollectionItrEntity>
{
    public HasIdsCardIdsArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<ICardIdsArgEntity>
    {
        public Task<bool> IsValid(ICardIdsArgEntity arg) => Task.FromResult(0 < arg.CardIds.Count);
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Provided list is empty";
    }
}

internal sealed class ValidCardIdsArgEntityValidator : OperationResponseValidator<ICardIdsArgEntity, ICardItemCollectionItrEntity>
{
    public ValidCardIdsArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<ICardIdsArgEntity>
    {
        public Task<bool> IsValid(ICardIdsArgEntity arg) => Task.FromResult(arg.CardIds.All(id => id.IzNotNullOrWhiteSpace()));
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Provided list has invalid entries";
    }
}
