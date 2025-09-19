using System.Linq;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Lib.Universal.Extensions;

namespace Lib.MtgDiscovery.Entry.Queries.Validators.Sets;

internal interface ISetCodesArgEntityValidator : IValidatorAction<ISetCodesArgEntity, IOperationResponse<ISetItemCollectionOufEntity>>;
internal sealed class SetCodesArgEntityValidatorContainer : ValidatorActionContainer<ISetCodesArgEntity, IOperationResponse<ISetItemCollectionOufEntity>>, ISetCodesArgEntityValidator
{
    public SetCodesArgEntityValidatorContainer() : base([
            new IsNotNullSetCodesArgEntityValidator(),
            new CodesNotNullSetCodesArgEntityValidator(),
            new HasCodesSetCodesArgEntityValidator(),
            new ValidSetCodesArgEntityValidator(),
        ])
    { }
}

internal sealed class IsNotNullSetCodesArgEntityValidator : OperationResponseValidator<ISetCodesArgEntity, ISetItemCollectionOufEntity>
{
    public IsNotNullSetCodesArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<ISetCodesArgEntity>
    {
        public Task<bool> IsValid(ISetCodesArgEntity arg) => Task.FromResult(arg is not null);
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Provided object is null";
    }
}

internal sealed class CodesNotNullSetCodesArgEntityValidator : OperationResponseValidator<ISetCodesArgEntity, ISetItemCollectionOufEntity>
{
    public CodesNotNullSetCodesArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<ISetCodesArgEntity>
    {
        public Task<bool> IsValid(ISetCodesArgEntity arg) => Task.FromResult(arg.SetCodes is not null);
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Provided list is null";
    }
}

internal sealed class HasCodesSetCodesArgEntityValidator : OperationResponseValidator<ISetCodesArgEntity, ISetItemCollectionOufEntity>
{
    public HasCodesSetCodesArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<ISetCodesArgEntity>
    {
        public Task<bool> IsValid(ISetCodesArgEntity arg) => Task.FromResult(0 < arg.SetCodes.Count);
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Provided list is empty";
    }
}

internal sealed class ValidSetCodesArgEntityValidator : OperationResponseValidator<ISetCodesArgEntity, ISetItemCollectionOufEntity>
{
    public ValidSetCodesArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<ISetCodesArgEntity>
    {
        public Task<bool> IsValid(ISetCodesArgEntity arg) => Task.FromResult(arg.SetCodes.All(code => code.IzNotNullOrWhiteSpace()));
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Provided list has invalid entries";
    }
}
