import { gql } from '@apollo/client';

export const GET_COLLECTION = gql`
  query GetCollection($collectionId: String!) {
    collection(collectionId: $collectionId) {
      __typename
      ... on CollectionsSuccessResponse {
        data {
          collectionId
          ownerId
          name
          type
          visibility
          isDefault
          authorizedUsers {
            userId
            role
            grantedAt
            grantedBy
          }
          createdAt
          updatedAt
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
