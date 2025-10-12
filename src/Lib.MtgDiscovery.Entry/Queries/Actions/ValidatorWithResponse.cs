using System.Threading.Tasks;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.Invocation.Operations;
using Lib.Universal.Extensions;

namespace Lib.MtgDiscovery.Entry.Queries.Actions;

/// <summary>
/// Wrapper that adapts a validator to return an AsyncTryResult.
/// </summary>
internal sealed class ValidatorWithResponse<TToValidate, TResponseType> : IValidatorWithResponse<TToValidate, TResponseType>
{
    private readonly IValidatorAction<TToValidate, IOperationResponse<TResponseType>> _validator;

    public ValidatorWithResponse(IValidatorAction<TToValidate, IOperationResponse<TResponseType>> validator) => _validator = validator;

    public async Task<AsyncTryOut<IOperationResponse<TResponseType>>> TryValidate(TToValidate toValidate)
    {
        IValidatorActionResult<IOperationResponse<TResponseType>> validatorResult = await _validator.Validate(toValidate).ConfigureAwait(false);

        if (validatorResult.IsValid()) return new SuccessAsyncTryOut<IOperationResponse<TResponseType>>();

        IOperationResponse<TResponseType> failureResponse = new FailureOperationResponse<TResponseType>(validatorResult.FailureStatus().OuterException);
        return new FailureAsyncTryOut<IOperationResponse<TResponseType>>(failureResponse);

    }
}
