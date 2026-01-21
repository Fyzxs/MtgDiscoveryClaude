import { gql } from '@apollo/client';

// TODO: Backend not implemented yet
// export const CREATE_USER_PROFILE = gql`
//   mutation CreateUserProfile($userProfile: CreateUserProfileInput!) {
//     createUserProfile(userProfile: $userProfile) {
//       __typename
//       ... on SuccessUserProfileResponse {
//         data {
//           id
//           auth0UserId
//           email
//           name
//           nickname
//           picture
//           isEmailVerified
//           createdAt
//           updatedAt
//           collectorProfile {
//             id
//             displayName
//             isPublic
//             totalCards
//             uniqueCards
//             favoriteSet
//             collectionValue
//           }
//         }
//       }
//       ... on FailureResponse {
//         status {
//           message
//         }
//       }
//     }
//   }
// `;

// TODO: Backend not implemented yet
// export const UPDATE_COLLECTOR_PROFILE = gql`
//   mutation UpdateCollectorProfile($collectorProfile: UpdateCollectorProfileInput!) {
//     updateCollectorProfile(collectorProfile: $collectorProfile) {
//       __typename
//       ... on SuccessCollectorProfileResponse {
//         data {
//           id
//           displayName
//           isPublic
//           totalCards
//           uniqueCards
//           favoriteSet
//           collectionValue
//           userId
//         }
//       }
//       ... on FailureResponse {
//         status {
//           message
//         }
//       }
//     }
//   }
// `;

export const REGISTER_USER = gql`
  mutation RegisterUser {
    registerUserInfo {
      __typename
      ... on UserRegistrationSuccessResponse {
        data {
          userId
          displayName
          email
          createdAt
          lastLoginAt
          isFirstLogin
        }
        status {
          message
          statusCode
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

// Alias for semantic clarity - this syncs user with backend and returns isFirstLogin
export const SYNC_USER = REGISTER_USER;

export const GET_USER_INFO = gql`
  query GetUserInfo {
    userInfo {
      userId
      email
    }
  }
`;