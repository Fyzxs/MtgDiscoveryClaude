import { gql } from '@apollo/client';

// TODO: Backend not implemented yet
// export const UPDATE_COLLECTION = gql`
//   mutation UpdateCollection($input: UpdateCollectionInput!) {
//     updateCollection(input: $input) {
//       ... on SuccessCardResponse {
//         __typename
//         card {
//           id
//           name
//           setCode
//           setName
//           collectorNumber
//           rarity
//           artist
//           userCollection {
//             finish
//             special
//             count
//           }
//         }
//       }
//       ... on FailureResponse {
//         __typename
//         status {
//           message
//         }
//       }
//     }
//   }
// `;