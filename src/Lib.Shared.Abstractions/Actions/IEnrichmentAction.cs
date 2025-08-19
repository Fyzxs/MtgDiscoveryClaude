using System.Threading.Tasks;

namespace Lib.Shared.Abstractions.Actions;

public interface IEnrichmentAction<in TTarget>
{
    Task Enrich(TTarget target);
}