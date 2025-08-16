using System.Collections.Generic;

namespace Lib.BlobStorage.Operations;

/// <summary>
/// Represents an item in a Blob listing, which can be either a directory or a file.
/// </summary>
public interface IBlobListingItem
{
    /// <summary>
    /// Gets the path of the item in the Blob storage.
    /// </summary>
    string Path { get; }
    /// <summary>
    /// Gets the metadata associated with the item.
    /// </summary>
    IDictionary<string, string> Metadata { get; }
}
