using System;
using Lib.BlobStorage.Apis.Operations.Responses;

namespace Lib.BlobStorage.Operations.Responses;

internal sealed class FailureBlobOpResponse<T> : BlobOpResponse<T>
{
    public override T Value => throw new InvalidOperationException($"Cannot retrieve Value from {nameof(FailureBlobOpResponse<T>)}");
    public override bool HasValue() => false;
}
