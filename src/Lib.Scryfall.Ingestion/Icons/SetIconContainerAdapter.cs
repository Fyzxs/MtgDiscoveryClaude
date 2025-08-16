using Lib.BlobStorage.Adapters;
using Lib.BlobStorage.Apis;
using Lib.BlobStorage.Apis.Configurations;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.Icons;

internal interface ISetIconContainerAdapter : IBlobContainerAdapter { }

internal sealed class SetIconContainerAdapter : BlobContainerAdapter, ISetIconContainerAdapter
{
    public SetIconContainerAdapter(ILogger logger)
        : this(logger, new SetIconContainerDefinition(), new ServiceLocatorAuthBlobConnectionConfig())
    {
    }

    private SetIconContainerAdapter(ILogger logger, IBlobContainerDefinition containerDefinition, IBlobConnectionConvenience connectionConfig)
        : base(logger, containerDefinition, connectionConfig)
    {
    }
}