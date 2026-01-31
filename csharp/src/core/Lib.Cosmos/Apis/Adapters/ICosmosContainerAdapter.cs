using Lib.Cosmos.Apis.Operators;

namespace Lib.Cosmos.Apis.Adapters;

/// <summary>
/// Adapter interface for Cosmos DB container operations that combines all operator interfaces.
/// </summary>
public interface ICosmosContainerAdapter :
    ICosmosContainerReadOperator,
    ICosmosContainerDeleteOperator,
    ICosmosContainerUpsertOperator,
    ICosmosContainerQueryOperator,
    ICosmosContainerQueryAsyncOperator;
