import { useState, useEffect, useCallback } from 'react';
import { logger } from '../../utils/logger';
import { useAuth0 } from '@auth0/auth0-react';
import { useQuery } from '@apollo/client/react';
import { GET_USER_INFO } from '../../graphql/mutations/user';
import { getTokenReadyState } from '../../graphql/apollo-client';

export interface UserProfile {
  id: string;
  auth0UserId: string;
  email: string;
  name?: string;
  nickname?: string;
  picture?: string;
  isEmailVerified: boolean;
  createdAt: string;
  updatedAt: string;
  collectorProfile?: CollectorProfile;
}

export interface CollectorProfile {
  id: string;
  displayName: string;
  isPublic: boolean;
  totalCards: number;
  uniqueCards: number;
  favoriteSet?: string;
  collectionValue?: number;
}

interface UserInfoQueryData {
  userInfo?: {
    userId: string;
    email: string;
  };
}

interface UserInfoQueryResult {
  loading: boolean;
  data?: UserInfoQueryData;
  error?: Error;
  refetch: () => Promise<unknown>;
}

export interface UserSyncState {
  userProfile: UserProfile | null;
  isLoading: boolean;
  error: string | null;
  isFirstTimeUser: boolean;
  syncUser: () => Promise<void>;
  refetchUserProfile: () => Promise<void>;
}

export const useUserSync = (): UserSyncState => {
  const { user, isAuthenticated, isLoading: auth0Loading } = useAuth0();
  const [userProfile, setUserProfile] = useState<UserProfile | null>(null);
  const [error, setError] = useState<string | null>(null);
  const [isFirstTimeUser, setIsFirstTimeUser] = useState(false);
  const [tokenReady, setTokenReady] = useState(false);

  useEffect(() => {
    const checkTokenReady = () => {
      const ready = getTokenReadyState();
      if (ready !== tokenReady) {
        logger.debug('useUserSync - Token ready state changed:', ready);
        setTokenReady(ready);
      }
    };

    if (isAuthenticated && auth0Loading === false) {
      const interval = setInterval(checkTokenReady, 100);
      return () => clearInterval(interval);
    }
  }, [isAuthenticated, auth0Loading, tokenReady]);

  // Only query user info when fully authenticated and token is ready
  const shouldQueryUserInfo = isAuthenticated && auth0Loading === false && tokenReady && user !== undefined;

  // Debug logging for authentication state
  useEffect(() => {
    logger.debug('useUserSync - Auth state:', {
      isAuthenticated,
      auth0Loading,
      tokenReady,
      hasUser: user !== undefined,
      shouldQuery: shouldQueryUserInfo
    });
  }, [isAuthenticated, auth0Loading, tokenReady, user, shouldQueryUserInfo]);

  // Use GET_USER_INFO authenticated query
  const { loading: userInfoLoading, data: userInfoData, error: userInfoError, refetch: refetchUserInfo } = useQuery<UserInfoQueryData>(GET_USER_INFO, {
    skip: shouldQueryUserInfo === false,
    errorPolicy: 'all'
  }) as UserInfoQueryResult;

  // Handle user info query results
  useEffect(() => {
    if (userInfoData?.userInfo) {
      // Convert simple userInfo to UserProfile format
      const simpleProfile: UserProfile = {
        id: userInfoData.userInfo.userId,
        auth0UserId: user?.sub || '',
        email: userInfoData.userInfo.email,
        name: user?.name,
        nickname: user?.nickname,
        picture: user?.picture,
        isEmailVerified: user?.email_verified || false,
        createdAt: new Date().toISOString(),
        updatedAt: new Date().toISOString()
      };
      setUserProfile(simpleProfile);
      setIsFirstTimeUser(false);
      logger.debug('User info loaded:', simpleProfile);
    } else if (userInfoError && shouldQueryUserInfo) {
      // Only log errors when we should be querying (not when skipped)
      logger.error('User info query error:', userInfoError);
      setError('Failed to load user info');
      setIsFirstTimeUser(true);
    }
  }, [userInfoData, userInfoError, user, shouldQueryUserInfo]);

  const syncUser = useCallback(async () => {
    // Simply refetch user info
    if (refetchUserInfo) {
      try {
        await refetchUserInfo();
      } catch (err) {
        logger.error('User sync error:', err);
        setError('Failed to sync user info');
      }
    }
  }, [refetchUserInfo]);

  const refetchUserProfile = useCallback(async () => {
    if (refetchUserInfo) {
      await refetchUserInfo();
    }
  }, [refetchUserInfo]);

  // Auto-sync user when Auth0 authentication completes and token is ready
  useEffect(() => {
    if (shouldQueryUserInfo && userProfile === null && userInfoLoading === false) {
      syncUser();
    }
  }, [shouldQueryUserInfo, userProfile, userInfoLoading, syncUser]);

  return {
    userProfile,
    isLoading: auth0Loading || userInfoLoading,
    error,
    isFirstTimeUser,
    syncUser,
    refetchUserProfile
  };
};