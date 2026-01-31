import { gql } from '@apollo/client';

// TODO: Backend not implemented yet - wrong type names
// export const GET_USER_CARDS_BY_SET = gql`
//   query GetUserCardsBySet($setArgs: UserCardsSetArgEntityInput!) {
//     userCardsBySet(setArgs: $setArgs) {
//       __typename
//       ... on SuccessUserCardsCollectionResponse {
//         data {
//           userId
//           cardId
//           setId
//           collectedList {
//             finish
//             special
//             count
//           }
//         }
//         status {
//           message
//           statusCode
//         }
//       }
//       ... on FailureResponse {
//         status {
//           message
//           statusCode
//         }
//       }
//     }
//   }
// `;

// TODO: Backend not implemented yet - wrong type names
// export const GET_USER_CARD_BY_ID = gql`
//   query GetUserCardById($cardArgs: UserCardsCardArgEntityInput!) {
//     userCardsByCard(cardArgs: $cardArgs) {
//       __typename
//       ... on SuccessUserCardsCollectionResponse {
//         data {
//           userId
//           cardId
//           setId
//           collectedList {
//             finish
//             special
//             count
//           }
//         }
//         status {
//           message
//           statusCode
//         }
//       }
//       ... on FailureResponse {
//         status {
//           message
//           statusCode
//         }
//       }
//     }
//   }
// `;

// TODO: Backend not implemented yet - wrong type names
// export const GET_USER_CARDS_BATCH = gql`
//   query GetUserCardsBatch($cardsArgs: UserCardsByIdsArgEntityInput!) {
//     userCardsByIds(cardsArgs: $cardsArgs) {
//       __typename
//       ... on SuccessUserCardsCollectionResponse {
//         data {
//           userId
//           cardId
//           setId
//           collectedList {
//             finish
//             special
//             count
//           }
//         }
//       }
//       ... on FailureResponse {
//         status {
//           message
//           statusCode
//         }
//       }
//     }
//   }
// `;

// TODO: UserSetCard query endpoint was removed - data now comes from set.userCollection
// export const GET_USER_SET_CARDS = gql`
//   query GetUserSetCards($setCardArgs: UserSetCardInput!) {
//     userSetCards(setCardArgs: $setCardArgs) {
//       __typename
//       ... on UserSetCardSuccessResponse {
//         data {
//           userId
//           setId
//           totalCards
//           uniqueCards
//           collecting {
//             setGroupId
//             collecting
//             count
//             collectingFinishes
//           }
//           groups {
//             setGroupId
//             group {
//               nonFoil { cards }
//               foil { cards }
//               etched { cards }
//             }
//           }
//         }
//         status {
//           message
//           statusCode
//         }
//       }
//       ... on FailureResponse {
//         status {
//           message
//           statusCode
//         }
//       }
//     }
//   }
// `;

export const ADD_SET_GROUP_TO_USER_SET_CARD = gql`
  mutation AddSetGroupToUserSetCard($input: AddSetGroupToUserSetCardInput!) {
    addSetGroupToUserSetCard(input: $input) {
      __typename
      ... on UserSetCardSuccessResponse {
        data {
          userId
          setId
          totalCards
          uniqueCards
          collecting {
            setGroupId
            collecting
            counts {
              total
              nonFoil
              foil
              etched
            }
            collectingFinishes
          }
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