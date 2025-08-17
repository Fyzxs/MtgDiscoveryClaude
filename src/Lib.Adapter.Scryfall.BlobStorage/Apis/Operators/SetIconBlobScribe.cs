using Lib.Adapter.Scryfall.BlobStorage.Containers;
using Lib.BlobStorage.Apis.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Scryfall.BlobStorage.Apis.Operators;

public sealed class SetIconBlobScribe : BlobWriteScribe
{
    public SetIconBlobScribe(ILogger logger)
        : base(new SetIconBlobContainer(logger))
    {
    }
}
