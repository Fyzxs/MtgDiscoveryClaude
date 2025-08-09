using Azure;
using Lib.BlobStorage.Apis.Operations.Responses;

namespace Lib.BlobStorage.Operations.Responses;

internal sealed class ResponseBlobOpResponse<T> : BlobOpResponse<T>
{
    private readonly Response<T> _response;

    public ResponseBlobOpResponse(Response<T> response)
    {
        _response = response;
    }

    public override T Value => _response.Value;
    public override bool HasValue()
    {
        return _response.HasValue;
    }
}
