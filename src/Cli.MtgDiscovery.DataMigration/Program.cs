using System;
using System.Threading.Tasks;

namespace Cli.MtgDiscovery.DataMigration;

internal sealed class Program
{
    private static async Task<int> Main(string[] args)
    {
        DataMigrationApplication application = new();

        try
        {
            await application.StartUp(args).ConfigureAwait(false);
            return 0;
        }
#pragma warning disable CA1031 // Catching general Exception in top-level handler for migration utility
        catch (Exception ex)
#pragma warning restore CA1031
        {
            Console.WriteLine("=== UNHANDLED EXCEPTION ===");
            Console.WriteLine($"Type: {ex.GetType().FullName}");
            Console.WriteLine($"Message: {ex.Message}");
            Console.WriteLine($"Stack Trace:\n{ex.StackTrace}");

            if (ex.InnerException != null)
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
}
