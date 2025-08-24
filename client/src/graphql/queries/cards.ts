import { gql } from '@apollo/client';


export const GET_CARDS_BY_IDS = gql`
  query GetCardsBatch($ids: CardIdsArgEntityInput!) {
    cardsById(ids: $ids) {
      __typename
      ... on SuccessCardsResponse {
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