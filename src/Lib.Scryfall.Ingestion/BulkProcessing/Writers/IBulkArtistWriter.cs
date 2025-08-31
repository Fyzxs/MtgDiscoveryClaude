using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Scryfall.Ingestion.BulkProcessing.Storage;

namespace Lib.Scryfall.Ingestion.BulkProcessing.Writers;

internal interface IBulkArtistWriter
{
    Task WriteArtistsAsync(IEnumerable<BulkArtistData> artists);
}