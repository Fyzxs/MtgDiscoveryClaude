using Lib.BlobStorage.Adapters;
using Lib.BlobStorage.Apis;
using Lib.BlobStorage.Apis.Configurations;
using Microsoft.Extensions.Logging;

namespace Example.BlobStorage;

public interface IDemoContainerAdapter : IBlobContainerAdapter { }

internal sealed class DemoContainerAdapter : BlobContainerAdapter, IDemoContainerAdapter
{
    public DemoContainerAdapter(ILogger logger, IBlobContainerDefinition containerDefinition, IBlobConnectionConvenience connectionConfig)
        : base(logger, containerDefinition, connectionConfig)
    {
    }
}
