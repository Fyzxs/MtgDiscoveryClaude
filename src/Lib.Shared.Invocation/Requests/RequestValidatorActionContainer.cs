using Lib.Shared.Abstractions.Actions;
using Lib.Shared.Invocation.Queries;

namespace Lib.Shared.Invocation.Requests;

public abstract class RequestValidatorActionContainer<TItem, TResponseType> : ValidatorActionContainer<TItem, RequestOperationStatus<TResponseType>>
{
    protected RequestValidatorActionContainer(params IValidatorAction<TItem, RequestOperationStatus<TResponseType>>[] actions) : base(actions)
    { }
}