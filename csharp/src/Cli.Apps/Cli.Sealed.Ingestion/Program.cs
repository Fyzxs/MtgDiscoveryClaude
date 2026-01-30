using System.Threading.Tasks;

namespace Cli.Sealed.Ingestion;

internal sealed class Program
{
    private static async Task Main(string[] args)
    {
        SealedProductIngestionApplication app = new(args);
        await app.StartUp(args).ConfigureAwait(false);
    }
}
