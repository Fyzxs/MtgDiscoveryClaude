using System.Threading.Tasks;
using Lib.Shared.Abstractions.Actions;

namespace Lib.Shared.Invocation.Commands;

public sealed class DefaultTrueCommandValidatorAction<TItem> : ICommandValidatorAction<TItem>
{
    public Task<IValidatorActionResult<CommandOperationStatus>> Validate(TItem item) => Task.FromResult<IValidatorActionResult<CommandOperationStatus>>(new ValidatorActionResult<CommandOperationStatus>());
}
