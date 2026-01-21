using System.Threading.Tasks;
using Lib.Aggregator.UserSetCards.Entities;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.Invocation.Operations;
using Lib.Universal.Extensions;

namespace Lib.MtgDiscovery.Entry.Queries.Validators.UserSetCards;

internal sealed class UserSetCardArgEntityValidatorContainer : ValidatorActionContainer<IUserSetCardArgEntity, IOperationResponse<IUserSetCardOufEntity>>, IUserSetCardArgEntityValidator
{
    public UserSetCardArgEntityValidatorContainer() : base([
            new IsNotNullUserSetCardArgEntityValidator(),
            new UserIdNotNullUserSetCardArgEntityValidator(),
            new HasValidUserIdUserSetCardArgEntityValidator(),
            new SetIdNotNullUserSetCardArgEntityValidator(),
            new HasValidSetIdUserSetCardArgEntityValidator()
        ])
    { }
}

internal sealed class IsNotNullUserSetCardArgEntityValidator : OperationResponseValidator<IUserSetCardArgEntity, IUserSetCardOufEntity>
{
    public IsNotNullUserSetCardArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IUserSetCardArgEntity>
    {
        public Task<bool> IsValid(IUserSetCardArgEntity arg) => Task.FromResult(arg is not null);
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Provided object is null";
    }
}

internal sealed class UserIdNotNullUserSetCardArgEntityValidator : OperationResponseValidator<IUserSetCardArgEntity, IUserSetCardOufEntity>
{
    public UserIdNotNullUserSetCardArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IUserSetCardArgEntity>
    {
        public Task<bool> IsValid(IUserSetCardArgEntity arg) => Task.FromResult(arg.UserId is not null);
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "UserId is null";
    }
}

internal sealed class HasValidUserIdUserSetCardArgEntityValidator : OperationResponseValidator<IUserSetCardArgEntity, IUserSetCardOufEntity>
{
    public HasValidUserIdUserSetCardArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IUserSetCardArgEntity>
    {
        public Task<bool> IsValid(IUserSetCardArgEntity arg) => Task.FromResult(arg.UserId.IzNotNullOrWhiteSpace());
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "UserId is empty or whitespace";
    }
}

internal sealed class SetIdNotNullUserSetCardArgEntityValidator : OperationResponseValidator<IUserSetCardArgEntity, IUserSetCardOufEntity>
{
    public SetIdNotNullUserSetCardArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IUserSetCardArgEntity>
    {
        public Task<bool> IsValid(IUserSetCardArgEntity arg) => Task.FromResult(arg.SetId is not null);
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "SetId is null";
    }
}

internal sealed class HasValidSetIdUserSetCardArgEntityValidator : OperationResponseValidator<IUserSetCardArgEntity, IUserSetCardOufEntity>
{
    public HasValidSetIdUserSetCardArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IUserSetCardArgEntity>
    {
        public Task<bool> IsValid(IUserSetCardArgEntity arg) => Task.FromResult(arg.SetId.IzNotNullOrWhiteSpace());
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "SetId is empty or whitespace";
    }
}
