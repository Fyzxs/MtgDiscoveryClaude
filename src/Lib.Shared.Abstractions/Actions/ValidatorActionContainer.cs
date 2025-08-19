using System.Threading.Tasks;

namespace Lib.Shared.Abstractions.Actions;

public abstract class ValidatorActionContainer<TItem, TFailureStatus> : IValidatorAction<TItem, TFailureStatus> where TFailureStatus : class
{
    private readonly IValidatorAction<TItem, TFailureStatus>[] _actions;

    protected ValidatorActionContainer(IValidatorAction<TItem, TFailureStatus>[] actions) => _actions = actions;

    public async Task<IValidatorActionResult<TFailureStatus>> Validate(TItem item)
    {
        foreach (IValidatorAction<TItem, TFailureStatus> action in _actions)
        {
            IValidatorActionResult<TFailureStatus> result = await action.Validate(item).ConfigureAwait(false);
            if (result.IsNotValid()) return result;
        }

        return new ValidatorActionResult<TFailureStatus>();
    }
}