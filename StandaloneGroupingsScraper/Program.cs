using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

public class ScryfallGroupingsScraper
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Starting Scryfall Set Groupings Scraper...");

        // Parse command line arguments
        string specificSet = null;
        int maxSetsToProcess = int.MaxValue;
        
        if (args.Length > 0)
        {
            // Check if first arg is a set code (letters) or a number
            if (!int.TryParse(args[0], out maxSetsToProcess))
            {
                specificSet = args[0].ToLower();
                Console.WriteLine($"Processing specific set: {specificSet}");
            }
            
            // Check for second argument if first was a set code
            if (specificSet != null && args.Length > 1 && int.TryParse(args[1], out var max))
            {
                maxSetsToProcess = max;
            }
        }

        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("User-Agent", "MtgDiscoveryGroupingScraper/1.0");

        Dictionary<string, SetGroupingData> allGroupings;
        
        if (specificSet != null)
        {
            // Process only the specific set
            allGroupings = await ScrapeSpecificSet(httpClient, specificSet);
        }
        else
        {
            allGroupings = await ScrapeAllSetGroupings(httpClient, maxSetsToProcess);
        }

        var json = JsonConvert.SerializeObject(allGroupings, Formatting.Indented);
        await File.WriteAllTextAsync("scryfall_set_groupings.json", json);

        Console.WriteLine($"Successfully scraped {allGroupings.Count} sets");
        Console.WriteLine($"Results saved to: scryfall_set_groupings.json");

        var totalGroupings = allGroupings.Values.Sum(s => s.Groupings.Count);
        Console.WriteLine($"Total groupings found: {totalGroupings}");
    }

    static async Task<Dictionary<string, SetGroupingData>> ScrapeSpecificSet(HttpClient httpClient, string setCode)
    {
        var setGroupings = new Dictionary<string, SetGroupingData>();
        
        try
        {
            Console.WriteLine($"\nScraping set: {setCode}");
            var groupings = await ScrapeSetGroupings(httpClient, setCode);
            
            if (groupings.Count > 0)
            {
                setGroupings[setCode] = new SetGroupingData { SetCode = setCode, Groupings = groupings };
                Console.WriteLine($"Found {groupings.Count} groupings for {setCode}");
                
                // Print detailed info for debugging
                foreach (var grouping in groupings)
                {
                    Console.WriteLine($"\n  Grouping: {grouping.DisplayName}");
                    Console.WriteLine($"    Cards: {grouping.CardCount}");
                    Console.WriteLine($"    Query: {grouping.RawQuery}");
                    if (grouping.ParsedFilters?.CollectorNumberRange != null)
                    {
                        var cnr = grouping.ParsedFilters.CollectorNumberRange;
                        if (cnr.Min != null || cnr.Max != null)
                        {
                            Console.WriteLine($"    CN Range: {cnr.Min ?? "?"} - {cnr.Max ?? "?"}");
                        }
                        if (cnr.OrConditions != null && cnr.OrConditions.Count > 0)
                        {
                            Console.WriteLine($"    OR Conditions: {string.Join(", ", cnr.OrConditions)}");
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine($"No groupings found for {setCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error scraping set {setCode}: {ex.Message}");
        }
        
        return setGroupings;
    }

    static async Task<Dictionary<string, SetGroupingData>> ScrapeAllSetGroupings(HttpClient httpClient, int maxSetsToProcess)
    {
        // Start fresh - don't load existing data
        var setGroupings = new Dictionary<string, SetGroupingData>();
        const string fileName = "scryfall_set_groupings.json";

        var allSetCodes = await GetAllSetCodes(httpClient);
        var setsToProcess = allSetCodes;

        Console.WriteLine($"Total sets available: {allSetCodes.Count}");

        // Limit number of sets to process if specified
        if (maxSetsToProcess < setsToProcess.Count)
        {
            setsToProcess = setsToProcess.Take(maxSetsToProcess).ToList();
            Console.WriteLine($"Limiting to {maxSetsToProcess} sets for this run");
        }

        int processed = 0;
        int setsAdded = 0;

        Console.WriteLine($"\nStarting to process {setsToProcess.Count} sets...\n");

        foreach (var setCode in setsToProcess)
        {
            try
            {
                var groupings = await ScrapeSetGroupings(httpClient, setCode);
                if (groupings.Count > 0)
                {
                    setGroupings[setCode] = new SetGroupingData { SetCode = setCode, Groupings = groupings };
                    setsAdded++;
                }

                processed++;

                // Show status for every set
                Console.WriteLine($"[{processed}/{setsToProcess.Count}] Processed {setCode} - Found {groupings.Count} groupings");

                // Save progress every 5 sets
                if (processed % 5 == 0)
                {
                    var progressJson = JsonConvert.SerializeObject(setGroupings, Formatting.Indented);
                    await File.WriteAllTextAsync(fileName, progressJson);
                    Console.WriteLine($"  -> Saved progress ({setGroupings.Count} total sets in file)");
                }

                await Task.Delay(100); // Be respectful
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error scraping set {setCode}: {ex.Message}");
            }
        }

        Console.WriteLine($"Processed {setsAdded} sets total");

        return setGroupings;
    }

    static async Task<List<string>> GetAllSetCodes(HttpClient httpClient)
    {
        var html = await httpClient.GetStringAsync("https://scryfall.com/sets");
        var pattern = @"href=""/sets/([a-z0-9]+)""";
        var matches = Regex.Matches(html, pattern, RegexOptions.IgnoreCase);

        return matches
            .Select(m => m.Groups[1].Value)
            .Distinct()
            .ToList();
    }

    static async Task<List<CardGrouping>> ScrapeSetGroupings(HttpClient httpClient, string setCode)
    {
        var url = $"https://scryfall.com/sets/{setCode}";
        Console.WriteLine($"  -> Fetching {url}...");

        var html = await httpClient.GetStringAsync(url);
        Console.WriteLine($"  -> Received {html.Length:N0} characters of HTML");

        var groupings = new List<CardGrouping>();

        // Split by the known header pattern - much faster than regex
        var headerSplits = html.Split(new[] { "<h2 class=\"card-grid-header\"" }, StringSplitOptions.RemoveEmptyEntries);
        Console.WriteLine($"  -> Found {headerSplits.Length - 1} potential card group headers");

        int order = 0;
        for (int i = 1; i < headerSplits.Length; i++) // Skip first split (before any headers)
        {
            var section = headerSplits[i];

            // Extract anchor ID if present (can be in h2 tag or in an inner <a> tag)
            string anchorId = "";
            
            // First check for id in the h2 tag (within first 100 chars)
            var h2IdIndex = section.IndexOf("id=\"");
            if (h2IdIndex >= 0 && h2IdIndex < 100)
            {
                var idStart = h2IdIndex + 4;
                var idEnd = section.IndexOf("\"", idStart);
                if (idEnd > idStart)
                {
                    anchorId = section.Substring(idStart, idEnd - idStart);
                }
            }
            
            // If not found in h2, look for <a id="..."> within the content
            if (string.IsNullOrEmpty(anchorId))
            {
                var anchorPattern = @"<a\s+id=""([^""]+)""";
                var anchorMatch = Regex.Match(section.Substring(0, Math.Min(section.Length, 500)), anchorPattern);
                if (anchorMatch.Success)
                {
                    anchorId = anchorMatch.Groups[1].Value;
                }
            }

            // Find the content span
            var contentIndex = section.IndexOf("<span class=\"card-grid-header-content\">");
            if (contentIndex < 0) continue;

            var contentStart = contentIndex + 40; // Length of the span tag
            var dotIndex = section.IndexOf("•", contentStart);
            if (dotIndex < 0) continue;

            // Extract display name (might have <a> tags)
            var nameSection = section.Substring(contentStart, dotIndex - contentStart);
            var displayName = nameSection;

            // Remove any <a> tags if present
            if (displayName.Contains("<a"))
            {
                var linkStart = displayName.IndexOf(">") + 1;
                var linkEnd = displayName.IndexOf("</a>");
                if (linkEnd > linkStart)
                {
                    displayName = displayName.Substring(linkStart, linkEnd - linkStart);
                }
            }
            
            // Clean up whitespace, newlines, and any remaining HTML entities
            displayName = displayName.Replace("\n", " ").Replace("\r", " ").Replace("\t", " ");
            displayName = System.Text.RegularExpressions.Regex.Replace(displayName, @"\s+", " ");
            displayName = displayName.Trim();
            
            // Remove any remaining HTML tags
            if (displayName.Contains("<"))
            {
                displayName = System.Text.RegularExpressions.Regex.Replace(displayName, @"<[^>]+>", "");
                displayName = displayName.Trim(); // Trim again after removing tags
            }
            
            // Decode HTML entities (e.g., &amp; -> &, &quot; -> ", etc.)
            displayName = HttpUtility.HtmlDecode(displayName);

            // Find the href with card count
            var hrefIndex = section.IndexOf("href=\"", dotIndex);
            if (hrefIndex < 0) continue;

            var hrefStart = hrefIndex + 6;
            var hrefEnd = section.IndexOf("\"", hrefStart);
            if (hrefEnd < 0) continue;

            var searchUrl = section.Substring(hrefStart, hrefEnd - hrefStart);

            // Extract card count
            var cardCountStart = hrefEnd + 2; // Skip ">
            var cardCountEnd = section.IndexOf(" card", cardCountStart);
            if (cardCountEnd < 0) continue;

            var cardCountStr = section.Substring(cardCountStart, cardCountEnd - cardCountStart);
            if (!int.TryParse(cardCountStr, out var cardCount)) continue;

            // Extract query parameter
            var queryIndex = searchUrl.IndexOf("q=");
            if (queryIndex < 0) continue;

            var query = HttpUtility.UrlDecode(searchUrl.Substring(queryIndex + 2).Split('&')[0]);

            Console.WriteLine($"     - {displayName} ({cardCount} cards)");

            groupings.Add(new CardGrouping
            {
                Id = string.IsNullOrEmpty(anchorId) ? $"group-{order}" : anchorId,
                DisplayName = displayName,
                Order = order++,
                CardCount = cardCount,
                RawQuery = query,
                ParsedFilters = ParseScryfallQuery(query)
            });
        }

        return groupings;
    }

    static GroupingFilters ParseScryfallQuery(string query)
    {
        var filters = new GroupingFilters();

        // Parse all collector number patterns
        var cnMinMatch = Regex.Match(query, @"cn[≥>=](\d+)");
        var cnMaxMatch = Regex.Match(query, @"cn[≤<=](\d+)");
        
        // Extract individual collector numbers (e.g., cn:"796" or cn:"796★")
        var cnExactPattern = @"cn:""?([^""\s\)]+)""?";
        var cnExactMatches = Regex.Matches(query, cnExactPattern);
        
        // Check if we have a complex condition (ranges AND/OR individual numbers)
        bool hasRange = cnMinMatch.Success || cnMaxMatch.Success;
        bool hasExactNumbers = cnExactMatches.Count > 0;
        bool hasOrCondition = query.Contains(" OR ");
        
        if (hasRange || hasExactNumbers)
        {
            var collectorNumberRange = new CollectorNumberRange();
            
            // Set range if present
            if (hasRange)
            {
                collectorNumberRange.Min = cnMinMatch.Success ? cnMinMatch.Groups[1].Value : null;
                collectorNumberRange.Max = cnMaxMatch.Success ? cnMaxMatch.Groups[1].Value : null;
            }
            
            // Handle OR conditions or exact matches
            if (hasOrCondition && hasExactNumbers)
            {
                // Complex case: might have ranges AND OR conditions
                var orConditions = new List<string>();
                foreach (Match match in cnExactMatches)
                {
                    orConditions.Add(match.Groups[1].Value);
                }
                
                // If we have both range and OR conditions, keep both
                // If we only have OR conditions (no range), just set OrConditions
                if (orConditions.Count > 0)
                {
                    collectorNumberRange.OrConditions = orConditions;
                }
            }
            else if (hasExactNumbers && cnExactMatches.Count == 1 && !hasRange)
            {
                // Single exact number with no range
                var cnValue = cnExactMatches[0].Groups[1].Value;
                collectorNumberRange.Min = cnValue;
                collectorNumberRange.Max = cnValue;
            }
            
            filters.CollectorNumberRange = collectorNumberRange;
        }

        filters.Properties = new Dictionary<string, object>();

        // is:X patterns
        var isMatches = Regex.Matches(query, @"is:(\w+)");
        foreach (Match match in isMatches)
        {
            filters.Properties[match.Groups[1].Value] = true;
        }

        // not:X patterns
        var notMatches = Regex.Matches(query, @"not:(\w+)");
        foreach (Match match in notMatches)
        {
            filters.Properties[match.Groups[1].Value] = false;
        }

        // border:X patterns
        var borderMatch = Regex.Match(query, @"border:(\w+)");
        if (borderMatch.Success)
        {
            filters.Properties["border"] = borderMatch.Groups[1].Value;
        }

        // frame:X patterns
        var frameMatch = Regex.Match(query, @"frame:(\w+)");
        if (frameMatch.Success)
        {
            filters.Properties["frame"] = frameMatch.Groups[1].Value;
        }

        // -type:X patterns (must check before type:X to avoid false matches)
        var notTypeMatch = Regex.Match(query, @"-type:(\w+)");
        if (notTypeMatch.Success)
        {
            filters.Properties["type_line_excludes"] = notTypeMatch.Groups[1].Value;
        }

        // type:X patterns (use negative lookbehind to exclude -type:)
        var typeMatch = Regex.Match(query, @"(?<!-)type:(\w+)");
        if (typeMatch.Success)
        {
            filters.Properties["type_line_contains"] = typeMatch.Groups[1].Value;
        }

        // date:YYYY-MM-DD patterns
        var dateMatch = Regex.Match(query, @"date:(\d{4}-\d{2}-\d{2})");
        if (dateMatch.Success)
        {
            filters.Properties["date"] = dateMatch.Groups[1].Value;
        }

        return filters;
    }
}

public class SetGroupingData
{
    public string SetCode { get; set; }
    public List<CardGrouping> Groupings { get; set; }
}

public class CardGrouping
{
    public string Id { get; set; }
    public string DisplayName { get; set; }
    public int Order { get; set; }
    public int CardCount { get; set; }
    public string RawQuery { get; set; }
    public GroupingFilters ParsedFilters { get; set; }
}

public class GroupingFilters
{
    public CollectorNumberRange CollectorNumberRange { get; set; }
    public Dictionary<string, object> Properties { get; set; }
}

public class CollectorNumberRange
{
    public string Min { get; set; }
    public string Max { get; set; }
    public List<string> OrConditions { get; set; }
}