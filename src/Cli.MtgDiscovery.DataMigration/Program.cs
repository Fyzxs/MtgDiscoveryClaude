using System;
using System.Threading.Tasks;

namespace Cli.MtgDiscovery.DataMigration;

internal sealed class Program
{
    private static async Task<int> Main(string[] args)
    {
        DataMigrationApplication application = new DataMigrationApplication();

        try
        {
            await application.StartUp(args).ConfigureAwait(false);
            return 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unhandled exception: {ex.Message}");
            return 1;
        }
    }
}
