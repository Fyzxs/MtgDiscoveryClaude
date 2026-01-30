namespace Lib.Universal.Extensions;

public static class StringExtensions
{
    public static bool IzNullOrWhiteSpace(this string value) => string.IsNullOrWhiteSpace(value);
    public static bool IzNotNullOrWhiteSpace(this string value) => string.IsNullOrWhiteSpace(value) is false;
}
