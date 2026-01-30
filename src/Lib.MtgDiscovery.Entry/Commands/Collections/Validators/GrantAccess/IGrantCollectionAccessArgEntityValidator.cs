using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args.Collections;
using Lib.Shared.DataModels.Entities.Itrs.Collections;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Commands.Collections.Validators.GrantAccess;

internal interface IGrantCollectionAccessArgEntityValidator : IValidatorAction<IGrantCollectionAccessArgEntity, IOperationResponse<IGrantCollectionAccessItrEntity>>
{
}
