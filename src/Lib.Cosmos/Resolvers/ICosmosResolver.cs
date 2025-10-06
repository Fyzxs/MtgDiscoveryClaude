using Lib.Cosmos.Apis.Operators;
using Lib.Shared.Abstractions.Resolvers;

namespace Lib.Cosmos.Resolvers;

/// <summary>
/// Defines a resolver that resolves a Cosmos DB operation response to a result using provided context.
/// </summary>
/// <typeparam name="TResolved">The type of the resolved result.</typeparam>
/// <typeparam name="TContext">The type of context needed for resolution.</typeparam>
public interface ICosmosResolver<TResolved, in TContext> : IResolver<OpResponse<TResolved>, TResolved, TContext>
{
    // Inherits: TResolved Resolve(OpResponse<TResolved> input, TContext context);
}
