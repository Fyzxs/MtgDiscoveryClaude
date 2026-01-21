import { gql } from '@apollo/client';

// Get all wishlisted cards for a specific user - returns full card data with collection/wishlist info
export const GET_USER_WISHLIST = gql`
  query GetUserWishlist($userId: String!) {
    userWishlist(userId: $userId) {
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
          reserved
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
          userWishlist {
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
