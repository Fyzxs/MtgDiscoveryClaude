using System.IO;

namespace TestConvenience.Core.Extensions;

public static class ObjectJsonExtensions
{
    public static JObject ToJObject(this object obj) => JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(obj));
}

public static class StringExtensions
{
    public static Stream ToStream(this string str)
    {
        MemoryStream stream = new();
#pragma warning disable CA2000
        StreamWriter writer = new(stream);
#pragma warning restore CA2000
        writer.Write(str);
        writer.Flush();
        stream.Position = 0;
        return stream;
    }
}

public static class StreamExtensions
{
    public static string AsString(this Stream stream)
    {
        stream.Position = 0;
#pragma warning disable CA2000
        return new StreamReader(stream).ReadToEnd();
#pragma warning restore CA2000
    }
}
