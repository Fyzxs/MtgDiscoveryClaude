using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.MtgDiscovery.GraphQL.Entities.Outs.Cards;
using Lib.Shared.DataModels.Entities;

namespace App.MtgDiscovery.GraphQL.Mappers;

internal sealed class CardItemItrToOutMapper : ICardItemItrToOutMapper
{
    public Task<CardItemOutEntity> Map(ICardItemItrEntity source)
    {
        CardItemOutEntity result = new()
        {
            Id = source.Id,
            OracleId = source.OracleId,
            ArenaId = source.ArenaId,
            MtgoId = source.MtgoId,
            MtgoFoilId = source.MtgoFoilId,
            MultiverseIds = source.MultiverseIds,
            TcgPlayerId = source.TcgPlayerId,
            TcgPlayerEtchedId = source.TcgPlayerEtchedId,
            CardMarketId = source.CardMarketId,
            Name = source.Name,
            FlavorName = source.FlavorName,
            Lang = source.Lang,
            ReleasedAt = source.ReleasedAt,
            Uri = source.Uri,
            ScryfallUri = source.ScryfallUri,
            Layout = source.Layout,
            HighResImage = source.HighResImage,
            ImageStatus = source.ImageStatus,
            ImageUris = MapImageUris(source.ImageUris),
            ManaCost = source.ManaCost,
            Cmc = source.Cmc,
            TypeLine = source.TypeLine,
            OracleText = source.OracleText,
            Colors = source.Colors,
            ColorIdentity = source.ColorIdentity,
            Keywords = source.Keywords,
            Legalities = MapLegalities(source.Legalities),
            Games = source.Games,
            Reserved = source.Reserved,
            GameChanger = source.GameChanger,
            Foil = source.Foil,
            NonFoil = source.NonFoil,
            Finishes = source.Finishes,
            Oversized = source.Oversized,
            Promo = source.Promo,
            Reprint = source.Reprint,
            Variation = source.Variation,
            SetId = source.SetId,
            SetCode = source.SetCode,
            SetName = source.SetName,
            SetType = source.SetType,
            SetUri = source.SetUri,
            SetSearchUri = source.SetSearchUri,
            ScryfallSetUri = source.ScryfallSetUri,
            RulingsUri = source.RulingsUri,
            PrintsSearchUri = source.PrintsSearchUri,
            CollectorNumber = source.CollectorNumber,
            Digital = source.Digital,
            Rarity = source.Rarity,
            FlavorText = source.FlavorText,
            CardBackId = source.CardBackId,
            Artist = source.Artist,
            ArtistIds = source.ArtistIds,
            IllustrationId = source.IllustrationId,
            BorderColor = source.BorderColor,
            Frame = source.Frame,
            FrameEffects = source.FrameEffects,
            SecurityStamp = source.SecurityStamp,
            FullArt = source.FullArt,
            Textless = source.Textless,
            Booster = source.Booster,
            StorySpotlight = source.StorySpotlight,
            PromoTypes = source.PromoTypes,
            EdhRecRank = source.EdhRecRank,
            PennyRank = source.PennyRank,
            Prices = MapPrices(source.Prices),
            RelatedUris = MapRelatedUris(source.RelatedUris),
            PurchaseUris = MapPurchaseUris(source.PurchaseUris),
            Power = source.Power,
            Toughness = source.Toughness,
            Loyalty = source.Loyalty,
            Defense = source.Defense,
            LifeModifier = source.LifeModifier,
            HandModifier = source.HandModifier,
            ColorIndicator = source.ColorIndicator,
            CardFaces = MapCardFaces(source.CardFaces),
            AllParts = MapAllParts(source.AllParts),
            PrintedName = source.PrintedName,
            PrintedTypeLine = source.PrintedTypeLine,
            PrintedText = source.PrintedText,
            Watermark = source.Watermark,
            ContentWarning = source.ContentWarning,
            Preview = MapPreview(source.Preview),
            ProducedMana = source.ProducedMana,
            AttractionLights = source.AttractionLights
        };

        return Task.FromResult(result);
    }

    private static ImageUrisOutEntity MapImageUris(IImageUrisItrEntity source)
    {
        if (source == null) return null;

        return new ImageUrisOutEntity
        {
            Small = source.Small,
            Normal = source.Normal,
            Large = source.Large,
            Png = source.Png,
            ArtCrop = source.ArtCrop,
            BorderCrop = source.BorderCrop
        };
    }

    private static LegalitiesOutEntity MapLegalities(ILegalitiesItrEntity source)
    {
        if (source == null) return null;

        return new LegalitiesOutEntity
        {
            Standard = source.Standard,
            Future = source.Future,
            Historic = source.Historic,
            Timeless = source.Timeless,
            Gladiator = source.Gladiator,
            Pioneer = source.Pioneer,
            Explorer = source.Explorer,
            Modern = source.Modern,
            Legacy = source.Legacy,
            Pauper = source.Pauper,
            Vintage = source.Vintage,
            Penny = source.Penny,
            Commander = source.Commander,
            Oathbreaker = source.Oathbreaker,
            StandardBrawl = source.StandardBrawl,
            Brawl = source.Brawl,
            Alchemy = source.Alchemy,
            PauperCommander = source.PauperCommander,
            Duel = source.Duel,
            Oldschool = source.OldSchool,
            Premodern = source.Premodern,
            Predh = source.PrEdh
        };
    }

    private static PricesOutEntity MapPrices(IPricesItrEntity source)
    {
        if (source == null) return null;

        return new PricesOutEntity
        {
            Usd = source.Usd,
            UsdFoil = source.UsdFoil,
            UsdEtched = source.UsdEtched,
            Eur = null,
            EurFoil = null,
            Tix = null
        };
    }

    private static RelatedUrisOutEntity MapRelatedUris(IRelatedUrisItrEntity source)
    {
        if (source == null) return null;

        return new RelatedUrisOutEntity
        {
            Gatherer = source.Gatherer,
            TcgplayerInfiniteArticles = source.TcgPlayerInfiniteArticles,
            TcgplayerInfiniteDecks = source.TcgPlayerInfiniteDecks,
            Edhrec = source.EdhRec
        };
    }

    private static PurchaseUrisOutEntity MapPurchaseUris(IPurchaseUrisItrEntity source)
    {
        if (source == null) return null;

        return new PurchaseUrisOutEntity
        {
            Tcgplayer = source.TcgPlayer,
            Cardmarket = source.CardMarket,
            Cardhoarder = null
        };
    }

    private static ICollection<CardFaceOutEntity> MapCardFaces(ICollection<ICardFaceItrEntity> source)
    {
        if (source == null) return null;

        return [.. source.Select(face => new CardFaceOutEntity
        {
            Artist = face.Artist,
            ArtistId = face.ArtistId,
            Cmc = face.Cmc,
            ColorIndicator = face.ColorIndicator,
            Colors = face.Colors,
            Defense = face.Defense,
            FlavorText = face.FlavorText,
            IllustrationId = face.IllustrationId,
            ImageUris = MapImageUris(face.ImageUris),
            Layout = face.Layout,
            Loyalty = face.Loyalty,
            ManaCost = face.ManaCost,
            Name = face.Name,
            OracleText = face.OracleText,
            Power = face.Power,
            PrintedName = face.PrintedName,
            PrintedText = face.PrintedText,
            PrintedTypeLine = face.PrintedTypeLine,
            Toughness = face.Toughness,
            TypeLine = face.TypeLine,
            Watermark = face.Watermark
        })];
    }

    private static ICollection<AllPartsOutEntity> MapAllParts(ICollection<IAllPartsItrEntity> source)
    {
        if (source == null) return null;

        return [.. source.Select(part => new AllPartsOutEntity
        {
            Id = part.Id,
            Component = part.Component,
            Name = part.Name,
            TypeLine = part.TypeLine,
            Uri = part.Uri
        })];
    }

    private static PreviewOutEntity MapPreview(IPreviewItrEntity source)
    {
        if (source == null) return null;

        return new PreviewOutEntity
        {
            Source = source.Source,
            SourceUri = source.SourceUri,
            PreviewedAt = source.PreviewedAt
        };
    }
}
