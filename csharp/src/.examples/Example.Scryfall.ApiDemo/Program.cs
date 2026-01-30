using System.Threading.Tasks;

namespace Example.Scryfall.ApiDemo;

internal sealed class Program
{
    private static async Task Main(string[] args)
    {
        ScryfallApiDemoApplication app = new();
        await app.StartUp(args).ConfigureAwait(false);
    }
}
