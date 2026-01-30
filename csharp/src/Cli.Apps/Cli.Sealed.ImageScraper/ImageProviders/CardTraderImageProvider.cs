using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Cli.Sealed.ImageScraper.Models;

namespace Cli.Sealed.ImageScraper.ImageProviders;

internal sealed partial class CardTraderImageProvider : IImageProvider
{
    private const string ManasearchBaseUrl = "https://www.cardtrader.com/en/manasearch_results?ids=";

    private readonly HttpClient _httpClient;

    public CardTraderImageProvider(HttpClient httpClient) => _httpClient = httpClient;

    public string Name => "CardTrader";

    public bool CanProvide(SealedProduct product) => product.HasCardTraderId;

    public async Task<IReadOnlyList<string>> GetImageUrlsAsync(
        SealedProduct product,
        ImageSize size,
        CancellationToken cancellationToken)
    {
        if (product.HasCardTraderId is false)
        {
            return [];
        }

        try
        {
            string imageUrl = await FetchImageUrlAsync(product.CardTraderId, cancellationToken)
                .ConfigureAwait(false);

            if (string.IsNullOrEmpty(imageUrl))
            {
                return [];
            }

            return [imageUrl];
        }
        catch (HttpRequestException)
        {
            return [];
        }
    }

    private async Task<string> FetchImageUrlAsync(string cardTraderId, CancellationToken cancellationToken)
    {
        string encodedId = Convert.ToBase64String(Encoding.UTF8.GetBytes(cardTraderId));
        string searchUrl = $"{ManasearchBaseUrl}{encodedId}";

        using HttpResponseMessage response = await _httpClient
            .GetAsync(new Uri(searchUrl), cancellationToken)
            .ConfigureAwait(false);

        if (response.IsSuccessStatusCode is false)
        {
            return null;
        }

        string html = await response.Content
            .ReadAsStringAsync(cancellationToken)
            .ConfigureAwait(false);

        return ExtractImageUrl(html);
    }

    private static string ExtractImageUrl(string html)
    {
        Match match = MetaImageRegex().Match(html);
        return match.Success ? match.Groups[1].Value : null;
    }

    [GeneratedRegex("<meta\\s+name=\"image\"\\s+content=\"([^\"]+)\"", RegexOptions.IgnoreCase)]
    private static partial Regex MetaImageRegex();
}
