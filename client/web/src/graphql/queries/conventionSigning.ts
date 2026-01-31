import { gql } from '@apollo/client';

export const USER_CARDS_FOR_SIGNING = gql`
  query UserCardsForSigning($forSigningArgs: UserCardsForSigningArgEntityInput!) {
    userCardsForSigning(forSigningArgs: $forSigningArgs) {
      __typename
      ... on SigningResultSuccessResponse {
        data {
          sets {
            setId
            setCode
            setName
            releasedAt
            artistCount
            unsignedCardCount
            artists {
              artistId
              artistName
              unsignedCount
              partiallySignedCount
              cards {
                cardId
                cardName
                imageUri
                unsignedCopies
                isPartiallySigned
                isFullySigned
                collectedDetails {
                  finish
                  special
                  count
                }
              }
            }
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
