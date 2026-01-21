using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cli.Sealed.ImageScraper;

internal sealed class Program
{
    private static async Task<int> Main(string[] args)
    {
        IReadOnlyList<string> setCodes = ParseSetCodes(args);

        ImageScraperApplication application = new(setCodes);

        try
        {
            await application.StartUp(args).ConfigureAwait(false);
            return 0;
        }
#pragma warning disable CA1031 // Catching general Exception in top-level handler for CLI utility
        catch (Exception ex)
#pragma warning restore CA1031
        {
            Console.WriteLine("=== UNHANDLED EXCEPTION ===");
            Console.WriteLine($"Type: {ex.GetType().FullName}");
            Console.WriteLine($"Message: {ex.Message}");
            Console.WriteLine($"Stack Trace:\n{ex.StackTrace}");

            if (ex.InnerException is not null)
            {
                Console.WriteLine("\n=== INNER EXCEPTION ===");
                Console.WriteLine($"Type: {ex.InnerException.GetType().FullName}");
                Console.WriteLine($"Message: {ex.InnerException.Message}");
                Console.WriteLine($"Stack Trace:\n{ex.InnerException.StackTrace}");
            }

            Console.WriteLine("\n=== FULL EXCEPTION ===");
            Console.WriteLine(ex.ToString());
            return 1;
        }
    }

    private static IReadOnlyList<string> ParseSetCodes(string[] args)
    {
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i].Equals("--all", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            if (args[i].Equals("--sets", StringComparison.OrdinalIgnoreCase) && i + 1 < args.Length)
            {
                return args[i + 1]
                    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    .Select(s => s.ToUpperInvariant())
                    .ToList();
            }
        }

        return Array.Empty<string>();
    }
}
