using System.Threading.Tasks;
using Lib.Universal.Extensions;

namespace Lib.Shared.Abstractions.Actions.Validators;

public abstract class ValidatorContainer<TItem> : IValidator<TItem>
{
    private readonly IValidator<TItem>[] _actions;

    protected ValidatorContainer(params IValidator<TItem>[] actions) => _actions = actions;

    public Task<bool> IsValid(TItem entity) => _actions.AllAsync(action => action.IsValid(entity));
}
