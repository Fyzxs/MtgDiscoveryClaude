using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.Shared.Abstractions.Actions;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Lib.Universal.Extensions;

namespace Lib.MtgDiscovery.Entry.Queries.Validators.UserCards;

internal sealed class UserCardsByIdsArgEntityValidatorContainer : ValidatorActionContainer<IUserCardsByIdsArgEntity, IOperationResponse<IEnumerable<IUserCardOufEntity>>>, IUserCardsByIdsArgEntityValidator
{
    public UserCardsByIdsArgEntityValidatorContainer() : base([
            new IsNotNullUserCardsByIdsArgEntityValidator(),
            new UserIdNotNullUserCardsByIdsArgEntityValidator(),
            new HasValidUserIdUserCardsByIdsArgEntityValidator(),
            new CardIdsNotNullUserCardsByIdsArgEntityValidator(),
            new CardIdsNotEmptyUserCardsByIdsArgEntityValidator(),
            new ValidCardIdsUserCardsByIdsArgEntityValidator()
        ])
    { }
}

internal sealed class IsNotNullUserCardsByIdsArgEntityValidator : OperationResponseValidator<IUserCardsByIdsArgEntity, IEnumerable<IUserCardOufEntity>>
{
    public IsNotNullUserCardsByIdsArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IUserCardsByIdsArgEntity>
    {
        public Task<bool> IsValid(IUserCardsByIdsArgEntity arg) => Task.FromResult(arg is not null);
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Provided object cannot be null";
    }
}

internal sealed class UserIdNotNullUserCardsByIdsArgEntityValidator : OperationResponseValidator<IUserCardsByIdsArgEntity, IEnumerable<IUserCardOufEntity>>
{
    public UserIdNotNullUserCardsByIdsArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IUserCardsByIdsArgEntity>
    {
        public Task<bool> IsValid(IUserCardsByIdsArgEntity arg) => Task.FromResult(arg.UserId is not null);
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "User Id cannot be null";
    }
}

internal sealed class HasValidUserIdUserCardsByIdsArgEntityValidator : OperationResponseValidator<IUserCardsByIdsArgEntity, IEnumerable<IUserCardOufEntity>>
{
    public HasValidUserIdUserCardsByIdsArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IUserCardsByIdsArgEntity>
    {
        public Task<bool> IsValid(IUserCardsByIdsArgEntity arg) => Task.FromResult(arg.UserId.IzNotNullOrWhiteSpace());
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "User Id cannot be empty";
    }
}

internal sealed class CardIdsNotNullUserCardsByIdsArgEntityValidator : OperationResponseValidator<IUserCardsByIdsArgEntity, IEnumerable<IUserCardOufEntity>>
{
    public CardIdsNotNullUserCardsByIdsArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IUserCardsByIdsArgEntity>
    {
        public Task<bool> IsValid(IUserCardsByIdsArgEntity arg) => Task.FromResult(arg.CardIds is not null);
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Card Ids cannot be null";
    }
}

internal sealed class CardIdsNotEmptyUserCardsByIdsArgEntityValidator : OperationResponseValidator<IUserCardsByIdsArgEntity, IEnumerable<IUserCardOufEntity>>
{
    public CardIdsNotEmptyUserCardsByIdsArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IUserCardsByIdsArgEntity>
    {
        public Task<bool> IsValid(IUserCardsByIdsArgEntity arg) => Task.FromResult(arg.CardIds.Count > 0);
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Card Ids cannot be empty";
    }
}

internal sealed class ValidCardIdsUserCardsByIdsArgEntityValidator : OperationResponseValidator<IUserCardsByIdsArgEntity, IEnumerable<IUserCardOufEntity>>
{
    public ValidCardIdsUserCardsByIdsArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IUserCardsByIdsArgEntity>
    {
        public Task<bool> IsValid(IUserCardsByIdsArgEntity arg) => Task.FromResult(arg.CardIds.All(id => id.IzNotNullOrWhiteSpace()));
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "All Card Ids must be valid";
    }
}
