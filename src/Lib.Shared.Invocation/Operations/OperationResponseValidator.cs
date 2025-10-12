using System.Threading.Tasks;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.Invocation.Exceptions;

namespace Lib.Shared.Invocation.Operations;

public abstract class OperationResponseValidator<TValidationType, TReturnType> : IValidatorAction<TValidationType, IOperationResponse<TReturnType>>
{
    private readonly IValidator<TValidationType> _validator;
    private readonly OperationResponseMessage _message;

    protected OperationResponseValidator(IValidator<TValidationType> validator, OperationResponseMessage message)
    {
        _validator = validator;
        _message = message;
    }
    public async Task<IValidatorActionResult<IOperationResponse<TReturnType>>> Validate(TValidationType item)
    {
        if (await _validator.IsValid(item).ConfigureAwait(false)) return new ValidatorActionResult<IOperationResponse<TReturnType>>();

        return new ValidatorActionResult<IOperationResponse<TReturnType>>(new FailureOperationResponse<TReturnType>(new BadRequestOperationException(_message)));
    }
}
