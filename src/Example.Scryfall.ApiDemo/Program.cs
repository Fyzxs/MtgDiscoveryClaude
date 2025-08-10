using System.Threading.Tasks;

namespace Example.Scryfall.ApiDemo;

class Program
{
    static async Task Main(string[] args)
    {
        ScryfallApiDemoApplication app = new ScryfallApiDemoApplication();
        await app.StartUp(args).ConfigureAwait(false);
    }
}