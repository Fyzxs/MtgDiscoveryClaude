using Lib.BlobStorage.Apis;
using Lib.BlobStorage.Apis.Configurations;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.BlobStorage.Containers;

internal sealed class SetIconBlobContainer : BlobContainerAdapter
{
    public SetIconBlobContainer(ILogger logger)
        : this(logger, new SetIconContainerDefinition(), new ServiceLocatorAuthBlobConnectionConfig())
    {
    }

    private SetIconBlobContainer(ILogger logger, IBlobContainerDefinition containerDefinition, IBlobConnectionConvenience connectionConfig)
        : base(logger, containerDefinition, connectionConfig)
    {
    }
}
