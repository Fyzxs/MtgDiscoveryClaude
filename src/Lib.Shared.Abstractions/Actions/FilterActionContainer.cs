using System.Threading.Tasks;

namespace Lib.Shared.Abstractions.Actions;

public abstract class FilterActionContainer<TItem, TFailureStatus> : IFilterAction<TItem, TFailureStatus> where TFailureStatus : class
{
    private readonly IFilterAction<TItem, TFailureStatus>[] _actions;

    protected FilterActionContainer(IFilterAction<TItem, TFailureStatus>[] actions) => _actions = actions;

    public async Task<IFilterActionResult<TFailureStatus>> IsFilteredOut(TItem item)
    {
        foreach (IFilterAction<TItem, TFailureStatus> action in _actions)
        {
            IFilterActionResult<TFailureStatus> result = await action.IsFilteredOut(item).ConfigureAwait(false);
            if (result.IsFilteredOut()) return result;
        }

        return new FilterActionResult<TFailureStatus>();
    }
}