using System.Threading.Tasks;
using Lib.Shared.Abstractions.Actions;

namespace Lib.Shared.Invocation.Commands;

public abstract class CommandOperationStatusResponseFilter<TItem> : IFilterAction<TItem, CommandOperationStatus>
{
    private readonly ICommandOperationStatusFilter<TItem> _filter;
    private readonly ICommandOperationStatusContent _content;

    protected CommandOperationStatusResponseFilter(ICommandOperationStatusFilter<TItem> filter, ICommandOperationStatusContent content)
    {
        _filter = filter;
        _content = content;
    }

    public async Task<IFilterActionResult<CommandOperationStatus>> IsFilteredOut(TItem item)
    {
        bool filterResult = await _filter.IsFilteredOut(item).ConfigureAwait(false);
        bool isNotFilteredOut = filterResult == IFilterAction<TItem, CommandOperationStatus>.ValueNotFilteredOut;
        if (isNotFilteredOut) return new FilterActionResult<CommandOperationStatus>();

        return new FilterActionResult<CommandOperationStatus>(new FilterFailedCommandOperationStatus(_content.Message(), _content.Status()));
    }
}
