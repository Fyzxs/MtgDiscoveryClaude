import { gql } from '@apollo/client';

export const GET_MY_COLLECTIONS = gql`
  query GetMyCollections {
    myCollections {
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
