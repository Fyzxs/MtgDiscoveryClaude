using System.Threading.Tasks;

namespace Lib.Shared.Abstractions.Actions.Filters;

public interface IFilterAction<in TItem, TFailureStatus>
{
    const bool ValueFilteredOut = true;
    const bool ValueNotFilteredOut = false;

    Task<IFilterActionResult<TFailureStatus>> IsFilteredOut(TItem item);
}
