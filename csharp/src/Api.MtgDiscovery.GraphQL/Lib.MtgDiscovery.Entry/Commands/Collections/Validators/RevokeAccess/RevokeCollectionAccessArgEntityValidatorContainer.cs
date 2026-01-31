using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args.Collections;
using Lib.Shared.DataModels.Entities.Itrs.Collections;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Commands.Collections.Validators.RevokeAccess;

internal sealed class RevokeCollectionAccessArgEntityValidatorContainer : ValidatorActionContainer<IRevokeCollectionAccessArgEntity, IOperationResponse<IRevokeCollectionAccessItrEntity>>, IRevokeCollectionAccessArgEntityValidator
{
    public RevokeCollectionAccessArgEntityValidatorContainer() : base([
            new IsNotNullRevokeCollectionAccessArgEntityValidator(),
            new HasValidCollectionIdRevokeCollectionAccessArgEntityValidator(),
            new HasValidTargetUserIdRevokeCollectionAccessArgEntityValidator(),
        ])
    { }
}
