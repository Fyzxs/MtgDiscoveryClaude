using System.Threading.Tasks;

namespace Example.Scryfall.CosmosIngestion;

internal sealed class Program
{
    private static async Task Main(string[] args)
    {
        ScryfallCosmosIngestionApplication app = new();
        await app.StartUp(args).ConfigureAwait(false);
    }
}
