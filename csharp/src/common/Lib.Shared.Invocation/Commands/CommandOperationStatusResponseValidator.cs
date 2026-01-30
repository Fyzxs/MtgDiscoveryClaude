//using System.Threading.Tasks;
//using Lib.Shared.Abstractions.Actions;

//namespace Lib.Shared.Invocation.Commands;

//public abstract class CommandOperationStatusResponseValidator<TItem> : IValidatorAction<TItem, CommandOperationResponse>
//{
//    private readonly ICommandOperationStatusValidator<TItem> _validator;
//    private readonly CommandOperationStatusMessage _content;

//    protected CommandOperationStatusResponseValidator(ICommandOperationStatusValidator<TItem> validator, CommandOperationStatusMessage content)
//    {
//        _validator = validator;
//        _content = content;
//    }

//    public async Task<IValidatorActionResult<CommandOperationResponse>> Validate(TItem item)
//    {
//        if (await _validator.IsValid(item).ConfigureAwait(false)) return new ValidatorActionResult<CommandOperationResponse>();

//        return new ValidatorActionResult<CommandOperationResponse>(new ValidatorFailedCommandOperationResponse(_content));
//    }
//}
