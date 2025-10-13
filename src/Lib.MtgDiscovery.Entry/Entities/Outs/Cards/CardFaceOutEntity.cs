#pragma warning disable CA1056, CA1819
using System.Collections.Generic;

namespace Lib.MtgDiscovery.Entry.Entities.Outs.Cards;

public class CardFaceOutEntity
{
    public string ObjectString { get; set; }

    public string Name { get; set; }

    public string ManaCost { get; set; }

    public string TypeLine { get; set; }

    public string OracleText { get; set; }

    public ICollection<string> Colors { get; set; }

    public ICollection<string> ColorIndicator { get; set; }

    public string Power { get; set; }

    public string Toughness { get; set; }

    public string Loyalty { get; set; }

    public string Defense { get; set; }

    public string Artist { get; set; }

    public string ArtistId { get; set; }

    public string IllustrationId { get; set; }

    public ImageUrisOutEntity ImageUris { get; set; }

    public string FlavorText { get; set; }

    public string PrintedName { get; set; }

    public string PrintedTypeLine { get; set; }

    public string PrintedText { get; set; }

    public string Watermark { get; set; }

    public string Layout { get; set; }

    public decimal? Cmc { get; set; }
}
