using System.Threading;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities.Collections;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Oufs.Collections;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Commands.Collections.Validators.Uniqueness;

internal interface ICreateCollectionNameUniquenessValidator
    : IValidatorAction<ICreateCollectionArgsEntity, IOperationResponse<ICollectionOufEntity>>
{
    Task<IValidatorActionResult<IOperationResponse<ICollectionOufEntity>>> Validate(ICreateCollectionArgsEntity item, CancellationToken cancellationToken);
}
