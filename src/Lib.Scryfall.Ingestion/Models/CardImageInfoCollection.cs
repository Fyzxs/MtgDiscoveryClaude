using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Lib.Scryfall.Shared.Apis.Models;

namespace Lib.Scryfall.Ingestion.Models;

internal sealed class CardImageInfoCollection : ICardImageInfoCollection
{
    private readonly Collection<ICardImageInfo> _imageInfos;

    public CardImageInfoCollection(Collection<ICardImageInfo> imageInfos) => _imageInfos = imageInfos;

    public IEnumerator<ICardImageInfo> GetEnumerator() => _imageInfos.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}