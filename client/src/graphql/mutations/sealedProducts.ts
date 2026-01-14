import { gql } from '@apollo/client';

// Mutation to add/update user's sealed product collection count
// Returns the updated sealed product(s) with userQuantity populated
// Matches the card collection pattern exactly
export const ADD_SEALED_PRODUCT_TO_COLLECTION = gql`
  mutation AddSealedProductToCollection($args: AddUserSealedProductInput!) {
    addUserSealedProduct(args: $args) {
      __typename
      ... on AddUserSealedProductSuccessResponse {
        data {
          uuid
          setId
          name
          category
          imageUrl
          userQuantity
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
