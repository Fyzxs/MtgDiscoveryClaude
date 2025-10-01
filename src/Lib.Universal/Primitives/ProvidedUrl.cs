using System;

namespace Lib.Universal.Primitives;

public sealed class ProvidedUrl : Url
{
    private readonly Uri _value;

    public ProvidedUrl(string value) : this(new Uri(value))
    {
    }

    private ProvidedUrl(Uri value) => _value = value;

    public override Uri AsSystemType() => _value;
}
