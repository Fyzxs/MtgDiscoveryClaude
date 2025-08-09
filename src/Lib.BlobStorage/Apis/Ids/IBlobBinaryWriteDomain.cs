using System;
using System.Collections.Generic;

namespace Lib.BlobStorage.Apis.Ids;

/// <summary>
/// 
/// </summary>
public interface IBlobBinaryWriteDomain
{
    /// <summary>
    /// 
    /// </summary>
    BlobPathEntity FilePath { get; }

    /// <summary>
    /// 
    /// </summary>
    BinaryData Content { get; }

    /// <summary>
    /// Optional metadata to write with the blob.
    /// </summary>
    IDictionary<string, string> Metadata { get; }
}