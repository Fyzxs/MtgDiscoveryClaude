using Lib.Shared.Abstractions.Actions;

namespace Lib.Shared.Invocation.Commands;

public abstract class CommandValidatorActionContainer<T> : ValidatorActionContainer<T, CommandOperationStatus>
{
    protected CommandValidatorActionContainer(params IValidatorAction<T, CommandOperationStatus>[] actions) : base(actions)
    { }
}
