using System.IO;
using System.Threading.Tasks;
using Lib.Scryfall.Ingestion.BulkProcessing.Storage;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Lib.Scryfall.Ingestion.BulkProcessing.Loaders;

internal sealed class BulkCardLoader : IBulkCardLoader
{
    public async Task LoadCardsAsync(Stream cardsStream, IBulkSetStorage setStorage, IBulkArtistStorage artistStorage)
    {
        using StreamReader reader = new(cardsStream);
        using JsonTextReader jsonReader = new(reader);

        JsonSerializer serializer = new();

        while (await jsonReader.ReadAsync().ConfigureAwait(false))
        {
            if (jsonReader.TokenType == JsonToken.StartObject)
            {
                JObject cardData = serializer.Deserialize<JObject>(jsonReader);
                ProcessCard(cardData, setStorage, artistStorage);
            }
        }
    }

    private static void ProcessCard(JObject cardData, IBulkSetStorage setStorage, IBulkArtistStorage artistStorage)
    {
        string cardId = cardData["id"]?.ToString();
        string setId = cardData["set_id"]?.ToString();
        string artist = cardData["artist"]?.ToString();

        if (string.IsNullOrWhiteSpace(cardId) || string.IsNullOrWhiteSpace(setId))
        {
            return;
        }

        // Add card to set
        if (setStorage.Contains(setId))
        {
            setStorage.AddCard(setId, cardId);
        }

        // Add artist association
        if (string.IsNullOrWhiteSpace(artist) is false)
        {
            artistStorage.AddArtistToCard(artist, cardId, setId);
        }
    }
}