using Lib.BlobStorage.Apis.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.Icons;

internal sealed class SetIconBlobScribe : BlobWriteScribe
{
    public SetIconBlobScribe(ILogger logger)
        : base(new SetIconContainerAdapter(logger))
    {
    }
}
