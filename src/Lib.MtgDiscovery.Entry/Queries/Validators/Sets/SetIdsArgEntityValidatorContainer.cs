using System.Linq;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.Shared.Abstractions.Actions;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Lib.Universal.Extensions;

namespace Lib.MtgDiscovery.Entry.Queries.Validators.Sets;

internal interface ISetIdsArgEntityValidator : IValidatorAction<ISetIdsArgEntity, IOperationResponse<ISetItemCollectionOufEntity>>;
internal sealed class SetIdsArgEntityValidatorContainer : ValidatorActionContainer<ISetIdsArgEntity, IOperationResponse<ISetItemCollectionOufEntity>>, ISetIdsArgEntityValidator
{
    public SetIdsArgEntityValidatorContainer() : base([
            new IsNotNullSetIdsArgEntityValidator(),
            new IdsNotNullSetIdsArgEntityValidator(),
            new HasIdsSetIdsArgEntityValidator(),
            new ValidSetIdsArgEntityValidator(),
        ])
    { }
}

internal sealed class IsNotNullSetIdsArgEntityValidator : OperationResponseValidator<ISetIdsArgEntity, ISetItemCollectionOufEntity>
{
    public IsNotNullSetIdsArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<ISetIdsArgEntity>
    {
        public Task<bool> IsValid(ISetIdsArgEntity arg) => Task.FromResult(arg is not null);
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Provided object is null";
    }
}

internal sealed class IdsNotNullSetIdsArgEntityValidator : OperationResponseValidator<ISetIdsArgEntity, ISetItemCollectionOufEntity>
{
    public IdsNotNullSetIdsArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<ISetIdsArgEntity>
    {
        public Task<bool> IsValid(ISetIdsArgEntity arg) => Task.FromResult(arg.SetIds is not null);
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Provided list is null";
    }
}

internal sealed class HasIdsSetIdsArgEntityValidator : OperationResponseValidator<ISetIdsArgEntity, ISetItemCollectionOufEntity>
{
    public HasIdsSetIdsArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<ISetIdsArgEntity>
    {
        public Task<bool> IsValid(ISetIdsArgEntity arg) => Task.FromResult(0 < arg.SetIds.Count);
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Provided list is empty";
    }
}

internal sealed class ValidSetIdsArgEntityValidator : OperationResponseValidator<ISetIdsArgEntity, ISetItemCollectionOufEntity>
{
    public ValidSetIdsArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<ISetIdsArgEntity>
    {
        public Task<bool> IsValid(ISetIdsArgEntity arg) => Task.FromResult(arg.SetIds.All(id => id.IzNotNullOrWhiteSpace()));
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Provided list has invalid entries";
    }
}
