using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Lib.Universal.Http;

namespace Lib.Scryfall.Ingestion.Tests.Fakes;

internal sealed class HttpClientFake : IHttpClient
{
    public HttpResponseMessage SendAsyncResult { get; init; }
    public int SendAsyncInvokeCount { get; private set; }
    public HttpRequestMessage SendAsyncLastRequest { get; private set; }

    public object ResponseAsResult { get; init; }
    public int ResponseAsInvokeCount { get; private set; }
    public Uri ResponseAsLastUri { get; private set; }

    public Stream StreamAsyncResult { get; init; }
    public int StreamAsyncInvokeCount { get; private set; }
    public Uri StreamAsyncLastUri { get; private set; }

    public int SaveAsFileAsyncInvokeCount { get; private set; }
    public Uri SaveAsFileAsyncLastUri { get; private set; }
    public string SaveAsFileAsyncLastFileName { get; private set; }

    public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
    {
        SendAsyncInvokeCount++;
        SendAsyncLastRequest = request;
        return Task.FromResult(SendAsyncResult);
    }

    public Task<T> ResponseAs<T>(Uri uri, CancellationToken token = default)
    {
        ResponseAsInvokeCount++;
        ResponseAsLastUri = uri;
        return Task.FromResult((T)ResponseAsResult);
    }

    public Task SaveAsFileAsync(string url, string fileName)
    {
        SaveAsFileAsyncInvokeCount++;
        SaveAsFileAsyncLastUri = new Uri(url);
        SaveAsFileAsyncLastFileName = fileName;
        return Task.CompletedTask;
    }

    public Task SaveAsFileAsync(Uri uri, string fileName)
    {
        SaveAsFileAsyncInvokeCount++;
        SaveAsFileAsyncLastUri = uri;
        SaveAsFileAsyncLastFileName = fileName;
        return Task.CompletedTask;
    }

    public Task<Stream> StreamAsync(string url)
    {
        StreamAsyncInvokeCount++;
        StreamAsyncLastUri = new Uri(url);
        return Task.FromResult(StreamAsyncResult);
    }

    public Task<Stream> StreamAsync(Uri url)
    {
        StreamAsyncInvokeCount++;
        StreamAsyncLastUri = url;
        return Task.FromResult(StreamAsyncResult);
    }
}
