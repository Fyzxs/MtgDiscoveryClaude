import { gql } from '@apollo/client';

// Get all wishlisted cards for a specific user - returns full card data with embedded wishlist info
export const GET_USER_WISHLIST = gql`
  query GetUserWishlist($userId: String!) {
    userWishlist(userId: $userId) {
      __typename
      ... on CardsSuccessResponse {
        data {
          id
          name
          releasedAt
          setId
          setCode
          setName
          artist
          artistIds
          collectorNumber
          rarity
          imageUris {
            small
            normal
            large
            png
            artCrop
            borderCrop
          }
          cardFaces {
            name
            imageUris {
              small
              normal
              large
              png
              artCrop
              borderCrop
            }
          }
          foil
          nonFoil
          finishes
          prices {
            usd
            usdFoil
            usdEtched
          }
          purchaseUris {
            tcgplayer
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
          statusCode
        }
      }
    }
  }
`;
