using Lib.Shared.Abstractions.Actions;
using Lib.Shared.Invocation.Queries;

namespace Lib.Shared.Invocation.Requests;

public interface IRequestValidatorAction<in TItem, TResponseType> : IValidatorAction<TItem, RequestOperationStatus<TResponseType>>;
