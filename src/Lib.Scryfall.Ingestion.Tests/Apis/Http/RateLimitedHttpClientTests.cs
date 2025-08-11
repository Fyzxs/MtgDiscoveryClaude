using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Lib.Scryfall.Ingestion.Apis.Http;
using Lib.Scryfall.Ingestion.Tests.Fakes;
using Lib.Universal.Http;
using TestConvenience.Core.Reflection;

namespace Lib.Scryfall.Ingestion.Tests.Apis.Http;

[TestClass]
public sealed class RateLimitedHttpClientTests
{
    [TestMethod]
    [TestCategory("unit")]
    public async Task SendAsync_AcquiresTokenAndCallsInnerClient()
    {
        // Arrange
        HttpResponseMessage expectedResponse = new(System.Net.HttpStatusCode.OK);
        HttpClientFake httpClientFake = new() { SendAsyncResult = expectedResponse };
        RateLimitTokenFake tokenFake = new();
        ScryfallRateLimiterFake rateLimiterFake = new() { AcquireTokenAsyncResult = tokenFake };
        RateLimitedHttpClient subject = new InstanceWrapper(httpClientFake, rateLimiterFake);
        using HttpRequestMessage request = new(HttpMethod.Get, "https://api.scryfall.com/test");

        // Act
        HttpResponseMessage actual = await subject.SendAsync(request).ConfigureAwait(false);

        // Assert
        _ = actual.Should().Be(expectedResponse);
        _ = rateLimiterFake.AcquireTokenAsyncInvokeCount.Should().Be(1);
        _ = httpClientFake.SendAsyncInvokeCount.Should().Be(1);
        _ = httpClientFake.SendAsyncLastRequest.Should().Be(request);
        _ = tokenFake.DisposeInvokeCount.Should().Be(1);
    }

    [TestMethod]
    [TestCategory("unit")]
    public async Task ResponseAs_AcquiresTokenAndCallsInnerClient()
    {
        // Arrange
        const string expectedResult = "test-result";
        Uri uri = new("https://api.scryfall.com/test");
        HttpClientFake httpClientFake = new() { ResponseAsResult = expectedResult };
        RateLimitTokenFake tokenFake = new();
        ScryfallRateLimiterFake rateLimiterFake = new() { AcquireTokenAsyncResult = tokenFake };
        RateLimitedHttpClient subject = new InstanceWrapper(httpClientFake, rateLimiterFake);

        // Act
        string actual = await subject.ResponseAs<string>(uri).ConfigureAwait(false);

        // Assert
        _ = actual.Should().Be(expectedResult);
        _ = rateLimiterFake.AcquireTokenAsyncInvokeCount.Should().Be(1);
        _ = httpClientFake.ResponseAsInvokeCount.Should().Be(1);
        _ = httpClientFake.ResponseAsLastUri.Should().Be(uri);
        _ = tokenFake.DisposeInvokeCount.Should().Be(1);
    }

    [TestMethod]
    [TestCategory("unit")]
    public async Task StreamAsync_WithUri_AcquiresTokenAndCallsInnerClient()
    {
        // Arrange
        Stream expectedStream = new MemoryStream(Encoding.UTF8.GetBytes("test"));
        Uri uri = new("https://api.scryfall.com/test");
        HttpClientFake httpClientFake = new() { StreamAsyncResult = expectedStream };
        RateLimitTokenFake tokenFake = new();
        ScryfallRateLimiterFake rateLimiterFake = new() { AcquireTokenAsyncResult = tokenFake };
        RateLimitedHttpClient subject = new InstanceWrapper(httpClientFake, rateLimiterFake);

        // Act
        Stream actual = await subject.StreamAsync(uri).ConfigureAwait(false);

        // Assert
        _ = actual.Should().BeSameAs(expectedStream);
        _ = rateLimiterFake.AcquireTokenAsyncInvokeCount.Should().Be(1);
        _ = httpClientFake.StreamAsyncInvokeCount.Should().Be(1);
        _ = httpClientFake.StreamAsyncLastUri.Should().Be(uri);
        _ = tokenFake.DisposeInvokeCount.Should().Be(1);
    }

    [TestMethod]
    [TestCategory("unit")]
    public async Task StreamAsync_WithString_AcquiresTokenAndCallsInnerClient()
    {
        // Arrange
        Stream expectedStream = new MemoryStream(Encoding.UTF8.GetBytes("test"));
        const string url = "https://api.scryfall.com/test";
        HttpClientFake httpClientFake = new() { StreamAsyncResult = expectedStream };
        RateLimitTokenFake tokenFake = new();
        ScryfallRateLimiterFake rateLimiterFake = new() { AcquireTokenAsyncResult = tokenFake };
        RateLimitedHttpClient subject = new InstanceWrapper(httpClientFake, rateLimiterFake);

        // Act
        Stream actual = await subject.StreamAsync(new Uri(url)).ConfigureAwait(false);

        // Assert
        _ = actual.Should().BeSameAs(expectedStream);
        _ = rateLimiterFake.AcquireTokenAsyncInvokeCount.Should().Be(1);
        _ = httpClientFake.StreamAsyncInvokeCount.Should().Be(1);
        _ = httpClientFake.StreamAsyncLastUri.Should().Be(new Uri(url));
        _ = tokenFake.DisposeInvokeCount.Should().Be(1);
    }

    [TestMethod]
    [TestCategory("unit")]
    public async Task SaveAsFileAsync_WithUri_AcquiresTokenAndCallsInnerClient()
    {
        // Arrange
        Uri uri = new("https://api.scryfall.com/test");
        const string fileName = "test.json";
        HttpClientFake httpClientFake = new();
        RateLimitTokenFake tokenFake = new();
        ScryfallRateLimiterFake rateLimiterFake = new() { AcquireTokenAsyncResult = tokenFake };
        RateLimitedHttpClient subject = new InstanceWrapper(httpClientFake, rateLimiterFake);

        // Act
        await subject.SaveAsFileAsync(uri, fileName).ConfigureAwait(false);

        // Assert
        _ = rateLimiterFake.AcquireTokenAsyncInvokeCount.Should().Be(1);
        _ = httpClientFake.SaveAsFileAsyncInvokeCount.Should().Be(1);
        _ = httpClientFake.SaveAsFileAsyncLastUri.Should().Be(uri);
        _ = httpClientFake.SaveAsFileAsyncLastFileName.Should().Be(fileName);
        _ = tokenFake.DisposeInvokeCount.Should().Be(1);
    }

    [TestMethod]
    [TestCategory("unit")]
    public async Task SaveAsFileAsync_WithString_AcquiresTokenAndCallsInnerClient()
    {
        // Arrange
        const string url = "https://api.scryfall.com/test";
        const string fileName = "test.json";
        HttpClientFake httpClientFake = new();
        RateLimitTokenFake tokenFake = new();
        ScryfallRateLimiterFake rateLimiterFake = new() { AcquireTokenAsyncResult = tokenFake };
        RateLimitedHttpClient subject = new InstanceWrapper(httpClientFake, rateLimiterFake);

        // Act
        await subject.SaveAsFileAsync(new Uri(url), fileName).ConfigureAwait(false);

        // Assert
        _ = rateLimiterFake.AcquireTokenAsyncInvokeCount.Should().Be(1);
        _ = httpClientFake.SaveAsFileAsyncInvokeCount.Should().Be(1);
        _ = httpClientFake.SaveAsFileAsyncLastUri.Should().Be(new Uri(url));
        _ = httpClientFake.SaveAsFileAsyncLastFileName.Should().Be(fileName);
        _ = tokenFake.DisposeInvokeCount.Should().Be(1);
    }

    private sealed class InstanceWrapper : TypeWrapper<RateLimitedHttpClient>
    {
        public InstanceWrapper(IHttpClient innerClient, IScryfallRateLimiter rateLimiter)
            : base(innerClient, rateLimiter) { }
    }
}
