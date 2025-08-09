using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Lib.Universal.Http;

public interface IHttpClient
{
    Task<HttpResponseMessage> SendAsync(HttpRequestMessage request);
    Task<T> ResponseAs<T>(Uri uri, CancellationToken token = default);
    Task SaveAsFileAsync(string url, string fileName);
    Task SaveAsFileAsync(Uri uri, string fileName);
    Task<Stream> StreamAsync(string url);
    Task<Stream> StreamAsync(Uri url);
}
