using System.Threading.Tasks;

namespace Example.Scryfall.ConfigDemo;

class Program
{
    static async Task Main(string[] args)
    {
        ScryfallConfigDemoApplication app = new ScryfallConfigDemoApplication();
        await app.StartUp(args).ConfigureAwait(false);
    }
}