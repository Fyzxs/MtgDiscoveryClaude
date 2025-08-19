using System.Threading.Tasks;
using Lib.Shared.Abstractions.Actions;

namespace Lib.Shared.Invocation.Commands;

public abstract class CommandOperationStatusResponseValidator<TItem> : IValidatorAction<TItem, CommandOperationStatus>
{
    private readonly ICommandOperationStatusValidator<TItem> _validator;
    private readonly CommandOperationStatusMessage _content;

    protected CommandOperationStatusResponseValidator(ICommandOperationStatusValidator<TItem> validator, CommandOperationStatusMessage content)
    {
        _validator = validator;
        _content = content;
    }

    public async Task<IValidatorActionResult<CommandOperationStatus>> Validate(TItem item)
    {
        if (await _validator.IsValid(item).ConfigureAwait(false)) return new ValidatorActionResult<CommandOperationStatus>();

        return new ValidatorActionResult<CommandOperationStatus>(new ValidatorFailedCommandOperationStatus(_content));
    }
}
