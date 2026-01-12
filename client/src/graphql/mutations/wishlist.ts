import { gql } from '@apollo/client';

// Add a card to the wishlist (use negative count to remove)
// Returns the card with updated userWishlist data
export const ADD_CARD_TO_WISHLIST = gql`
  mutation AddCardToWishlist($args: AddCardToWishlistInput!) {
    addCardToWishlist(args: $args) {
      __typename
      ... on CardsSuccessResponse {
        data {
          id
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
