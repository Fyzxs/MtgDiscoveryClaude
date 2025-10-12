using System.Threading.Tasks;
using Lib.Aggregator.UserSetCards.Entities;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.Validators.UserSetCards;

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