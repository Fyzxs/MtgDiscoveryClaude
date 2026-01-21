namespace Lib.Shared.Abstractions.Resolvers;

/// <summary>
/// Defines a resolver that resolves an input to a result using provided context.
/// </summary>
/// <typeparam name="TInput">The type of input to resolve from.</typeparam>
/// <typeparam name="TResolved">The type of the resolved result.</typeparam>
/// <typeparam name="TContext">The type of context needed for resolution.</typeparam>
public interface IResolver<in TInput, out TResolved, in TContext>
{
    /// <summary>
    /// Resolves the input to a result using the provided context.
    /// </summary>
    /// <param name="input">The input to resolve.</param>
    /// <param name="context">The context needed for resolution.</param>
    /// <returns>The resolved result.</returns>
    TResolved Resolve(TInput input, TContext context);
}
