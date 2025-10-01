using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace Lib.Scryfall.Ingestion.Services;

internal interface ICardGroupingMatcher
{
    string GetGroupIdForCard(dynamic cardData, string setCode);
}

internal sealed partial class CardGroupingMatcher : ICardGroupingMatcher
{

    [GeneratedRegex(@"^(\d+)")]
    private static partial Regex MyRegex();

    private readonly ISetGroupingsLoader _groupingsLoader;

    public CardGroupingMatcher() : this(new MonoStateSetGroupingsLoader())
    {
    }

    private CardGroupingMatcher(ISetGroupingsLoader groupingsLoader) => _groupingsLoader = groupingsLoader;

    public string GetGroupIdForCard(dynamic cardData, string setCode)
    {
        SetGroupingData groupingData = _groupingsLoader.GetGroupingsForSet(setCode);
        if (groupingData is null || groupingData.Groupings is null)
        {
            return null;
        }

        // Sort groupings by order (process in order)
        List<CardGrouping> sortedGroupings = [.. groupingData.Groupings.OrderBy(g => g.Order)];

        // Find first matching grouping
        foreach (CardGrouping grouping in sortedGroupings)
        {
            if (CardMatchesGrouping(cardData, grouping))
            {
                return grouping.Id;
            }
        }

        return null;
    }

    private static bool CardMatchesGrouping(dynamic cardData, CardGrouping grouping)
    {
        if (grouping.ParsedFilters is null)
        {
            return false;
        }

        // Check collector number range if specified
        if (grouping.ParsedFilters.CollectorNumberRange is not null)
        {
            if (MatchesCollectorNumberRange(cardData, grouping.ParsedFilters.CollectorNumberRange) is false)
            {
                return false;
            }
        }

        // Check properties if specified
        if (grouping.ParsedFilters.Properties is not null && grouping.ParsedFilters.Properties.Count > 0)
        {
            if (MatchesProperties(cardData, grouping.ParsedFilters.Properties) is false)
            {
                return false;
            }
        }

        return true;
    }

    private static bool MatchesCollectorNumberRange(dynamic cardData, CollectorNumberRange range)
    {
        string collectorNumber = GetStringProperty(cardData, "collector_number");
        if (string.IsNullOrEmpty(collectorNumber))
        {
            return false;
        }

        bool matchesRange = false;

        // Check min-max range
        if (string.IsNullOrEmpty(range.Min) is false && string.IsNullOrEmpty(range.Max) is false)
        {
            int cardNum = GetNumericValue(collectorNumber);
            int minNum = GetNumericValue(range.Min);
            int maxNum = GetNumericValue(range.Max);

            matchesRange = cardNum >= minNum && cardNum <= maxNum;
        }

        // Check OR conditions (specific collector numbers)
        bool matchesOrCondition = false;
        if (range.OrConditions is not null && range.OrConditions.Count > 0)
        {
            string normalizedCardNumber = collectorNumber.ToLowerInvariant().Trim();
            matchesOrCondition = range.OrConditions.Any(cn =>
            {
                string normalizedCondition = cn.ToLowerInvariant().Trim();
                return normalizedCardNumber == normalizedCondition;
            });
        }

        return matchesRange || matchesOrCondition;
    }

    private static int GetNumericValue(string value)
    {
        // Extract leading digits from the string
        Match match = MyRegex().Match(value);
        return match.Success ? int.Parse(match.Groups[1].Value) : 0;
    }

    private static bool MatchesProperties(dynamic cardData, Dictionary<string, object> properties)
    {
        foreach (KeyValuePair<string, object> property in properties)
        {
            // Handle exclusion properties
            if (property.Key.EndsWith("_excludes"))
            {
                string propName = property.Key.Replace("_excludes", "");
                string cardValue = GetStringProperty(cardData, propName);
                if (string.IsNullOrEmpty(cardValue) is false)
                {
                    string valueStr = property.Value?.ToString() ?? string.Empty;
                    if (cardValue.Contains(valueStr, StringComparison.OrdinalIgnoreCase))
                    {
                        return false;
                    }
                }
            }
            // Handle contains properties
            else if (property.Key.EndsWith("_contains"))
            {
                string propName = property.Key.Replace("_contains", "");
                string cardValue = GetStringProperty(cardData, propName);
                if (string.IsNullOrEmpty(cardValue))
                {
                    return false;
                }

                string valueStr = property.Value?.ToString() ?? string.Empty;
                if (cardValue.Contains(valueStr, StringComparison.OrdinalIgnoreCase) is false)
                {
                    return false;
                }
            }
            // Direct property match
            else
            {
                if (CheckPropertyMatch(cardData, property.Key, property.Value) is false)
                {
                    return false;
                }
            }
        }

        return true;
    }

    private static bool CheckPropertyMatch(dynamic cardData, string propertyKey, object expectedValue)
    {
        // Try to get the property value from card data
        object cardValue = GetProperty(cardData, propertyKey);

        // Check direct property match
        if (cardValue is not null)
        {
            // Boolean comparison
            if (expectedValue is bool expectedBool && cardValue is bool cardBool)
            {
                return cardBool == expectedBool;
            }
            // String comparison (case insensitive)
            if (expectedValue is string expectedStr && cardValue.ToString() is string cardStr)
            {
                return string.Equals(cardStr, expectedStr, StringComparison.OrdinalIgnoreCase);
            }
            // Direct value comparison
            return Equals(cardValue, expectedValue);
        }

        // Check in array properties (finishes, frame_effects, promo_types)
        if (expectedValue is bool boolValue && boolValue)
        {
            // For boolean true, check if the property name exists in arrays
            if (CheckArrayContains(cardData, "finishes", propertyKey) ||
                CheckArrayContains(cardData, "frame_effects", propertyKey) ||
                CheckArrayContains(cardData, "promo_types", propertyKey))
            {
                return true;
            }
        }
        else if (expectedValue is string stringValue)
        {
            // For string values, check if the value exists in arrays
            if (propertyKey == "frame" && CheckArrayContains(cardData, "frame_effects", stringValue))
            {
                return true;
            }

            if (propertyKey == "border" && CheckBorderColor(cardData, stringValue))
            {
                return true;
            }

            if (CheckArrayContains(cardData, "finishes", stringValue) ||
                CheckArrayContains(cardData, "frame_effects", stringValue) ||
                CheckArrayContains(cardData, "promo_types", stringValue))
            {
                return true;
            }
        }

        // For boolean false, we want NOT found
        if (expectedValue is bool falseBool && falseBool is false)
        {
            // If we reached here and it's false, that means we didn't find it, which is what we want
            return true;
        }

        return false;
    }

    private static bool CheckArrayContains(dynamic cardData, string arrayProperty, string value)
    {
        JArray array = GetArrayProperty(cardData, arrayProperty);
        if (array is null || array.Count == 0)
        {
            return false;
        }

        return array.Any(item =>
        {
            string itemStr = item.ToString();
            return string.Equals(itemStr, value, StringComparison.OrdinalIgnoreCase);
        });
    }

    private static bool CheckBorderColor(dynamic cardData, string expectedColor)
    {
        string borderColor = GetStringProperty(cardData, "border_color");
        return string.Equals(borderColor, expectedColor, StringComparison.OrdinalIgnoreCase);
    }

    private static object GetProperty(dynamic data, string propertyName)
    {
        if (data is JObject jobj)
        {
            if (jobj.TryGetValue(propertyName, out JToken token))
            {
                if (token.Type == JTokenType.Boolean)
                {
                    return token.Value<bool>();
                }

                if (token.Type == JTokenType.String)
                {
                    return token.Value<string>();
                }

                return token;
            }
        }

        return null;
    }

    private static string GetStringProperty(dynamic data, string propertyName)
    {
        if (data is JObject jobj)
        {
            if (jobj.TryGetValue(propertyName, out JToken token))
            {
                return token.ToString();
            }
        }

        return null;
    }

    private static JArray GetArrayProperty(dynamic data, string propertyName)
    {
        if (data is JObject jobj)
        {
            if (jobj.TryGetValue(propertyName, out JToken token) && token is JArray array)
            {
                return array;
            }
        }

        return null;
    }
}
