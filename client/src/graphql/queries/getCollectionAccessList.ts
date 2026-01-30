import { gql } from '@apollo/client';

export const GET_COLLECTION_ACCESS_LIST = gql`
  query GetCollectionAccessList($collectionId: String!) {
    collectionAccessList(collectionId: $collectionId) {
      __typename
      ... on AuthorizedUsersSuccessResponse {
        data {
          userId
          role
          grantedAt
          grantedBy
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
