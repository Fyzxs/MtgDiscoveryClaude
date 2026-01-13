import { gql } from '@apollo/client';

// Mutation to add/update user's sealed product collection count
// Returns the updated product data with new count
export const ADD_USER_SEALED_PRODUCT = gql`
  mutation AddUserSealedProduct($args: AddUserSealedProductInput!) {
    addUserSealedProduct(args: $args) {
      __typename
      ... on AddUserSealedProductSuccessResponse {
        data {
          productUuid
          count
        }
        status {
          message
          statusCode
        }
        metaData {
          invocationId
          timeStamp
          elapsedTime
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
