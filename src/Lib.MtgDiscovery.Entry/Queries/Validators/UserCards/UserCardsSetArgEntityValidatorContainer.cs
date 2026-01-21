using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Lib.Universal.Extensions;

namespace Lib.MtgDiscovery.Entry.Queries.Validators.UserCards;

internal sealed class UserCardsSetArgEntityValidatorContainer : ValidatorActionContainer<IUserCardsBySetArgEntity, IOperationResponse<IEnumerable<IUserCardOufEntity>>>, IUserCardsSetArgEntityValidator
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

internal sealed class IsNotNullUserCardsSetArgEntityValidator : OperationResponseValidator<IUserCardsBySetArgEntity, IEnumerable<IUserCardOufEntity>>
{
    public IsNotNullUserCardsSetArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IUserCardsBySetArgEntity>
    {
        public Task<bool> IsValid(IUserCardsBySetArgEntity arg) => Task.FromResult(arg is not null);
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Provided object is null";
    }
}

internal sealed class SetIdNotNullUserCardsSetArgEntityValidator : OperationResponseValidator<IUserCardsBySetArgEntity, IEnumerable<IUserCardOufEntity>>
{
    public SetIdNotNullUserCardsSetArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IUserCardsBySetArgEntity>
    {
        public Task<bool> IsValid(IUserCardsBySetArgEntity arg) => Task.FromResult(arg.SetId is not null);
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Set Id cannot be null";
    }
}

internal sealed class HasValidSetIdUserCardsSetArgEntityValidator : OperationResponseValidator<IUserCardsBySetArgEntity, IEnumerable<IUserCardOufEntity>>
{
    public HasValidSetIdUserCardsSetArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IUserCardsBySetArgEntity>
    {
        public Task<bool> IsValid(IUserCardsBySetArgEntity arg) => Task.FromResult(arg.SetId.IzNotNullOrWhiteSpace());
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Set Id cannot be empty";
    }
}

internal sealed class UserIdNotNullUserCardsSetArgEntityValidator : OperationResponseValidator<IUserCardsBySetArgEntity, IEnumerable<IUserCardOufEntity>>
{
    public UserIdNotNullUserCardsSetArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IUserCardsBySetArgEntity>
    {
        public Task<bool> IsValid(IUserCardsBySetArgEntity arg) => Task.FromResult(arg.UserId is not null);
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "User Id cannot be null";
    }
}

internal sealed class HasValidUserIdUserCardsSetArgEntityValidator : OperationResponseValidator<IUserCardsBySetArgEntity, IEnumerable<IUserCardOufEntity>>
{
    public HasValidUserIdUserCardsSetArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IUserCardsBySetArgEntity>
    {
        public Task<bool> IsValid(IUserCardsBySetArgEntity arg) => Task.FromResult(arg.UserId.IzNotNullOrWhiteSpace());
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "User Id cannot be empty";
    }
}
