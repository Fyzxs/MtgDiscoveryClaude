import { gql } from '@apollo/client';

export const GET_CARDS_BY_NAME = gql`
  query GetCardsByName($cardName: CardNameArgEntityInput!) {
    cardsByName(cardName: $cardName) {
      __typename
      ... on CardsSuccessResponse {
        data {
          id
          oracleId
          multiverseIds
          tcgPlayerId
          cardMarketId
          name
          lang
          releasedAt
          uri
          scryfallUri
          layout
          highResImage
          imageStatus
          imageUris {
            small
            normal
            large
            png
            artCrop
            borderCrop
          }
          manaCost
          cmc
          typeLine
          oracleText
          power
          toughness
          loyalty
          defense
          lifeModifier
          handModifier
          colors
          colorIdentity
          colorIndicator
          keywords
          legalities {
            standard
            future
            historic
            timeless
            gladiator
            pioneer
            explorer
            modern
            legacy
            pauper
            vintage
            penny
            commander
            oathbreaker
            standardBrawl
            brawl
            alchemy
            pauperCommander
            duel
            oldschool
            premodern
            predh
          }
          games
          reserved
          foil
          nonFoil
          finishes
          oversized
          promo
          promoTypes
          reprint
          variation
          setId
          setCode
          setName
          setType
          setUri
          setSearchUri
          scryfallSetUri
          rulingsUri
          printsSearchUri
          collectorNumber
          digital
          rarity
          flavorText
          cardBackId
          artist
          artistIds
          illustrationId
          borderColor
          frame
          frameEffects
          securityStamp
          fullArt
          textless
          booster
          storySpotlight
          edhRecRank
          pennyRank
          prices {
            usd
            usdFoil
            usdEtched
            eur
            eurFoil
            tix
          }
          relatedUris {
            gatherer
            tcgplayerInfiniteArticles
            tcgplayerInfiniteDecks
            edhrec
          }
          purchaseUris {
            tcgplayer
            cardmarket
            cardhoarder
          }
          cardFaces {
            objectString
            name
            manaCost
            typeLine
            oracleText
            colors
            colorIndicator
            power
            toughness
            loyalty
            defense
            artist
            artistId
            illustrationId
            imageUris {
              small
              normal
              large
              png
              artCrop
              borderCrop
            }
            flavorText
            printedName
            printedTypeLine
            printedText
            watermark
            layout
            cmc
          }
          allParts {
            objectString
            id
            component
            name
            typeLine
            uri
          }
          printedName
          printedTypeLine
          printedText
          watermark
          contentWarning
          preview {
            source
            sourceUri
            previewedAt
          }
          producedMana
          attractions
          setGroupId
          userCollection {
            finish
            special
            count
          }
        }
      }
      ... on FailureResponse {
        status {
          message
        }
      }
    }
  }
`;

export const GET_CARDS_BY_SET_CODE = gql`
  query GetCardsBySetCode($setCode: SetCodeArgEntityInput!) {
    cardsBySetCode(setCode: $setCode) {
      __typename
      ... on CardsSuccessResponse {
        data {
          id
          oracleId
          multiverseIds
          tcgPlayerId
          cardMarketId
          name
          lang
          releasedAt
          uri
          scryfallUri
          layout
          highResImage
          imageStatus
          imageUris {
            small
            normal
            large
            png
            artCrop
            borderCrop
          }
          manaCost
          cmc
          typeLine
          oracleText
          power
          toughness
          loyalty
          defense
          lifeModifier
          handModifier
          colors
          colorIdentity
          colorIndicator
          keywords
          legalities {
            standard
            future
            historic
            timeless
            gladiator
            pioneer
            explorer
            modern
            legacy
            pauper
            vintage
            penny
            commander
            oathbreaker
            standardBrawl
            brawl
            alchemy
            pauperCommander
            duel
            oldschool
            premodern
            predh
          }
          games
          reserved
          foil
          nonFoil
          finishes
          oversized
          promo
          promoTypes
          reprint
          variation
          setId
          setCode
          setName
          setType
          setUri
          setSearchUri
          scryfallSetUri
          rulingsUri
          printsSearchUri
          collectorNumber
          digital
          rarity
          flavorText
          cardBackId
          artist
          artistIds
          illustrationId
          borderColor
          frame
          frameEffects
          securityStamp
          fullArt
          textless
          booster
          storySpotlight
          edhRecRank
          pennyRank
          prices {
            usd
            usdFoil
            usdEtched
            eur
            eurFoil
            tix
          }
          relatedUris {
            gatherer
            tcgplayerInfiniteArticles
            tcgplayerInfiniteDecks
            edhrec
          }
          purchaseUris {
            tcgplayer
            cardmarket
            cardhoarder
          }
          cardFaces {
            objectString
            name
            manaCost
            typeLine
            oracleText
            colors
            colorIndicator
            power
            toughness
            loyalty
            defense
            artist
            artistId
            illustrationId
            imageUris {
              small
              normal
              large
              png
              artCrop
              borderCrop
            }
            flavorText
            printedName
            printedTypeLine
            printedText
            watermark
            layout
            cmc
          }
          allParts {
            objectString
            id
            component
            name
            typeLine
            uri
          }
          printedName
          printedTypeLine
          printedText
          watermark
          contentWarning
          preview {
            source
            sourceUri
            previewedAt
          }
          producedMana
          attractions
          setGroupId
          userCollection {
            finish
            special
            count
          }
        }
      }
      ... on FailureResponse {
        status {
          message
        }
      }
    }
  }
`;

export const GET_CARDS_BY_IDS = gql`
  query GetCardsBatch($ids: CardIdsArgEntityInput!) {
    cardsById(ids: $ids) {
      __typename
      ... on CardsSuccessResponse {
        data {
          id
          oracleId
          name
          lang
          releasedAt
          uri
          scryfallUri
          layout
          highResImage
          imageStatus
          imageUris {
            small
            normal
            large
            png
            artCrop
            borderCrop
          }
          manaCost
          cmc
          typeLine
          oracleText
          power
          toughness
          colors
          colorIdentity
          keywords
          setId
          setCode
          setName
          setType
          collectorNumber
          digital
          rarity
          artist
          artistIds
          illustrationId
          borderColor
          frame
          prices {
            usd
            usdFoil
            usdEtched
            eur
            eurFoil
            tix
          }
          relatedUris {
            gatherer
            tcgplayerInfiniteArticles
            tcgplayerInfiniteDecks
            edhrec
          }
          purchaseUris {
            tcgplayer
            cardmarket
            cardhoarder
          }
          userCollection {
            finish
            special
            count
          }
        }
      }
      ... on FailureResponse {
        status {
          message
        }
      }
    }
  }
`;

export const GET_CARDS_BY_ARTIST = gql`
  query CardsByArtistName($artistName: ArtistNameArgEntityInput!) {
    cardsByArtistName(artistName: $artistName) {
      __typename
      ... on CardsByArtistSuccessResponse {
        data {
          id
          oracleId
          multiverseIds
          tcgPlayerId
          cardMarketId
          name
          lang
          releasedAt
          uri
          scryfallUri
          layout
          highResImage
          imageStatus
          imageUris {
            small
            normal
            large
            png
            artCrop
            borderCrop
          }
          manaCost
          cmc
          typeLine
          oracleText
          power
          toughness
          loyalty
          defense
          lifeModifier
          handModifier
          colors
          colorIdentity
          colorIndicator
          keywords
          legalities {
            standard
            future
            historic
            timeless
            gladiator
            pioneer
            explorer
            modern
            legacy
            pauper
            vintage
            penny
            commander
            oathbreaker
            standardBrawl
            brawl
            alchemy
            pauperCommander
            duel
            oldschool
            premodern
            predh
          }
          games
          reserved
          foil
          nonFoil
          finishes
          oversized
          promo
          promoTypes
          reprint
          variation
          setId
          setCode
          setName
          setType
          setUri
          setSearchUri
          scryfallSetUri
          rulingsUri
          printsSearchUri
          collectorNumber
          digital
          rarity
          flavorText
          cardBackId
          artist
          artistIds
          illustrationId
          borderColor
          frame
          frameEffects
          securityStamp
          fullArt
          textless
          booster
          storySpotlight
          edhRecRank
          pennyRank
          prices {
            usd
            usdFoil
            usdEtched
            eur
            eurFoil
            tix
          }
          relatedUris {
            gatherer
            tcgplayerInfiniteArticles
            tcgplayerInfiniteDecks
            edhrec
          }
          purchaseUris {
            tcgplayer
            cardmarket
            cardhoarder
          }
          cardFaces {
            objectString
            name
            manaCost
            typeLine
            oracleText
            colors
            colorIndicator
            power
            toughness
            loyalty
            defense
            artist
            artistId
            illustrationId
            imageUris {
              small
              normal
              large
              png
              artCrop
              borderCrop
            }
            flavorText
            printedName
            printedTypeLine
            printedText
            watermark
            layout
            cmc
          }
          allParts {
            objectString
            id
            component
            name
            typeLine
            uri
          }
          printedName
          printedTypeLine
          printedText
          watermark
          contentWarning
          preview {
            source
            sourceUri
            previewedAt
          }
          producedMana
          attractions
          setGroupId
          userCollection {
            finish
            special
            count
          }
        }
      }
      ... on FailureResponse {
        status {
          message
        }
      }
    }
  }
`;