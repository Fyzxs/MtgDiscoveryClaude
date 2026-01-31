import { gql } from '@apollo/client';

export const GET_ALL_SETS = gql`
  query GetAllSets($args: AllSetsInput) {
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
          groupings {
            id
            cardCounts {
              total
              nonFoil
              foil
              etched
            }
          }
          userCollection {
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
  query GetMultipleSetsByCode($codes: SetCodesInput!) {
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
  query GetSetByCodeWithGroupings($codes: SetCodesInput!) {
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
            cardCounts {
              total
              nonFoil
              foil
              etched
            }
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
              counts {
                total
                nonFoil
                foil
                etched
              }
              collectingFinishes
            }
            groups {
              setGroupId
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