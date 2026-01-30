using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args.Collections;
using Lib.Shared.DataModels.Entities.Itrs.Collections;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Commands.Collections.Validators.Transfer;

internal sealed class TransferCollectionOwnershipArgEntityValidatorContainer : ValidatorActionContainer<ITransferCollectionOwnershipArgEntity, IOperationResponse<ITransferCollectionOwnershipItrEntity>>, ITransferCollectionOwnershipArgEntityValidator
{
    public TransferCollectionOwnershipArgEntityValidatorContainer() : base([
            new IsNotNullTransferCollectionOwnershipArgEntityValidator(),
            new HasValidCollectionIdTransferCollectionOwnershipArgEntityValidator(),
            new HasValidTargetUserIdTransferCollectionOwnershipArgEntityValidator(),
        ])
    { }
}
