using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Lib.Universal.Http;

namespace Lib.Scryfall.Ingestion.Http;

internal sealed class ScryfallHttpClient : IHttpClient
{
    private readonly IHttpClient _innerClient;

    public ScryfallHttpClient() : this(new MonoStateHttpClient())
    {
    }

    private ScryfallHttpClient(IHttpClient innerClient) => _innerClient = innerClient;

    public async Task<HttpResponseMessage> SendAsync([NotNull] HttpRequestMessage request)
    {
#pragma warning disable CA1062 // Validate that request is not null
        AddScryfallHeaders(request);
#pragma warning restore CA1062
        return await _innerClient.SendAsync(request).ConfigureAwait(false);
    }

    public async Task<T> ResponseAs<T>(Uri uri, CancellationToken token = default)
    {
        using HttpRequestMessage request = new(HttpMethod.Get, uri);
        AddScryfallHeaders(request);
        using HttpResponseMessage response = await SendAsync(request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        string content = await response.Content.ReadAsStringAsync(token).ConfigureAwait(false);
        return JsonConvert.DeserializeObject<T>(content);
    }

    public async Task SaveAsFileAsync(string url, string fileName) => await SaveAsFileAsync(new Uri(url), fileName).ConfigureAwait(false);

    public async Task SaveAsFileAsync(Uri uri, string fileName)
    {
        using HttpRequestMessage request = new(HttpMethod.Get, uri);
        AddScryfallHeaders(request);
        using HttpResponseMessage response = await SendAsync(request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        // ReSharper disable twice UseAwaitUsing
        using Stream stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
        using FileStream fileStream = new(fileName, FileMode.Create);
        await stream.CopyToAsync(fileStream).ConfigureAwait(false);
    }

    public async Task<Stream> StreamAsync(string url) => await StreamAsync(new Uri(url)).ConfigureAwait(false);

    public async Task<Stream> StreamAsync(Uri url)
    {
        using HttpRequestMessage request = new(HttpMethod.Get, url);
        AddScryfallHeaders(request);
        HttpResponseMessage response = await SendAsync(request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
    }

    private static void AddScryfallHeaders([NotNull] HttpRequestMessage request)
    {
        request.Headers.Add("Accept", "application/json;q=0.9,*/*;q=0.8");
        request.Headers.Add("User-Agent", "mtgdiscovery");
    }
}
