import { useState, useEffect, useCallback } from 'react';
import { useAuth0 } from '@auth0/auth0-react';
import { useMutation, useQuery } from '@apollo/client';
import { SYNC_USER_PROFILE, GET_USER_PROFILE } from '../../graphql/mutations/user';

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

  const [syncUserProfile] = useMutation(SYNC_USER_PROFILE);
  
  const { loading: profileLoading, refetch: refetchProfile } = useQuery(GET_USER_PROFILE, {
    skip: !isAuthenticated,
    errorPolicy: 'all',
    onCompleted: (data) => {
      if (data?.userProfile?.__typename === 'SuccessUserProfileResponse') {
        setUserProfile(data.userProfile.data);
        setIsFirstTimeUser(false);
      } else if (data?.userProfile?.__typename === 'FailureResponse') {
        // User doesn't exist in backend yet
        setIsFirstTimeUser(true);
      }
    },
    onError: (error) => {
      console.log('User profile query error (expected for new users):', error);
      setIsFirstTimeUser(true);
    }
  });

  const syncUser = useCallback(async () => {
    if (!isAuthenticated || !user) {
      return;
    }

    try {
      setError(null);
      
      const userInput = {
        auth0UserId: user.sub || '',
        email: user.email || '',
        name: user.name || user.email?.split('@')[0] || '',
        nickname: user.nickname || user.name || user.email?.split('@')[0] || '',
        picture: user.picture || '',
        isEmailVerified: user.email_verified || false
      };

      const { data } = await syncUserProfile({
        variables: {
          userProfile: userInput
        }
      });

      if (data?.syncUserProfile?.__typename === 'SuccessUserProfileResponse') {
        setUserProfile(data.syncUserProfile.data);
        setIsFirstTimeUser(false);
        console.log('User profile synced successfully:', data.syncUserProfile.data);
      } else if (data?.syncUserProfile?.__typename === 'FailureResponse') {
        setError(data.syncUserProfile.status.message);
      }
    } catch (err) {
      const errorMessage = err instanceof Error ? err.message : 'Failed to sync user profile';
      setError(errorMessage);
      console.error('User sync error:', err);
    }
  }, [isAuthenticated, user, syncUserProfile]);

  const refetchUserProfile = useCallback(async () => {
    if (refetchProfile) {
      await refetchProfile();
    }
  }, [refetchProfile]);

  // Auto-sync user when Auth0 authentication completes
  useEffect(() => {
    if (isAuthenticated && user && !userProfile && !profileLoading) {
      syncUser();
    }
  }, [isAuthenticated, user, userProfile, profileLoading, syncUser]);

  return {
    userProfile,
    isLoading: auth0Loading || profileLoading,
    error,
    isFirstTimeUser,
    syncUser,
    refetchUserProfile
  };
};