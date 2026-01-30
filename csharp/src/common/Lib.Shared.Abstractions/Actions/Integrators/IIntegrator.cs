using System.Threading.Tasks;

namespace Lib.Shared.Abstractions.Actions.Integrators;

/// <summary>
/// Defines an integrator that incorporates changes from a delta object into a state object,
/// returning a state object with the integrated changes.
/// </summary>
/// <typeparam name="TState">The type of the state object being modified.</typeparam>
/// <typeparam name="TDelta">The type of the delta/change object being integrated.</typeparam>
public interface IIntegrator<TState, in TDelta>
{
    /// <summary>
    /// Integrates the delta changes into the current state, returning a state object with the integrated changes.
    /// </summary>
    /// <param name="current">The current state object.</param>
    /// <param name="change">The delta/change object to integrate.</param>
    /// <returns>A state object with the integrated changes (maybe the same instance or a new instance).</returns>
    Task<TState> Integrate(TState current, TDelta change);
}
