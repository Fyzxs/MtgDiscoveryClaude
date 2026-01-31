import { gql } from '@apollo/client';

export const REVOKE_COLLECTION_ACCESS = gql`
  mutation RevokeCollectionAccess($collectionId: String!, $targetUserId: String!) {
    revokeCollectionAccess(args: { collectionId: $collectionId, targetUserId: $targetUserId }) {
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
