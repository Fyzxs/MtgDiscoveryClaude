using System;
using System.Collections.Generic;
using Lib.BlobStorage.Apis.Ids;
using Lib.Scryfall.Shared.Apis.Models;

namespace Lib.Scryfall.Ingestion.BlobStorage.Entities;

internal sealed class SetIconBlobEntity : IBlobBinaryWriteDomain
{
    private readonly IScryfallSet _set;
    private readonly byte[] _iconData;

    public SetIconBlobEntity(IScryfallSet set, byte[] iconData)
    {
        _set = set;
        _iconData = iconData;
    }

    public BlobPathEntity FilePath => new ProvidedBlobPathEntity($"id/{_set.Id()}.svg");

    public BinaryData Content => new(_iconData);

    public IDictionary<string, string> Metadata => new Dictionary<string, string>
    {
        { "set_id", _set.Id() },
        { "set_code", _set.Code() },
        { "content_type", "image/svg+xml" }
    };
}
