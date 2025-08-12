using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Lib.Scryfall.Ingestion.Apis.Dtos;
using Lib.Scryfall.Ingestion.Apis.Http;
using Lib.Scryfall.Ingestion.Apis.Models;
using Lib.Scryfall.Ingestion.Apis.Values;
using Lib.Universal.Http;
using Newtonsoft.Json;

namespace Lib.Scryfall.Ingestion.Apis.Paging;

/// <summary>
/// Base implementation for handling Scryfall API pagination with composition.
/// </summary>
public class HttpScryfallListPaging<T> : IScryfallListPaging<T> where T : IScryfallDto
{
    private readonly IScryfallSearchUri _searchUriProvider;
    private readonly Url _url;
    private readonly IHttpClient _httpClient;
    private readonly IScryfallDtoFactory<T> _dtoFactory;

    protected HttpScryfallListPaging(IScryfallSearchUri searchUriProvider, IScryfallDtoFactory<T> dtoFactory)
        : this(searchUriProvider, dtoFactory, new RateLimitedHttpClient())
    {
    }

    private HttpScryfallListPaging(IScryfallSearchUri searchUriProvider, IScryfallDtoFactory<T> dtoFactory, IHttpClient httpClient)
    {
        _searchUriProvider = searchUriProvider;
        _url = null;
        _dtoFactory = dtoFactory;
        _httpClient = httpClient;
    }

#pragma warning disable CA2000 // Dispose objects before losing scope - Managed resources will be garbage collected
    protected HttpScryfallListPaging(Url url, IScryfallDtoFactory<T> dtoFactory)
        : this(url, dtoFactory, new RateLimitedHttpClient())
    {
    }
#pragma warning restore CA2000

    private HttpScryfallListPaging(Url url, IScryfallDtoFactory<T> dtoFactory, IHttpClient httpClient)
    {
        _searchUriProvider = null;
        _url = url;
        _dtoFactory = dtoFactory;
        _httpClient = httpClient;
    }

    public IAsyncEnumerable<T> Items()
    {
        Url urlToUse = _url ?? _searchUriProvider?.SearchUri();
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
        catch (System.Net.Http.HttpRequestException ex)
        {
            Console.WriteLine($"Exception retrieving [{pageUrl}]: {ex.Message}");
            yield break;
        }

        if (paging.HasMore is false && paging.Data is null)
        {
            yield break;
        }

        foreach (dynamic item in paging.Data)
        {
            yield return _dtoFactory.Create(item);
        }

        if (paging.HasMore is false)
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
