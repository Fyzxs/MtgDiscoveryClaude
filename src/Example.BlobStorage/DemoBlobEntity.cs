using System;
using System.Collections.Generic;
using Lib.BlobStorage.Apis.Ids;

namespace Example.BlobStorage;

public interface IDemoBlobEntity : IBlobBinaryWriteDomain { }

internal sealed class DemoBlobEntity : IDemoBlobEntity
{
    private readonly string _fileName;
    private readonly string _content;

    public DemoBlobEntity(string fileName, string content)
    {
        _fileName = fileName;
        _content = content;
    }

    public BlobPathEntity FilePath => new ProvidedBlobPathEntity(_fileName);

    public BinaryData Content => new(_content);

    public IDictionary<string, string> Metadata => new Dictionary<string, string>
    {
        { "created_by", "Example.BlobStorage" },
        { "timestamp", DateTimeOffset.UtcNow.ToString("O") }
    };
}
