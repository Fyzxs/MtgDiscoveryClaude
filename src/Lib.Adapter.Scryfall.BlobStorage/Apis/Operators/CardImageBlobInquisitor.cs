using Lib.Adapter.Scryfall.BlobStorage.Containers;
using Lib.BlobStorage.Apis.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Scryfall.BlobStorage.Apis.Operators;

public sealed class CardImageBlobInquisitor : BlobInquisitor
{
    public CardImageBlobInquisitor(ILogger logger)
        : base(new CardImageBlobContainer(logger))
    {
    }
}