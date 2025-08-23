using Lib.Shared.DataModels.Entities;

namespace Lib.Aggregator.Cards.Models;

internal sealed class PreviewItrEntity : IPreviewItrEntity
{
    private readonly dynamic _data;

    public PreviewItrEntity(dynamic data)
    {
        _data = data;
    }

    public string Source => _data?.source;
    public string SourceUri => _data?.source_uri;
    public string PreviewedAt => _data?.previewed_at;
}
