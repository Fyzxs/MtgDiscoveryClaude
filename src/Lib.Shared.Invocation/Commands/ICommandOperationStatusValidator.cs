using Lib.Shared.Abstractions.Actions.Validators;

namespace Lib.Shared.Invocation.Commands;

public interface ICommandOperationStatusValidator<in TItem> : IValidator<TItem>;
