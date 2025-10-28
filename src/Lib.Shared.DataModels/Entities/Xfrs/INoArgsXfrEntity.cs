namespace Lib.Shared.DataModels.Entities.Xfrs;

/// <summary>
/// Marker entity representing operations that require no arguments at the adapter layer.
/// Used for parameterless adapter operations following the single-method delegation pattern.
/// </summary>
public interface INoArgsXfrEntity;
