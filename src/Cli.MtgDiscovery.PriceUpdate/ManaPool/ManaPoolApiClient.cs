#nullable enable
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Cli.MtgDiscovery.PriceUpdate.Configuration;
using Cli.MtgDiscovery.PriceUpdate.ManaPool.Entities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Cli.MtgDiscovery.PriceUpdate.ManaPool;

internal sealed class ManaPoolApiClient : IManaPoolApiClient
{
    private readonly ILogger<ManaPoolApiClient> _logger;
    private readonly HttpClient _httpClient;
    private readonly PriceUpdateConfiguration _config;

    public ManaPoolApiClient(
        ILogger<ManaPoolApiClient> logger,
        HttpClient httpClient,
        PriceUpdateConfiguration config)
    {
        _logger = logger;
        _httpClient = httpClient;
        _config = config;
    }

    public async Task<IReadOnlyDictionary<string, ManaPoolPriceItem>> FetchAllPricesAsync()
    {
        Dictionary<string, ManaPoolPriceItem> prices = [];
        int currentPage = 1;
        int totalPages = 1;

        _logger.LogInformation("Starting to fetch prices from ManaPool API: {Url}", _config.ManaPoolApiUrl);

        do
        {
            ManaPoolApiResponse response = await FetchPageWithRetryAsync(currentPage).ConfigureAwait(false);

            totalPages = response.Meta.TotalPages;

            foreach (ManaPoolPriceItem item in response.Data)
            {
                if (string.IsNullOrEmpty(item.ScryfallId) is false)
                {
                    prices[item.ScryfallId] = item;
                }
            }

            _logger.LogInformation(
                "Fetched page {CurrentPage}/{TotalPages} - {ItemCount} items (Total accumulated: {TotalCount})",
                currentPage,
                totalPages,
                response.Data.Count,
                prices.Count);

            currentPage++;
        }
        while (currentPage <= totalPages);

        _logger.LogInformation("Completed fetching all prices. Total unique cards: {Count}", prices.Count);

        return prices;
    }

    private async Task<ManaPoolApiResponse> FetchPageWithRetryAsync(int page)
    {
        Uri uri = new($"{_config.ManaPoolApiUrl}?page={page}");

        for (int attempt = 1; attempt <= _config.RetryCount; attempt++)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(uri).ConfigureAwait(false);
                _ = response.EnsureSuccessStatusCode();

                string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                ManaPoolApiResponse result = JsonConvert.DeserializeObject<ManaPoolApiResponse>(json)
                    ?? throw new InvalidOperationException($"Failed to deserialize ManaPool API response for page {page}");

                return result;
            }
            catch (Exception ex) when (attempt < _config.RetryCount)
            {
                _logger.LogWarning(
                    ex,
                    "Attempt {Attempt}/{MaxAttempts} failed for page {Page}. Retrying in {DelayMs}ms...",
                    attempt,
                    _config.RetryCount,
                    page,
                    _config.RetryDelayMs);

                await Task.Delay(_config.RetryDelayMs).ConfigureAwait(false);
            }
        }

        throw new InvalidOperationException($"Failed to fetch page {page} after {_config.RetryCount} attempts");
    }
}
