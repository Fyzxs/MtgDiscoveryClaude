import { gql } from '@apollo/client';

export const GET_USER_CARDS_BY_SET = gql`
  query GetUserCardsBySet($setArgs: UserCardsSetArgEntityInput!) {
    userCardsBySet(setArgs: $setArgs) {
      __typename
      ... on SuccessUserCardsCollectionResponse {
        data {
          userId
          cardId
          setId
          collectedList {
            finish
            special
            count
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

export const GET_USER_CARD_BY_ID = gql`
  query GetUserCardById($cardArgs: UserCardsCardArgEntityInput!) {
    userCardsByCard(cardArgs: $cardArgs) {
      __typename
      ... on SuccessUserCardsCollectionResponse {
        data {
          userId
          cardId
          setId
          collectedList {
            finish
            special
            count
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

export const GET_USER_CARDS_BATCH = gql`
  query GetUserCardsBatch($cardsArgs: UserCardsByIdsArgEntityInput!) {
    userCardsByIds(cardsArgs: $cardsArgs) {
      __typename
      ... on SuccessUserCardsCollectionResponse {
        data {
          userId
          cardId
          setId
          collectedList {
            finish
            special
            count
          }
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

export const GET_USER_SET_CARDS = gql`
  query GetUserSetCards($setCardArgs: UserSetCardInput!) {
    userSetCards(setCardArgs: $setCardArgs) {
      __typename
      ... on UserSetCardSuccessResponse {
        data {
          userId
          setId
          totalCards
          uniqueCards
          groupsCollecting
          groups {
            key
            value {
              nonFoil {
                cards
              }
              foil {
                cards
              }
              etched {
                cards
              }
            }
          }
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