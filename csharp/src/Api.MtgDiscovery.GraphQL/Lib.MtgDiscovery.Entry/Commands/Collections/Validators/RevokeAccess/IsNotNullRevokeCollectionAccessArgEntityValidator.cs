using System.Threading.Tasks;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args.Collections;
using Lib.Shared.DataModels.Entities.Itrs.Collections;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Commands.Collections.Validators.RevokeAccess;

internal sealed class IsNotNullRevokeCollectionAccessArgEntityValidator : OperationResponseValidator<IRevokeCollectionAccessArgEntity, IRevokeCollectionAccessItrEntity>
{
    public IsNotNullRevokeCollectionAccessArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IRevokeCollectionAccessArgEntity>
    {
        public Task<bool> IsValid(IRevokeCollectionAccessArgEntity arg) => Task.FromResult(arg is not null);
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Revoke collection access argument cannot be null";
    }
}
