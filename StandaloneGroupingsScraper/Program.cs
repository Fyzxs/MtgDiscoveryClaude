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
        int maxSetsToProcess = args.Length > 0 && int.TryParse(args[0], out var max) ? max : int.MaxValue;

        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("User-Agent", "MtgDiscoveryGroupingScraper/1.0");

        var allGroupings = await ScrapeAllSetGroupings(httpClient, maxSetsToProcess);

        var json = JsonConvert.SerializeObject(allGroupings, Formatting.Indented);
        await File.WriteAllTextAsync("scryfall_set_groupings.json", json);

        Console.WriteLine($"Successfully scraped {allGroupings.Count} sets");
        Console.WriteLine($"Results saved to: scryfall_set_groupings.json");

        var totalGroupings = allGroupings.Values.Sum(s => s.Groupings.Count);
        Console.WriteLine($"Total groupings found: {totalGroupings}");
    }

    static async Task<Dictionary<string, SetGroupingData>> ScrapeAllSetGroupings(HttpClient httpClient, int maxSetsToProcess)
    {
        // Load existing data if file exists
        var setGroupings = new Dictionary<string, SetGroupingData>();
        const string fileName = "scryfall_set_groupings.json";

        if (File.Exists(fileName))
        {
            Console.WriteLine("Loading existing groupings file...");
            var existingJson = await File.ReadAllTextAsync(fileName);
            setGroupings = JsonConvert.DeserializeObject<Dictionary<string, SetGroupingData>>(existingJson)
                          ?? new Dictionary<string, SetGroupingData>();
            Console.WriteLine($"Loaded {setGroupings.Count} existing sets");
        }

        var allSetCodes = await GetAllSetCodes(httpClient);

        // Filter out already processed sets
        var setsToProcess = allSetCodes.Where(code => !setGroupings.ContainsKey(code)).ToList();

        Console.WriteLine($"Total sets available: {allSetCodes.Count}");
        Console.WriteLine($"Already processed: {setGroupings.Count}");
        Console.WriteLine($"Sets to process: {setsToProcess.Count}");

        // Limit number of sets to process if specified
        if (maxSetsToProcess < setsToProcess.Count)
        {
            setsToProcess = setsToProcess.Take(maxSetsToProcess).ToList();
            Console.WriteLine($"Limiting to {maxSetsToProcess} sets for this run");
        }

        int processed = 0;
        int newSetsAdded = 0;

        if (setsToProcess.Count > 0)
        {
            Console.WriteLine($"\nStarting to process {setsToProcess.Count} sets...\n");
        }
        else
        {
            Console.WriteLine("\nNo new sets to process. All sets are already in the file.");
            return setGroupings;
        }

        foreach (var setCode in setsToProcess)
        {
            try
            {
                var groupings = await ScrapeSetGroupings(httpClient, setCode);
                if (groupings.Count > 0)
                {
                    setGroupings[setCode] = new SetGroupingData { SetCode = setCode, Groupings = groupings };
                    newSetsAdded++;
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

        Console.WriteLine($"Added {newSetsAdded} new sets in this run");

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

            // Extract anchor ID if present
            string anchorId = "";
            var idIndex = section.IndexOf("id=\"");
            if (idIndex >= 0 && idIndex < 100) // Must be near the start
            {
                var idStart = idIndex + 4;
                var idEnd = section.IndexOf("\"", idStart);
                if (idEnd > idStart)
                {
                    anchorId = section.Substring(idStart, idEnd - idStart);
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

        // Parse collector number ranges
        var cnMinMatch = Regex.Match(query, @"cn[≥>=](\d+)");
        var cnMaxMatch = Regex.Match(query, @"cn[≤<=](\d+)");

        if (cnMinMatch.Success || cnMaxMatch.Success)
        {
            filters.CollectorNumberRange = new CollectorNumberRange
            {
                Min = cnMinMatch.Success ? cnMinMatch.Groups[1].Value : null,
                Max = cnMaxMatch.Success ? cnMaxMatch.Groups[1].Value : null
            };
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

        // type:X patterns
        var typeMatch = Regex.Match(query, @"type:(\w+)");
        if (typeMatch.Success)
        {
            filters.Properties["type_line_contains"] = typeMatch.Groups[1].Value;
        }

        // -type:X patterns
        var notTypeMatch = Regex.Match(query, @"-type:(\w+)");
        if (notTypeMatch.Success)
        {
            filters.Properties["type_line_excludes"] = notTypeMatch.Groups[1].Value;
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
}