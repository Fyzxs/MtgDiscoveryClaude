using System.Threading.Tasks;
using Lib.Shared.Abstractions.Actions;
using Lib.Shared.Invocation.Queries;

namespace Lib.Shared.Invocation.Requests;

public abstract class RequestOperationStatusResponseValidator<TItem, TResponseType> : IValidatorAction<TItem, RequestOperationStatus<TResponseType>>
{
    private readonly IValidator<TItem> _validator;
    private readonly string _message;

    protected RequestOperationStatusResponseValidator(IValidator<TItem> validator, string message)
    {
        _validator = validator;
        _message = message;
    }

    public async Task<IValidatorActionResult<RequestOperationStatus<TResponseType>>> Validate(TItem item)
    {
        if (await _validator.IsValid(item).ConfigureAwait(false)) return new ValidatorActionResult<RequestOperationStatus<TResponseType>>();

        return new ValidatorActionResult<RequestOperationStatus<TResponseType>>(new ValidatorFailedResponseOperationStatus<TResponseType>(_message));
    }
}
