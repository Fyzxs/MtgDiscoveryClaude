using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Shared.Abstractions.Actions;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;
using Lib.Universal.Extensions;

namespace Lib.MtgDiscovery.Entry.Queries.Validators.UserCards;

internal sealed class UserCardsSetArgEntityValidatorContainer : ValidatorActionContainer<IUserCardsSetArgEntity, IOperationResponse<IEnumerable<IUserCardItrEntity>>>, IUserCardsSetArgEntityValidator
{
    public UserCardsSetArgEntityValidatorContainer() : base([
            new IsNotNullUserCardsSetArgEntityValidator(),
            new SetIdNotNullUserCardsSetArgEntityValidator(),
            new HasValidSetIdUserCardsSetArgEntityValidator(),
            new UserIdNotNullUserCardsSetArgEntityValidator(),
            new HasValidUserIdUserCardsSetArgEntityValidator()
        ])
    { }
}

internal sealed class IsNotNullUserCardsSetArgEntityValidator : OperationResponseValidator<IUserCardsSetArgEntity, IEnumerable<IUserCardItrEntity>>
{
    public IsNotNullUserCardsSetArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IUserCardsSetArgEntity>
    {
        public Task<bool> IsValid(IUserCardsSetArgEntity arg) => Task.FromResult(arg is not null);
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Provided object is null";
    }
}

internal sealed class SetIdNotNullUserCardsSetArgEntityValidator : OperationResponseValidator<IUserCardsSetArgEntity, IEnumerable<IUserCardItrEntity>>
{
    public SetIdNotNullUserCardsSetArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IUserCardsSetArgEntity>
    {
        public Task<bool> IsValid(IUserCardsSetArgEntity arg) => Task.FromResult(arg.SetId is not null);
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Set Id cannot be null";
    }
}

internal sealed class HasValidSetIdUserCardsSetArgEntityValidator : OperationResponseValidator<IUserCardsSetArgEntity, IEnumerable<IUserCardItrEntity>>
{
    public HasValidSetIdUserCardsSetArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IUserCardsSetArgEntity>
    {
        public Task<bool> IsValid(IUserCardsSetArgEntity arg) => Task.FromResult(arg.SetId.IzNotNullOrWhiteSpace());
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Set Id cannot be empty";
    }
}

internal sealed class UserIdNotNullUserCardsSetArgEntityValidator : OperationResponseValidator<IUserCardsSetArgEntity, IEnumerable<IUserCardItrEntity>>
{
    public UserIdNotNullUserCardsSetArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IUserCardsSetArgEntity>
    {
        public Task<bool> IsValid(IUserCardsSetArgEntity arg) => Task.FromResult(arg.UserId is not null);
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "User Id cannot be null";
    }
}

internal sealed class HasValidUserIdUserCardsSetArgEntityValidator : OperationResponseValidator<IUserCardsSetArgEntity, IEnumerable<IUserCardItrEntity>>
{
    public HasValidUserIdUserCardsSetArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IUserCardsSetArgEntity>
    {
        public Task<bool> IsValid(IUserCardsSetArgEntity arg) => Task.FromResult(arg.UserId.IzNotNullOrWhiteSpace());
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "User Id cannot be empty";
    }
}
