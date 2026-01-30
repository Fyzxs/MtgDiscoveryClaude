import { gql } from '@apollo/client';

export const TRANSFER_COLLECTION_OWNERSHIP = gql`
  mutation TransferCollectionOwnership($collectionId: String!, $targetUserId: String!) {
    transferCollectionOwnership(args: { collectionId: $collectionId, targetUserId: $targetUserId }) {
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
