using System.Threading.Tasks;
using Lib.Aggregator.UserSetCards.Entities;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.Invocation.Operations;
using Lib.Universal.Extensions;

namespace Lib.MtgDiscovery.Entry.Commands.UserSetCards.Validators;

internal sealed class AddSetGroupToUserSetCardArgsValidatorContainer : ValidatorActionContainer<IAddSetGroupToUserSetCardArgsEntity, IOperationResponse<IUserSetCardOufEntity>>, IAddSetGroupToUserSetCardArgsValidator
{
    public AddSetGroupToUserSetCardArgsValidatorContainer() : base([
            new AuthUserNotEmptyValidator(),
            new SetIdNotEmptyValidator(),
            new SetGroupIdNotEmptyValidator(),
            new CountNotNegativeValidator(),
        ])
    { }
}

internal sealed class AuthUserNotEmptyValidator : OperationResponseValidator<IAddSetGroupToUserSetCardArgsEntity, IUserSetCardOufEntity>
{
    public AuthUserNotEmptyValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IAddSetGroupToUserSetCardArgsEntity>
    {
        public Task<bool> IsValid(IAddSetGroupToUserSetCardArgsEntity arg) => Task.FromResult(arg.AuthUser?.UserId.IzNotNullOrWhiteSpace() ?? false);
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Auth user ID cannot be empty";
    }
}

internal sealed class SetIdNotEmptyValidator : OperationResponseValidator<IAddSetGroupToUserSetCardArgsEntity, IUserSetCardOufEntity>
{
    public SetIdNotEmptyValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IAddSetGroupToUserSetCardArgsEntity>
    {
        public Task<bool> IsValid(IAddSetGroupToUserSetCardArgsEntity arg) => Task.FromResult(arg.AddSetGroupToUserSetCard?.SetId.IzNotNullOrWhiteSpace() ?? false);
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Set ID cannot be empty";
    }
}

internal sealed class SetGroupIdNotEmptyValidator : OperationResponseValidator<IAddSetGroupToUserSetCardArgsEntity, IUserSetCardOufEntity>
{
    public SetGroupIdNotEmptyValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IAddSetGroupToUserSetCardArgsEntity>
    {
        public Task<bool> IsValid(IAddSetGroupToUserSetCardArgsEntity arg) => Task.FromResult(arg.AddSetGroupToUserSetCard?.SetGroupId.IzNotNullOrWhiteSpace() ?? false);
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Set Group ID cannot be empty";
    }
}

internal sealed class CountNotNegativeValidator : OperationResponseValidator<IAddSetGroupToUserSetCardArgsEntity, IUserSetCardOufEntity>
{
    public CountNotNegativeValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IAddSetGroupToUserSetCardArgsEntity>
    {
        public Task<bool> IsValid(IAddSetGroupToUserSetCardArgsEntity arg) => Task.FromResult(arg.AddSetGroupToUserSetCard != null && 0 <= arg.AddSetGroupToUserSetCard.Count);
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Count cannot be negative";
    }
}
