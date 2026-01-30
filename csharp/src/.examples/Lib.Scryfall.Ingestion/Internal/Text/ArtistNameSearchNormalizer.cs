using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Lib.Scryfall.Ingestion.Internal.Text;

internal sealed class ArtistNameSearchNormalizer : IArtistNameSearchNormalizer
{
    public string Normalize(IEnumerable<string> artistNames)
    {
        HashSet<string> searchTerms = [];

        foreach (string name in artistNames.Where(name => string.IsNullOrWhiteSpace(name) is false))
        {
            searchTerms.Add(name.ToLowerInvariant());

            string asciiNormalized = RemoveDiacritics(name).ToLowerInvariant();
            searchTerms.Add(asciiNormalized);
        }

        return string.Join(" ", searchTerms);
    }

    private static string RemoveDiacritics(string text)
    {
        string normalizedString = text.Normalize(NormalizationForm.FormD);
        StringBuilder stringBuilder = new(normalizedString.Length);

        foreach (char c in normalizedString)
        {
            UnicodeCategory unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory is UnicodeCategory.NonSpacingMark) continue;

            stringBuilder.Append(c);
        }

        return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
    }
}
