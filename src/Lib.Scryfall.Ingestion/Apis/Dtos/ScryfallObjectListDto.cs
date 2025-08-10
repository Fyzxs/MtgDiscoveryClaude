namespace Lib.Scryfall.Ingestion.Apis.Dtos;

/// <summary>
/// Represents a paginated list response from Scryfall API.
/// </summary>
public sealed class ScryfallObjectListDto
{
    private readonly dynamic _rawData;

    public ScryfallObjectListDto(dynamic rawData)
    {
        _rawData = rawData;
    }

    public dynamic Data => _rawData.data;
    public bool HasMore => _rawData.has_more ?? false;
    public string NextPage => _rawData.next_page;
    public int TotalCards => _rawData.total_cards ?? 0;
}
