using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Commands.Collections.Validators;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args.Collections;
using Lib.Shared.DataModels.Entities.Itrs.Collections;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Tests.Commands.Collections.Fakes;

internal sealed class CreateCollectionArgEntityValidatorFake : ICreateCollectionArgEntityValidator
{
    public IValidatorActionResult<IOperationResponse<ICollectionItrEntity>> ValidateResult { get; init; }
    public int ValidateInvokeCount { get; private set; }

    public Task<IValidatorActionResult<IOperationResponse<ICollectionItrEntity>>> Validate(ICreateCollectionArgEntity arg)
    {
        ValidateInvokeCount++;
        return Task.FromResult(ValidateResult);
    }
}
