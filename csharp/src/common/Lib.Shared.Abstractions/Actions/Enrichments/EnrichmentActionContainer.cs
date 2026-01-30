using System.Threading.Tasks;

namespace Lib.Shared.Abstractions.Actions.Enrichments;

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

public abstract class EnrichmentActionContainer<TTarget, T1> : IEnrichmentAction<TTarget, T1>
{
    private readonly IEnrichmentAction<TTarget, T1>[] _actions;

    protected EnrichmentActionContainer(params IEnrichmentAction<TTarget, T1>[] actions) => _actions = actions;

    public async Task Enrich(TTarget target, T1 t1)
    {
        foreach (IEnrichmentAction<TTarget, T1> action in _actions)
        {
            await action.Enrich(target, t1).ConfigureAwait(false);
        }
    }
}
