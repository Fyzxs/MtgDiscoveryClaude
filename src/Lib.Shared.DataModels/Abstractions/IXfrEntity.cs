namespace Lib.Shared.DataModels.Abstractions;

/// <summary>
/// Base marker interface for all transfer entities in the adapter layer.
/// Transfer entities are used within adapter layer operations.
/// All XfrEntities are cacheable by design.
/// Data flow: Within Adapter Layer (coordinates between aggregator and external systems)
/// </summary>
public interface IXfrEntity : ICacheableEntity
{
    // Marker interface - inherits cacheability requirement
}
