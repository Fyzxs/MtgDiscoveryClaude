using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Shared.Abstractions.Actions;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Lib.Universal.Extensions;

namespace Lib.MtgDiscovery.Entry.Queries.Validators.UserCards;

internal sealed class UserCardArgEntityValidatorContainer : ValidatorActionContainer<IUserCardArgEntity, IOperationResponse<IEnumerable<IUserCardOufEntity>>>, IUserCardArgEntityValidator
{
    public UserCardArgEntityValidatorContainer() : base([
            new IsNotNullUserCardArgEntityValidator(),
            new UserIdNotNullUserCardArgEntityValidator(),
            new HasValidUserIdUserCardArgEntityValidator(),
            new CardIdNotNullUserCardArgEntityValidator(),
            new HasValidCardIdUserCardArgEntityValidator()
        ])
    { }
}

internal sealed class IsNotNullUserCardArgEntityValidator : OperationResponseValidator<IUserCardArgEntity, IEnumerable<IUserCardOufEntity>>
{
    public IsNotNullUserCardArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IUserCardArgEntity>
    {
        public Task<bool> IsValid(IUserCardArgEntity arg) => Task.FromResult(arg is not null);
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Provided object is null";
    }
}

internal sealed class UserIdNotNullUserCardArgEntityValidator : OperationResponseValidator<IUserCardArgEntity, IEnumerable<IUserCardOufEntity>>
{
    public UserIdNotNullUserCardArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IUserCardArgEntity>
    {
        public Task<bool> IsValid(IUserCardArgEntity arg) => Task.FromResult(arg.UserId is not null);
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "User Id cannot be null";
    }
}

internal sealed class HasValidUserIdUserCardArgEntityValidator : OperationResponseValidator<IUserCardArgEntity, IEnumerable<IUserCardOufEntity>>
{
    public HasValidUserIdUserCardArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IUserCardArgEntity>
    {
        public Task<bool> IsValid(IUserCardArgEntity arg) => Task.FromResult(arg.UserId.IzNotNullOrWhiteSpace());
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "User Id cannot be empty";
    }
}

internal sealed class CardIdNotNullUserCardArgEntityValidator : OperationResponseValidator<IUserCardArgEntity, IEnumerable<IUserCardOufEntity>>
{
    public CardIdNotNullUserCardArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IUserCardArgEntity>
    {
        public Task<bool> IsValid(IUserCardArgEntity arg) => Task.FromResult(arg.CardId is not null);
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Card Id cannot be null";
    }
}

internal sealed class HasValidCardIdUserCardArgEntityValidator : OperationResponseValidator<IUserCardArgEntity, IEnumerable<IUserCardOufEntity>>
{
    public HasValidCardIdUserCardArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IUserCardArgEntity>
    {
        public Task<bool> IsValid(IUserCardArgEntity arg) => Task.FromResult(arg.CardId.IzNotNullOrWhiteSpace());
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Card Id cannot be empty";
    }
}
