using System;
using System.Threading.Tasks;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args.Collections;
using Lib.Shared.DataModels.Entities.Itrs.Collections;
using Lib.Shared.Invocation.Operations;
using Lib.Universal.Extensions;

namespace Lib.MtgDiscovery.Entry.Commands.Collections.Validators.Transfer;

internal sealed class HasValidTargetUserIdTransferCollectionOwnershipArgEntityValidator : OperationResponseValidator<ITransferCollectionOwnershipArgEntity, ITransferCollectionOwnershipItrEntity>
{
    public HasValidTargetUserIdTransferCollectionOwnershipArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<ITransferCollectionOwnershipArgEntity>
    {
        public Task<bool> IsValid(ITransferCollectionOwnershipArgEntity arg) =>
            Task.FromResult(arg.TargetUserId.IzNotNullOrWhiteSpace() && Guid.TryParse(arg.TargetUserId, out _));
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Target user ID must be a valid GUID";
    }
}
