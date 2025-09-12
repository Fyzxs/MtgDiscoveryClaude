using System.Threading.Tasks;

namespace Example.Scryfall.BulkIngestion;

internal class Program
{
    private static async Task Main(string[] args)
    {
        ScryfallBulkIngestionApplication app = new();
        await app.StartUp(args).ConfigureAwait(false);
    }
}
