using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cli.Sealed.ImageScraper.Models;

namespace Cli.Sealed.ImageScraper.ImageProviders;

internal sealed class CardMarketImageProvider : IImageProvider
{
    private const string BaseUrl = "https://product-images.s3.cardmarket.com";

    private static readonly string[] s_extensions = ["jpg", "png"];
    private static readonly int[] s_prefixesWhenDiff7 = [7, 32, 24, 2];
    private static readonly int[] s_prefixesOtherwise = [32, 24, 7, 2];

    public string Name => "CardMarket";

    public bool CanProvide(SealedProduct product) => product.HasMcmId;

    public Task<IReadOnlyList<string>> GetImageUrlsAsync(SealedProduct product, ImageSize size, CancellationToken cancellationToken)
    {
        if (product.HasMcmId is false)
        {
            return Task.FromResult<IReadOnlyList<string>>(Array.Empty<string>());
        }

        string mcmId = product.McmId;
        List<string> urls = [];

        int[] prefixes = CalculateDiff(mcmId) == 7 ? s_prefixesWhenDiff7 : s_prefixesOtherwise;

        foreach (int prefix in prefixes)
        {
            AddUrlsForPrefix(urls, prefix, mcmId);
        }

        return Task.FromResult<IReadOnlyList<string>>(urls);
    }

    private static void AddUrlsForPrefix(List<string> urls, int prefix, string mcmId)
    {
        foreach (string ext in s_extensions)
        {
            urls.Add($"{BaseUrl}/{prefix}/{mcmId}/{mcmId}.{ext}");
        }
    }

    private static int CalculateDiff(string mcmId)
    {
        int oddSum = 0;
        int evenSum = 0;

        for (int i = 0; i < mcmId.Length; i++)
        {
            if (char.IsDigit(mcmId[i]) is false)
            {
                continue;
            }

            int digit = mcmId[i] - '0';
            int position = i + 1;

            if (position % 2 == 1)
            {
                oddSum += digit;
            }
            else
            {
                evenSum += digit;
            }
        }

        return oddSum - evenSum;
    }
}
