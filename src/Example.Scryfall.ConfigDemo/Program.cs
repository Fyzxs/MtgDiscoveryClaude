namespace Example.Scryfall.ConfigDemo;

internal class Program
{
    private static async Task Main(string[] args)
    {
        ScryfallConfigDemoApplication app = new();
        await app.StartUp(args).ConfigureAwait(false);
    }
}
