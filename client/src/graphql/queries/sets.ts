import { gql } from '@apollo/client';

export const GET_ALL_SETS = gql`
  query GetAllSets($args: AllSetsArgEntityInput) {
    allSets(args: $args) {
      __typename
      ... on SetsSuccessResponse {
        data {
          id
          code
          tcgPlayerId
          name
          uri
          scryfallUri
          searchUri
          releasedAt
          setType
          cardCount
          printedSize
          digital
          nonFoilOnly
          foilOnly
          block
          iconSvgUri
          userCollection {
            totalCards
            uniqueCards
            collecting {
              setGroupId
              collecting
              count
            }
            groups {
              rarity
              group {
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

export const GET_SETS_BY_CODE = gql`
  query GetMultipleSetsByCode($codes: SetCodesArgEntityInput!) {
    setsByCode(codes: $codes) {
      __typename
      ... on SetsSuccessResponse {
        data {
          id
          code
          tcgPlayerId
          name
          uri
          scryfallUri
          searchUri
          releasedAt
          setType
          cardCount
          printedSize
          digital
          nonFoilOnly
          foilOnly
          block
          iconSvgUri
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

export const GET_SET_BY_CODE_WITH_GROUPINGS = gql`
  query GetSetByCodeWithGroupings($codes: SetCodesArgEntityInput!) {
    setsByCode(codes: $codes) {
      __typename
      ... on SetsSuccessResponse {
        data {
          id
          code
          tcgPlayerId
          name
          uri
          scryfallUri
          searchUri
          setType
          cardCount
          printedSize
          releasedAt
          digital
          nonFoilOnly
          foilOnly
          block
          iconSvgUri
          groupings {
            id
            displayName
            order
            cardCount
            rawQuery
            filters {
              collectorNumberRange {
                min
                max
                orConditions
              }
              properties
            }
          }
          userCollection {
            totalCards
            uniqueCards
            collecting {
              setGroupId
              collecting
              count
            }
            groups {
              rarity
              group {
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