using System;
using System.IO;
using System.Linq;
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
public sealed class ScryfallHttpClientTests
{
    [TestMethod]
    [TestCategory("unit")]
    public async Task SendAsync_AddsRequiredHeaders()
    {
        // Arrange
        HttpResponseMessage expectedResponse = new(System.Net.HttpStatusCode.OK);
        HttpClientFake httpClientFake = new() { SendAsyncResult = expectedResponse };
        ScryfallHttpClient subject = new InstanceWrapper(httpClientFake);
        using HttpRequestMessage request = new(HttpMethod.Get, "https://api.scryfall.com/test");

        // Act
        HttpResponseMessage actual = await subject.SendAsync(request).ConfigureAwait(false);

        // Assert
        _ = actual.Should().Be(expectedResponse);
        _ = httpClientFake.SendAsyncInvokeCount.Should().Be(1);
        _ = httpClientFake.SendAsyncLastRequest.Should().Be(request);
        _ = request.Headers.Accept.Should().HaveCount(1);
        _ = request.Headers.Accept.First().ToString().Should().Be("application/json; q=0.9, */*; q=0.8");
        _ = request.Headers.UserAgent.ToString().Should().Be("mtgdiscovery");
    }

    [TestMethod]
    [TestCategory("unit")]
    public async Task ResponseAs_CallsInnerClient()
    {
        // Arrange
        const string expectedResult = "test-result";
        Uri uri = new("https://api.scryfall.com/test");
        HttpClientFake httpClientFake = new() { ResponseAsResult = expectedResult };
        ScryfallHttpClient subject = new InstanceWrapper(httpClientFake);

        // Act
        string actual = await subject.ResponseAs<string>(uri).ConfigureAwait(false);

        // Assert
        _ = actual.Should().Be(expectedResult);
        _ = httpClientFake.ResponseAsInvokeCount.Should().Be(1);
        _ = httpClientFake.ResponseAsLastUri.Should().Be(uri);
    }

    [TestMethod]
    [TestCategory("unit")]
    public async Task StreamAsync_WithUri_CallsInnerClient()
    {
        // Arrange
        Stream expectedStream = new MemoryStream(Encoding.UTF8.GetBytes("test"));
        Uri uri = new("https://api.scryfall.com/test");
        HttpClientFake httpClientFake = new() { StreamAsyncResult = expectedStream };
        ScryfallHttpClient subject = new InstanceWrapper(httpClientFake);

        // Act
        Stream actual = await subject.StreamAsync(uri).ConfigureAwait(false);

        // Assert
        _ = actual.Should().BeSameAs(expectedStream);
        _ = httpClientFake.StreamAsyncInvokeCount.Should().Be(1);
        _ = httpClientFake.StreamAsyncLastUri.Should().Be(uri);
    }

    [TestMethod]
    [TestCategory("unit")]
    public async Task StreamAsync_WithString_CallsInnerClient()
    {
        // Arrange
        Stream expectedStream = new MemoryStream(Encoding.UTF8.GetBytes("test"));
        const string url = "https://api.scryfall.com/test";
        HttpClientFake httpClientFake = new() { StreamAsyncResult = expectedStream };
        ScryfallHttpClient subject = new InstanceWrapper(httpClientFake);

        // Act
        Stream actual = await subject.StreamAsync(new Uri(url)).ConfigureAwait(false);

        // Assert
        _ = actual.Should().BeSameAs(expectedStream);
        _ = httpClientFake.StreamAsyncInvokeCount.Should().Be(1);
        _ = httpClientFake.StreamAsyncLastUri.Should().Be(new Uri(url));
    }

    [TestMethod]
    [TestCategory("unit")]
    public async Task SaveAsFileAsync_WithUri_CallsInnerClient()
    {
        // Arrange
        Uri uri = new("https://api.scryfall.com/test");
        const string fileName = "test.json";
        HttpClientFake httpClientFake = new();
        ScryfallHttpClient subject = new InstanceWrapper(httpClientFake);

        // Act
        await subject.SaveAsFileAsync(uri, fileName).ConfigureAwait(false);

        // Assert
        _ = httpClientFake.SaveAsFileAsyncInvokeCount.Should().Be(1);
        _ = httpClientFake.SaveAsFileAsyncLastUri.Should().Be(uri);
        _ = httpClientFake.SaveAsFileAsyncLastFileName.Should().Be(fileName);
    }

    [TestMethod]
    [TestCategory("unit")]
    public async Task SaveAsFileAsync_WithString_CallsInnerClient()
    {
        // Arrange
        const string url = "https://api.scryfall.com/test";
        const string fileName = "test.json";
        HttpClientFake httpClientFake = new();
        ScryfallHttpClient subject = new InstanceWrapper(httpClientFake);

        // Act
        await subject.SaveAsFileAsync(new Uri(url), fileName).ConfigureAwait(false);

        // Assert
        _ = httpClientFake.SaveAsFileAsyncInvokeCount.Should().Be(1);
        _ = httpClientFake.SaveAsFileAsyncLastUri.Should().Be(new Uri(url));
        _ = httpClientFake.SaveAsFileAsyncLastFileName.Should().Be(fileName);
    }

    private sealed class InstanceWrapper : TypeWrapper<ScryfallHttpClient>
    {
        public InstanceWrapper(IHttpClient innerClient)
            : base(innerClient) { }
    }
}