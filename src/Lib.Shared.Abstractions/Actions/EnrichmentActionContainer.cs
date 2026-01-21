using System.Threading.Tasks;

namespace Lib.Shared.Abstractions.Actions;

public abstract class EnrichmentActionContainer<TTarget> : IEnrichmentAction<TTarget>
{
    private readonly IEnrichmentAction<TTarget>[] _actions;

    protected EnrichmentActionContainer(params IEnrichmentAction<TTarget>[] actions) => _actions = actions;

    public async Task Enrich(TTarget target)
    {
        foreach (IEnrichmentAction<TTarget> action in _actions)
        {
            await action.Enrich(target).ConfigureAwait(false);
        }
    }
}
