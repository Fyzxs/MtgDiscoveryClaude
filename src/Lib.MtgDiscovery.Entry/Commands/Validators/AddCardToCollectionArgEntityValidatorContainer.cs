using System;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Lib.Universal.Extensions;

namespace Lib.MtgDiscovery.Entry.Commands.Validators;

internal sealed class AddCardToCollectionArgEntityValidatorContainer : ValidatorActionContainer<IAddCardToCollectionArgsEntity, IOperationResponse<IUserCardOufEntity>>, IAddCardToCollectionArgEntityValidator
{
    public AddCardToCollectionArgEntityValidatorContainer() : base([
            new HasValidCardIdAddCardToCollectionArgEntityValidator(),
            new HasValidSetIdAddCardToCollectionArgEntityValidator(),
            new HasValidUserIdAddCardToCollectionArgEntityValidator(),
            new AuthUserMatchesUserIdValidator(),
            new CollectedItemNotNullValidator(),
            new CollectedItemCountValidator(),
            new CollectedItemFinishValidator(),
            new CollectedItemSpecialValidator(),
        ])
    { }
}

internal sealed class HasValidCardIdAddCardToCollectionArgEntityValidator : OperationResponseValidator<IAddCardToCollectionArgsEntity, IUserCardOufEntity>
{
    public HasValidCardIdAddCardToCollectionArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IAddCardToCollectionArgsEntity>
    {
        public Task<bool> IsValid(IAddCardToCollectionArgsEntity arg) => Task.FromResult(arg.AddUserCard.CardId.IzNotNullOrWhiteSpace());
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Card ID cannot be empty";
    }
}

internal sealed class HasValidSetIdAddCardToCollectionArgEntityValidator : OperationResponseValidator<IAddCardToCollectionArgsEntity, IUserCardOufEntity>
{
    public HasValidSetIdAddCardToCollectionArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IAddCardToCollectionArgsEntity>
    {
        public Task<bool> IsValid(IAddCardToCollectionArgsEntity arg) => Task.FromResult(arg.AddUserCard.SetId.IzNotNullOrWhiteSpace());
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Set ID cannot be empty";
    }
}

internal sealed class HasValidUserIdAddCardToCollectionArgEntityValidator : OperationResponseValidator<IAddCardToCollectionArgsEntity, IUserCardOufEntity>
{
    public HasValidUserIdAddCardToCollectionArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IAddCardToCollectionArgsEntity>
    {
        public Task<bool> IsValid(IAddCardToCollectionArgsEntity arg) => Task.FromResult(arg.AddUserCard.UserId.IzNotNullOrWhiteSpace());
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "User ID cannot be empty";
    }
}

internal sealed class AuthUserMatchesUserIdValidator : OperationResponseValidator<IAddCardToCollectionArgsEntity, IUserCardOufEntity>
{
    public AuthUserMatchesUserIdValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IAddCardToCollectionArgsEntity>
    {
        public Task<bool> IsValid(IAddCardToCollectionArgsEntity arg) =>
            Task.FromResult(string.Equals(arg.AuthUser.UserId, arg.AddUserCard.UserId, StringComparison.OrdinalIgnoreCase));
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Authenticated user does not match the provided user ID";
    }
}

