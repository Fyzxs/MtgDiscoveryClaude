using System;
using System.IO;
using System.Threading.Tasks;
using Lib.Scryfall.Ingestion.BulkProcessing.Models;
using Lib.Scryfall.Ingestion.BulkProcessing.Storage;
using Newtonsoft.Json;

namespace Lib.Scryfall.Ingestion.BulkProcessing.Loaders;

internal sealed class BulkRulingLoader : IBulkRulingLoader
{
    public async Task LoadRulingsAsync(Stream rulingsStream, IBulkRulingStorage rulingStorage)
    {
        using StreamReader reader = new(rulingsStream);
        using JsonTextReader jsonReader = new(reader);

        JsonSerializer serializer = new();

        while (await jsonReader.ReadAsync().ConfigureAwait(false))
        {
            if (jsonReader.TokenType == JsonToken.StartObject)
            {
                ScryfallBulkRuling ruling = serializer.Deserialize<ScryfallBulkRuling>(jsonReader);
                if (ruling != null && string.IsNullOrWhiteSpace(ruling.OracleId) is false)
                {
                    BulkRulingEntry entry = new()
                    {
                        Source = ruling.Source,
                        PublishedAt = ruling.PublishedAt,
                        Comment = ruling.Comment
                    };

                    rulingStorage.AddRuling(ruling.OracleId, entry);
                }
            }
        }
    }
}