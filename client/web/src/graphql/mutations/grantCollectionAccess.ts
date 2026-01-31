import { gql } from '@apollo/client';

export const GRANT_COLLECTION_ACCESS = gql`
  mutation GrantCollectionAccess($collectionId: String!, $targetUserId: String!, $role: String!) {
    grantCollectionAccess(args: { collectionId: $collectionId, targetUserId: $targetUserId, role: $role }) {
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
