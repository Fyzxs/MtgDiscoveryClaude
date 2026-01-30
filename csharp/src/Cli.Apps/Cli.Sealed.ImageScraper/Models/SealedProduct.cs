namespace Cli.Sealed.ImageScraper.Models;

internal sealed class SealedProduct
{
    public string Uuid { get; init; }
    public string Name { get; init; }
    public string SetCode { get; init; }
    public string SetName { get; init; }
    public string Category { get; init; }
    public string Subtype { get; init; }
    public string TcgplayerProductId { get; init; }
    public string McmId { get; init; }
    public string CardTraderId { get; init; }

    public bool HasTcgplayerProductId => string.IsNullOrWhiteSpace(TcgplayerProductId) is false;
    public bool HasMcmId => string.IsNullOrWhiteSpace(McmId) is false;
    public bool HasCardTraderId => string.IsNullOrWhiteSpace(CardTraderId) is false;
}
