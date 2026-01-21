import { gql } from '@apollo/client';

// This mutation returns the same fields as regular card queries to enable proper caching
export const ADD_CARD_TO_COLLECTION = gql`
  mutation AddCardToCollection($args: AddCardToCollectionInput!) {
    addCardToCollection(args: $args) {
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
          statusCode
        }
      }
    }
  }
`;