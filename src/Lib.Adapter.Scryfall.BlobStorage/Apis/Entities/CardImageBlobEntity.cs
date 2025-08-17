using System;
using System.Collections.Generic;
using Lib.BlobStorage.Apis.Ids;
using Lib.Scryfall.Shared.Apis.Models;

namespace Lib.Adapter.Scryfall.BlobStorage.Apis.Entities;

public sealed class CardImageBlobEntity : IBlobBinaryWriteDomain
{
    private readonly ICardImageInfo _imageInfo;
    private readonly byte[] _bytes;

    public CardImageBlobEntity(ICardImageInfo imageInfo, byte[] bytes)
    {
        _imageInfo = imageInfo;
        _bytes = bytes;
    }

    public BlobPathEntity FilePath => new ProvidedBlobPathEntity(_imageInfo.StoragePath());

    public BinaryData Content => new(_bytes);

    public IDictionary<string, string> Metadata => _imageInfo.Metadata();
}
