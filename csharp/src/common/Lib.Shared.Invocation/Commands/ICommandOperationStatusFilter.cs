using Lib.Shared.Abstractions.Actions.Filters;

namespace Lib.Shared.Invocation.Commands;

public interface ICommandOperationStatusFilter<in TItem> : IFilter<TItem>;
