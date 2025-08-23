using Lib.Shared.Abstractions.Actions;
using Lib.Shared.Invocation.Operations;

namespace Lib.Shared.Invocation.Requests;

public interface IRequestValidatorAction<in TItem, TResponseType> : IValidatorAction<TItem, OperationResponse<TResponseType>>;
