using System.Collections.Generic;

namespace Lib.BlobStorage.Operations;

internal sealed class BlobListingItem : IBlobListingItem
{
    public string Path { get; init; }

    public IDictionary<string, string> Metadata { get; init; }
}