using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Lib.Scryfall.Ingestion.Apis.Models;
using Lib.Scryfall.Ingestion.Apis.Values;
using Lib.Scryfall.Ingestion.Internal.Dtos;
using Lib.Scryfall.Ingestion.Internal.Http;
using Lib.Universal.Http;

namespace Lib.Scryfall.Ingestion.Internal.Paging;
internal class HttpScryfallListPaging<T> : IScryfallListPaging<T> where T : IScryfallDto
{
    private readonly IScryfallSearchUri _searchUri;
    private readonly IHttpClient _httpClient;
    private readonly IScryfallDtoFactory<T> _dtoFactory;
    private readonly ILogger _logger;

#pragma warning disable CA2000 // Dispose objects before losing scope - Managed resources will be garbage collected
    protected HttpScryfallListPaging(IScryfallSearchUri searchUri, IScryfallDtoFactory<T> dtoFactory, ILogger logger)
        : this(searchUri, dtoFactory, new RateLimitedHttpClient(), logger)
    {
    }
#pragma warning restore CA2000

    private HttpScryfallListPaging(IScryfallSearchUri searchUri, IScryfallDtoFactory<T> dtoFactory, IHttpClient httpClient, ILogger logger)
    {
        _searchUri = searchUri;
        _dtoFactory = dtoFactory;
        _httpClient = httpClient;
        _logger = logger;
    }

    public IAsyncEnumerable<T> Items()
    {
        Url urlToUse = _searchUri.SearchUri();
        return ItemsInternal(urlToUse);
    }

    private async IAsyncEnumerable<T> ItemsInternal(Url pageUrl)
    {
        ScryfallObjectListDto paging;
        try
        {
            // ReSharper disable twice UseAwaitUsing
            using Stream stream = await _httpClient.StreamAsync(pageUrl).ConfigureAwait(false);
            using StreamReader reader = new(stream);
            using JsonTextReader jsonReader = new(reader);

            JsonSerializer serializer = new();
            dynamic rawData = serializer.Deserialize(jsonReader);

            // Create DTO from raw data
            paging = new ScryfallObjectListDto(rawData);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogPageRetrievalError(pageUrl.AsSystemType().ToString(), ex);
            yield break;
        }

        foreach (dynamic item in paging.Data)
        {
            yield return _dtoFactory.Create(item);
        }

        if (paging.HasNoMore)
        {
            yield break;
        }

        // Get next page from next_page URL if available
        if (string.IsNullOrEmpty(paging.NextPage)) yield break;

        await foreach (T item in ItemsInternal(new Url(paging.NextPage)).ConfigureAwait(false))
        {
            yield return item;
        }
    }
}

internal static partial class HttpScryfallListPagingLoggerExtensions
{
    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Failed to retrieve page from {Url}")]
    public static partial void LogPageRetrievalError(this ILogger logger, string url, Exception ex);
}
