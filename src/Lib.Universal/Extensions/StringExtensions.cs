namespace Lib.Universal.Extensions;

public static class StringExtensions
{
    public static bool IzNullOrWhiteSpace(this string value) => string.IsNullOrWhiteSpace(value);
}
