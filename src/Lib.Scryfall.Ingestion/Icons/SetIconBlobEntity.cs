using System;
using System.Collections.Generic;
using Lib.BlobStorage.Apis.Ids;

namespace Lib.Scryfall.Ingestion.Icons;

internal interface ISetIconBlobEntity : IBlobBinaryWriteDomain
{
    string SetId { get; }
    string SetCode { get; }
}

internal sealed class SetIconBlobEntity : ISetIconBlobEntity
{
    private readonly string _setId;
    private readonly string _setCode;
    private readonly byte[] _iconData;

    public SetIconBlobEntity(string setId, string setCode, byte[] iconData)
    {
        _setId = setId;
        _setCode = setCode;
        _iconData = iconData;
    }

    public string SetId => _setId;
    public string SetCode => _setCode;

    public BlobPathEntity FilePath => new ProvidedBlobPathEntity($"id/{_setId}.svg");

    public BinaryData Content => new(_iconData);

    public IDictionary<string, string> Metadata => new Dictionary<string, string>
    {
        { "set_id", _setId },
        { "set_code", _setCode },
        { "content_type", "image/svg+xml" }
    };
}
