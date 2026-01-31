using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TestConvenience.Core.Extensions;

internal static class ObjectJsonExtensions
{
    public static JObject ToJObject(this object obj) => JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(obj));
}

internal static class StringExtensions
{
    public static Stream ToStream(this string str)
    {
        MemoryStream stream = new();
        StreamWriter writer = new(stream, leaveOpen: true);
        writer.Write(str);
        writer.Flush();
        writer.Dispose();
        stream.Position = 0;
        return stream;
    }
}

internal static class StreamExtensions
{
    public static string AsString(this Stream stream)
    {
        stream.Position = 0;
        using StreamReader reader = new(stream, leaveOpen: true);
        return reader.ReadToEnd();
    }
}
