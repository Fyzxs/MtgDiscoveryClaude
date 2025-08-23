//using System.Threading.Tasks;
//using Lib.Shared.Abstractions.Actions;

//namespace Lib.Shared.Invocation.Commands;

//public abstract class CommandOperationStatusResponseFilter<TItem> : IFilterAction<TItem, ICommandOperationResponse>
//{
//    private readonly ICommandOperationStatusFilter<TItem> _filter;
//    private readonly ICommandOperationStatusContent _content;

//    protected CommandOperationStatusResponseFilter(ICommandOperationStatusFilter<TItem> filter, ICommandOperationStatusContent content)
//    {
//        _filter = filter;
//        _content = content;
//    }

//    public async Task<IFilterActionResult<ICommandOperationResponse>> IsFilteredOut(TItem item)
//    {
//        bool filterResult = await _filter.IsFilteredOut(item).ConfigureAwait(false);
//        bool isNotFilteredOut = filterResult == IFilterAction<TItem, ICommandOperationResponse>.ValueNotFilteredOut;
//        if (isNotFilteredOut) return new FilterActionResult<ICommandOperationResponse>();

//        return new FilterActionResult<ICommandOperationResponse>(new FilterFailedCommandOperationResponse(_content.Message(), _content.Status()));
//    }
//}
