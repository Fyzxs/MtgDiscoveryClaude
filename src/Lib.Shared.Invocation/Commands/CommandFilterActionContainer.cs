using Lib.Shared.Abstractions.Actions;

namespace Lib.Shared.Invocation.Commands;

public abstract class CommandFilterActionContainer<T> : FilterActionContainer<T, CommandOperationStatus>
{
    protected CommandFilterActionContainer(params IFilterAction<T, CommandOperationStatus>[] actions) : base(actions)
    { }
}