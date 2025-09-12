using System.Threading.Tasks;

namespace Lib.Shared.Abstractions.Actions;

public abstract class FilterContainer<TItem1, TItem2> : IFilter<TItem1, TItem2>
{
    private const bool BeingFilteredOut = true;
    private const bool NotFilteredOut = false;

    private readonly IFilter<TItem1, TItem2>[] _actions;

    protected FilterContainer(params IFilter<TItem1, TItem2>[] actions) => _actions = actions;

    public async Task<bool> IsFilteredOut(TItem1 item1, TItem2 item2)
    {
        foreach (IFilter<TItem1, TItem2> filterAction in _actions)
        {
            if (await filterAction.IsFilteredOut(item1, item2).ConfigureAwait(false)) return BeingFilteredOut;
        }

        return NotFilteredOut;
    }
}
