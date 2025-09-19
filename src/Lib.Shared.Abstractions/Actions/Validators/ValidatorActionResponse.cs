using System.Threading.Tasks;
using Lib.Universal.Extensions;

namespace Lib.Shared.Abstractions.Actions.Validators;

public abstract class ValidatorActionResponse<TToValidate, TValidatorResponse, TReturnResponse> : IValidatorActionResponse<TToValidate, TReturnResponse>
{
    private readonly IValidatorAction<TToValidate, TValidatorResponse> _validator;

    protected ValidatorActionResponse(IValidatorAction<TToValidate, TValidatorResponse> validator)
    {
        _validator = validator;
    }

    public async Task<IAsyncTryOut<TReturnResponse>> TryIsValid(TToValidate toValidate)
    {
        IValidatorActionResult<TValidatorResponse> validatorResult = await _validator.Validate(toValidate).ConfigureAwait(false);
        if (validatorResult.IsNotValid()) return new FailureAsyncTryOut<TReturnResponse>(Value(validatorResult));
        return new SuccessAsyncTryOut<TReturnResponse>();
    }

    protected abstract TReturnResponse Value(IValidatorActionResult<TValidatorResponse> validatorResult);
}
