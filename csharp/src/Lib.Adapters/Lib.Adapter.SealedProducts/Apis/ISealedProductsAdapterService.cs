namespace Lib.Adapter.SealedProducts.Apis;

/// <summary>
/// Composite service interface for sealed products adapter operations.
/// Inherits from specialized query adapter interfaces following the 3-tier pattern:
///   AdapterService → QueryAdapter/CommandAdapter → Single-Operation Adapters
/// </summary>
public interface ISealedProductsAdapterService : ISealedProductsQueryAdapter;
