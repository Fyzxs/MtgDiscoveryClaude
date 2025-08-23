using System.Collections.Generic;

namespace Lib.Shared.DataModels.Entities;

public interface ICardFaceItrEntity
{
    string ObjectString { get; }
    string Name { get; }
    string ManaCost { get; }
    string TypeLine { get; }
    string OracleText { get; }
    ICollection<string> Colors { get; }
    ICollection<string> ColorIndicator { get; }
    string Power { get; }
    string Toughness { get; }
    string Loyalty { get; }
    string Defense { get; }
    string Artist { get; }
    string ArtistId { get; }
    string IllustrationId { get; }
    IImageUrisItrEntity ImageUris { get; }
    string FlavorText { get; }
    string PrintedName { get; }
    string PrintedTypeLine { get; }
    string PrintedText { get; }
    string Watermark { get; }
    string Layout { get; }
    decimal? Cmc { get; }
}
