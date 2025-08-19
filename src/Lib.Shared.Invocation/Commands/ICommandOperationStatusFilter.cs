using Lib.Shared.Abstractions.Actions;

namespace Lib.Shared.Invocation.Commands;

public interface ICommandOperationStatusFilter<in TItem> : IFilter<TItem>;
