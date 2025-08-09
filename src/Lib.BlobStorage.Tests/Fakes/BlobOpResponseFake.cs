using Lib.BlobStorage.Apis.Operations.Responses;

namespace Lib.BlobStorage.Tests.Fakes;

internal sealed class BlobOpResponseFake<T> : BlobOpResponse<T>
{
    public T ValueResult { get; init; }
    public int HasValueInvokeCount { get; private set; }
    public bool HasValueResult { get; init; }

    public BlobOpResponseFake()
    {
        // Defaults for test flexibility
        ValueResult = default!;
        HasValueResult = true;
    }

    public override T Value => ValueResult;
    public override bool HasValue()
    {
        HasValueInvokeCount++;
        return HasValueResult;
    }
}
