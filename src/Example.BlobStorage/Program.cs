using System.Threading.Tasks;

namespace Example.BlobStorage;

internal static class Program
{
    private static async Task Main(string[] args)
    {
        BlobStorageExampleApplication app = new();
        await app.StartUp(args).ConfigureAwait(false);
    }
}