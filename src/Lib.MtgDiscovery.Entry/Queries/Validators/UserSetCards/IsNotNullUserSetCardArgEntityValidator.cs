using System.Threading.Tasks;
using Lib.Aggregator.UserSetCards.Entities;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.Validators.UserSetCards;

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