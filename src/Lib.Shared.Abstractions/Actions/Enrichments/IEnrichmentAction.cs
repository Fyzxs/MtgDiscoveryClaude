using System.Threading.Tasks;

namespace Lib.Shared.Abstractions.Actions.Enrichments;

public interface IEnrichmentAction<in TTarget>
{
    Task Enrich(TTarget target);
}

public interface IEnrichmentAction<in TTarget, in T1>
{
    Task Enrich(TTarget target, T1 t1);
}
