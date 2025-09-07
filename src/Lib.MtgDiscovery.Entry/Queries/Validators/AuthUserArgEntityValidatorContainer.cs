using System.Threading.Tasks;
using Lib.Shared.Abstractions.Actions;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;
using Lib.Universal.Extensions;

namespace Lib.MtgDiscovery.Entry.Queries.Validators;

internal sealed class AuthUserArgEntityValidatorContainer : ValidatorActionContainer<IAuthUserArgEntity, IOperationResponse<IUserRegistrationItrEntity>>, IAuthUserArgEntityValidator
{
    public AuthUserArgEntityValidatorContainer() : base([
            new IsNotNullAuthUserArgEntityValidator(),
            new HasValidUserIdAuthUserArgEntityValidator(),
            new HasValidSourceIdAuthUserArgEntityValidator(),
            new HasValidDisplayNameAuthUserArgEntityValidator(),
        ])
    { }
}

internal sealed class IsNotNullAuthUserArgEntityValidator : OperationResponseValidator<IAuthUserArgEntity, IUserRegistrationItrEntity>
{
    public IsNotNullAuthUserArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IAuthUserArgEntity>
    {
        public Task<bool> IsValid(IAuthUserArgEntity arg) => Task.FromResult(arg is not null);
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "User authentication argument cannot be null";
    }
}

internal sealed class HasValidUserIdAuthUserArgEntityValidator : OperationResponseValidator<IAuthUserArgEntity, IUserRegistrationItrEntity>
{
    public HasValidUserIdAuthUserArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IAuthUserArgEntity>
    {
        public Task<bool> IsValid(IAuthUserArgEntity arg) => Task.FromResult(arg.UserId.IzNotNullOrWhiteSpace());
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "User ID cannot be empty";
    }
}

internal sealed class HasValidSourceIdAuthUserArgEntityValidator : OperationResponseValidator<IAuthUserArgEntity, IUserRegistrationItrEntity>
{
    public HasValidSourceIdAuthUserArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IAuthUserArgEntity>
    {
        public Task<bool> IsValid(IAuthUserArgEntity arg) => Task.FromResult(arg.SourceId.IzNotNullOrWhiteSpace());
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Source ID cannot be empty";
    }
}

internal sealed class HasValidDisplayNameAuthUserArgEntityValidator : OperationResponseValidator<IAuthUserArgEntity, IUserRegistrationItrEntity>
{
    public HasValidDisplayNameAuthUserArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IAuthUserArgEntity>
    {
        public Task<bool> IsValid(IAuthUserArgEntity arg) => Task.FromResult(arg.DisplayName.IzNotNullOrWhiteSpace());
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Display name cannot be empty";
    }
}
