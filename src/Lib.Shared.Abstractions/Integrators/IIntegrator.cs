namespace Lib.Shared.Abstractions.Integrators;

/// <summary>
/// Defines an integrator that incorporates changes from a delta object into a state object,
/// returning a new state object with the integrated changes.
/// </summary>
/// <typeparam name="TState">The type of the state object being modified.</typeparam>
/// <typeparam name="TDelta">The type of the delta/change object being integrated.</typeparam>
public interface IIntegrator<TState, in TDelta>
{
    /// <summary>
    /// Integrates the delta changes into the current state, returning a new state object.
    /// </summary>
    /// <param name="current">The current state object.</param>
    /// <param name="change">The delta/change object to integrate.</param>
    /// <returns>A new state object with the integrated changes.</returns>
    TState Integrate(TState current, TDelta change);
}
