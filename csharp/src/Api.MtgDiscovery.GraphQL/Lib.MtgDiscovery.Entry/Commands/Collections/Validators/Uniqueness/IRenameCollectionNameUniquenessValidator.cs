using Lib.MtgDiscovery.Entry.Entities.Collections;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Oufs.Collections;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Commands.Collections.Validators.Uniqueness;

internal interface IRenameCollectionNameUniquenessValidator
    : IValidatorAction<IRenameCollectionArgsEntity, IOperationResponse<ICollectionOufEntity>>
{
}
