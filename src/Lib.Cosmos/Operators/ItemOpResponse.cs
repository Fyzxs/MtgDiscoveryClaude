using System.Net;
using Lib.Cosmos.Apis.Operators;
using Microsoft.Azure.Cosmos;

namespace Lib.Cosmos.Operators;

internal sealed class ItemOpResponse<T> : OpResponse<T>
{
    private readonly ItemResponse<T> _itemResponse;

    public ItemOpResponse(ItemResponse<T> itemResponse) => _itemResponse = itemResponse;

    public override T Value => _itemResponse.Resource;

    public override HttpStatusCode StatusCode => _itemResponse.StatusCode;
}
