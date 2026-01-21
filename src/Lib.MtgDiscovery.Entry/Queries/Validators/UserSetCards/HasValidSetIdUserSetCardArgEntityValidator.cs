using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.Invocation.Operations;
using Lib.Universal.Extensions;

namespace Lib.MtgDiscovery.Entry.Queries.Validators.UserSetCards;

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