using System;
using System.Threading.Tasks;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args.Collections;
using Lib.Shared.DataModels.Entities.Itrs.Collections;
using Lib.Shared.Invocation.Operations;
using Lib.Universal.Extensions;

namespace Lib.MtgDiscovery.Entry.Commands.Collections.Validators.RevokeAccess;

internal sealed class HasValidTargetUserIdRevokeCollectionAccessArgEntityValidator : OperationResponseValidator<IRevokeCollectionAccessArgEntity, IRevokeCollectionAccessItrEntity>
{
    public HasValidTargetUserIdRevokeCollectionAccessArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IRevokeCollectionAccessArgEntity>
    {
        public Task<bool> IsValid(IRevokeCollectionAccessArgEntity arg) => Task.FromResult(arg.TargetUserId.IzNotNullOrWhiteSpace() && Guid.TryParse(arg.TargetUserId, out _));
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Target user ID must be a valid GUID";
    }
}
