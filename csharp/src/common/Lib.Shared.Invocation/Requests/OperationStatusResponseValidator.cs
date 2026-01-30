using System.Threading.Tasks;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.Invocation.Operations;

namespace Lib.Shared.Invocation.Requests;

public abstract class OperationStatusResponseValidator<TItem, TResponseType> : IValidatorAction<TItem, OperationResponse<TResponseType>>
{
    private readonly IValidator<TItem> _validator;
    private readonly string _message;

    protected OperationStatusResponseValidator(IValidator<TItem> validator, string message)
    {
        _validator = validator;
        _message = message;
    }

    public async Task<IValidatorActionResult<OperationResponse<TResponseType>>> Validate(TItem item)
    {
        if (await _validator.IsValid(item).ConfigureAwait(false)) return new ValidatorActionResult<OperationResponse<TResponseType>>();

        return new ValidatorActionResult<OperationResponse<TResponseType>>(new ValidatorFailedOperationResponse<TResponseType>(_message));
    }
}
