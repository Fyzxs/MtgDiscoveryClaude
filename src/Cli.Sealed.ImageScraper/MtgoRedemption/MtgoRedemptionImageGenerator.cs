using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using SkiaSharp;
using Svg.Skia;

namespace Cli.Sealed.ImageScraper.MtgoRedemption;

internal sealed class MtgoRedemptionImageGenerator : IMtgoRedemptionImageGenerator
{
    private const string ScryfallIconUrlBase = "https://svgs.scryfall.io/sets/";
    private const int ImageWidth = 600;
    private const int ImageHeight = 800;
    private const int IconSize = 250;
    private const int TextPadding = 40;
    private const int MaxTextWidth = 520;
    private const float RedemptionFontSize = 42f;
    private const float SetNameFontSize = 36f;

    private static readonly SKColor s_backgroundColor = SKColors.White;
    private static readonly SKColor s_greenTextColor = new(0, 128, 0);
    private static readonly SKColor s_goldTextColor = new(218, 165, 32);
    private static readonly SKColor s_setNameColor = SKColors.Black;

    private static readonly Dictionary<string, string> s_setIconMapping = new(StringComparer.OrdinalIgnoreCase)
    {
        { "TSB", "TSP" }
    };

    private readonly HttpClient _httpClient;

    public MtgoRedemptionImageGenerator(HttpClient httpClient) => _httpClient = httpClient;

    public async Task<bool> GenerateAsync(
        string setCode,
        string setName,
        bool isFoil,
        string outputPath,
        CancellationToken cancellationToken)
    {
        try
        {
            using SKSvg svg = await DownloadSetIconAsync(setCode, cancellationToken)
                .ConfigureAwait(false);

            if (svg?.Picture is null)
            {
                return false;
            }

            using SKBitmap bitmap = GenerateImage(svg.Picture, setName, isFoil);

            return SaveImage(bitmap, outputPath);
        }
        catch (HttpRequestException)
        {
            return false;
        }
#pragma warning disable CA1031 // Catch general exception to return false on any failure
        catch (Exception)
        {
            return false;
        }
#pragma warning restore CA1031
    }

    private async Task<SKSvg> DownloadSetIconAsync(string setCode, CancellationToken cancellationToken)
    {
        string iconSetCode = s_setIconMapping.TryGetValue(setCode, out string mappedCode)
            ? mappedCode
            : setCode;

#pragma warning disable CA1308 // URL requires lowercase set code
        string url = $"{ScryfallIconUrlBase}{iconSetCode.ToLowerInvariant()}.svg";
#pragma warning restore CA1308

        using HttpResponseMessage response = await _httpClient
            .GetAsync(new Uri(url), cancellationToken)
            .ConfigureAwait(false);

        if (response.IsSuccessStatusCode is false)
        {
            return null;
        }

        string svgContent = await response.Content
            .ReadAsStringAsync(cancellationToken)
            .ConfigureAwait(false);

        SKSvg svg = new();
        svg.FromSvg(svgContent);
        return svg;
    }

#pragma warning disable CA2000 // Bitmap ownership transfers to caller who disposes
    private static SKBitmap GenerateImage(SKPicture iconPicture, string setName, bool isFoil)
    {
        SKBitmap bitmap = new(ImageWidth, ImageHeight);

        using SKCanvas canvas = new(bitmap);

        canvas.Clear(s_backgroundColor);
        DrawIcon(canvas, iconPicture);
        DrawSetName(canvas, setName);
        DrawRedemptionText(canvas, isFoil);

        return bitmap;
    }
#pragma warning restore CA2000

    private static void DrawIcon(SKCanvas canvas, SKPicture picture)
    {
        SKRect bounds = picture.CullRect;

        float scale = Math.Min(
            (float)IconSize / bounds.Width,
            (float)IconSize / bounds.Height);

        float scaledWidth = bounds.Width * scale;
        float scaledHeight = bounds.Height * scale;

        float x = (ImageWidth - scaledWidth) / 2;
        float y = 80;

        canvas.Save();
        canvas.Translate(x, y);
        canvas.Scale(scale);
        canvas.DrawPicture(picture);
        canvas.Restore();
    }

    private static void DrawSetName(SKCanvas canvas, string setName)
    {
        if (string.IsNullOrWhiteSpace(setName))
        {
            return;
        }

        using SKTypeface typeface = SKTypeface.FromFamilyName(
            "Arial",
            SKFontStyleWeight.Bold,
            SKFontStyleWidth.Normal,
            SKFontStyleSlant.Upright);

        using SKFont font = new(typeface, SetNameFontSize);

        using SKPaint paint = new()
        {
            Color = s_setNameColor,
            IsAntialias = true
        };

        IReadOnlyList<string> lines = WrapText(setName, font, MaxTextWidth);
        float lineHeight = font.Spacing;
        float startY = 380;

        foreach (string line in lines)
        {
            float textWidth = font.MeasureText(line);
            float x = (ImageWidth - textWidth) / 2;

            canvas.DrawText(line, x, startY, SKTextAlign.Left, font, paint);
            startY += lineHeight;
        }
    }

    private static IReadOnlyList<string> WrapText(string text, SKFont font, float maxWidth)
    {
        List<string> lines = [];
        string[] words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (words.Length == 0)
        {
            return lines;
        }

        string currentLine = words[0];

        for (int i = 1; i < words.Length; i++)
        {
            string testLine = $"{currentLine} {words[i]}";
            float testWidth = font.MeasureText(testLine);

            if (testWidth > maxWidth)
            {
                lines.Add(currentLine);
                currentLine = words[i];
            }
            else
            {
                currentLine = testLine;
            }
        }

        lines.Add(currentLine);
        return lines;
    }

    private static void DrawRedemptionText(SKCanvas canvas, bool isFoil)
    {
        string text = isFoil ? "MTGO REDEMPTION FOIL" : "MTGO REDEMPTION";
        SKColor textColor = isFoil ? s_goldTextColor : s_greenTextColor;

        using SKTypeface typeface = SKTypeface.FromFamilyName(
            "Arial",
            SKFontStyleWeight.Bold,
            SKFontStyleWidth.Normal,
            SKFontStyleSlant.Upright);

        using SKFont font = new(typeface, RedemptionFontSize);

        using SKPaint paint = new()
        {
            Color = textColor,
            IsAntialias = true
        };

        float textWidth = font.MeasureText(text);
        float x = (ImageWidth - textWidth) / 2;
        float y = ImageHeight - TextPadding - 60;

        canvas.DrawText(text, x, y, SKTextAlign.Left, font, paint);
    }

    private static bool SaveImage(SKBitmap bitmap, string outputPath)
    {
        string directory = Path.GetDirectoryName(outputPath);
        if (string.IsNullOrEmpty(directory) is false && Directory.Exists(directory) is false)
        {
            _ = Directory.CreateDirectory(directory);
        }

        using SKImage image = SKImage.FromBitmap(bitmap);
        using SKData data = image.Encode(SKEncodedImageFormat.Jpeg, 90);

        using FileStream fileStream = new(outputPath, FileMode.Create, FileAccess.Write);
        data.SaveTo(fileStream);

        return true;
    }
}
