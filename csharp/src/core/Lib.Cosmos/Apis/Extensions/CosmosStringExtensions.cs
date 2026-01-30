using System;
using System.Text.RegularExpressions;

namespace Lib.Cosmos.Apis.Extensions;

/// <summary>
/// Provides extension methods for string manipulation specific to Cosmos compliance.
/// </summary>
public static partial class CosmosStringExtensions
{
    private const string InvalidCharactersPattern = @"[^a-zA-Z0-9\-_\.]";

    [GeneratedRegex(InvalidCharactersPattern)]
    private static partial Regex InvalidCharactersRegex();

    /// <summary>
    /// Converts the provided string into a Cosmos-compliant string.
    /// This includes trimming whitespace, replacing slashes with hyphens,
    /// and replacing invalid characters with underscores.
    /// </summary>
    /// <param name="toConvert">The string value to convert.</param>
    /// <returns>A sanitized string that complies with Cosmos requirements.</returns>
    public static string ToCosmosCompliantString(this string toConvert)
    {
        /*
         * Apologies(qgil, 20250707) We don't normally check these things.
         * Cosmos needs the extra protection; else it gets cranky.
         */
        ArgumentException.ThrowIfNullOrWhiteSpace(toConvert);

        string mutation = toConvert.Trim();
        mutation = mutation.Replace("/", "-").Replace("\\", "-");
        mutation = InvalidCharactersRegex().Replace(mutation, "_");

        return mutation;
    }
}
