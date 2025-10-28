using Lib.Shared.DataModels.Entities.Xfrs;

namespace Lib.Aggregator.Sets.Queries.Entities;

/// <summary>
/// Concrete implementation of INoArgsXfrEntity for Aggregator layer.
/// Represents operations that require no input arguments at the adapter layer.
/// </summary>
internal sealed class NoArgsXfrEntity : INoArgsXfrEntity;
