using System.Collections.Generic;

namespace Cli.Sealed.ImageScraper.Models;

internal sealed class ImageSize
{
    public static readonly ImageSize Size200 = new(200);
    public static readonly ImageSize Size400 = new(400);
    public static readonly ImageSize Size600 = new(600);
    public static readonly ImageSize Size800 = new(800);
    public static readonly ImageSize Size1000 = new(1000);

    public static readonly IReadOnlyList<ImageSize> All =
    [
        Size200,
        Size400,
        Size600,
        Size800,
        Size1000
    ];

    public static readonly IReadOnlyList<ImageSize> AllDescending =
    [
        Size1000,
        Size800,
        Size600,
        Size400,
        Size200
    ];

    public int Value { get; }

    private ImageSize(int value) => Value = value;

    public string ToFileSuffix() => $"{Value}x{Value}";

    public override string ToString() => ToFileSuffix();
}
