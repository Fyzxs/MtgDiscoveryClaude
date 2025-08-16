using System;
using Lib.Universal.Primitives;

namespace Lib.Scryfall.Ingestion.Apis.Values;
public sealed class Url : ToSystemType<Uri>
{
    private readonly Uri _value;

    public Url(string value) : this(new Uri(value))
    {
    }

    private Url(Uri value)
    {
        _value = value;
    }

    public override Uri AsSystemType() => _value;
}
