using Lib.Cosmos.Apis.Operators;
using Lib.Shared.Invocation.Commands;

namespace Lib.Shared.Invocation.Adapters;

public interface ICosmosRequestStatusFactory
{
    CommandOperationStatus CosmosSuccess();
    CommandOperationStatus CosmosFailure<TCosmosItem>(OpResponse<TCosmosItem> cosmosResponse);
}
