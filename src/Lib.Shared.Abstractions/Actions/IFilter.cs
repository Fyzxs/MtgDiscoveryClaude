using System.Threading.Tasks;

namespace Lib.Shared.Abstractions.Actions;

public interface IFilter<in TItem>
{
    const bool ValueFilteredOut = true;
    const bool ValueNotFilteredOut = false;

    Task<bool> IsFilteredOut(TItem item);
    async Task<bool> IsNotFilteredOut(TItem item) => await IsFilteredOut(item).ConfigureAwait(false) is false;
}

public interface IFilter<in TItem1, in TItem2>
{
    Task<bool> IsFilteredOut(TItem1 item1, TItem2 item2);
    async Task<bool> IsNotFilteredOut(TItem1 item1, TItem2 item2) => await IsFilteredOut(item1, item2).ConfigureAwait(false) is false;
}
