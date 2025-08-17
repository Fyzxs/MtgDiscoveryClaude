using Lib.BlobStorage.Apis.Operations;
using Lib.Scryfall.Ingestion.BlobStorage.Containers;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.BlobStorage.Operators;

internal sealed class SetIconBlobScribe : BlobWriteScribe
{
    public SetIconBlobScribe(ILogger logger)
        : base(new SetIconBlobContainer(logger))
    {
    }
}
