using System;
using System.Collections.Generic;

namespace Lib.BlobStorage.Apis.Ids;

/// <summary>
/// Represents a domain entity for writing binary data to a blob storage.
/// </summary>
public interface IBlobBinaryWriteDomain
{
    /// <summary>
    /// Gets the file path associated with the blob.
    /// </summary>
    /// <remarks>
    /// The file path is represented as a <see cref="BlobPathEntity"/> and is used to uniquely identify
    /// the location of the blob within the storage system.
    /// </remarks>
    BlobPathEntity FilePath { get; }

    /// <summary>
    /// Gets the binary content to be written to the blob storage.
    /// </summary>
    /// <value>
    /// A <see cref="BinaryData"/> instance representing the binary content of the blob.
    /// </value>
    BinaryData Content { get; }

    /// <summary>
    /// Optional metadata to write with the blob.
    /// </summary>
    IDictionary<string, string> Metadata { get; }
}
