using System;
using System.IO;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Lib.Universal.Http;

public sealed class MonoStateHttpClient : IHttpClient
{
#if DEBUG
    public static void TestSet(HttpClient testInstance) => s_httpClient = testInstance;
#endif
    private static HttpClient s_httpClient;

    private HttpClient MonoState() => s_httpClient ??= new HttpClient();
    public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request) => await MonoState().SendAsync(request).ConfigureAwait(false);
    public async Task<T> ResponseAs<T>(Uri uri, CancellationToken token)
    {
        HttpClient client = MonoState();
        Stream s = await client.GetStreamAsync(uri, token).ConfigureAwait(false);
        await using ConfiguredAsyncDisposable _ = s.ConfigureAwait(false);
        using StreamReader sr = new(s);
#pragma warning disable CA2007 // It's a constructor
        await using JsonReader reader = new JsonTextReader(sr);
#pragma warning restore CA2007
        JsonSerializer serializer = new();
        return serializer.Deserialize<T>(reader);
    }

    public async Task SaveAsFileAsync(string url, string fileName) => await SaveAsFileAsync(new Uri(url), fileName).ConfigureAwait(false);

    public async Task SaveAsFileAsync(Uri uri, string fileName)
    {
        Stream stream = await MonoState().GetStreamAsync(uri).ConfigureAwait(false);
        await using ConfiguredAsyncDisposable _ = stream.ConfigureAwait(false);
#pragma warning disable CA2007 // It's a constructor
        await using FileStream fileStream = new(fileName, FileMode.Create);
#pragma warning restore CA2007
        await stream.CopyToAsync(fileStream).ConfigureAwait(false);
    }

    public async Task<Stream> StreamAsync(string url) => await StreamAsync(new Uri(url)).ConfigureAwait(false);
    public async Task<Stream> StreamAsync(Uri url) => await MonoState().GetStreamAsync(url).ConfigureAwait(false);
}
