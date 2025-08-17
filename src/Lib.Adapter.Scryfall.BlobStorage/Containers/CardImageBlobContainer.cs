using Lib.BlobStorage.Apis;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Scryfall.BlobStorage.Containers;

internal sealed class CardImageBlobContainer : BlobContainerAdapter
{
    public CardImageBlobContainer(ILogger logger)
        : base(logger, new CardImageContainerDefinition(), new ServiceLocatorAuthBlobConnectionConfig())
    {
    }
}
