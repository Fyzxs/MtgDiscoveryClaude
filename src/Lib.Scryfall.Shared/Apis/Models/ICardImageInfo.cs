using System.Collections.Generic;
using Lib.Universal.Primitives;

namespace Lib.Scryfall.Shared.Apis.Models;

/// <summary>
/// 
/// </summary>
public interface ICardImageInfo
{
    Url ImageUrl();
    IDictionary<string, string> Metadata();
    string LogValue();
}
