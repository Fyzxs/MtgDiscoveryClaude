import React, { createContext, useContext, ReactNode } from 'react';
import { useAuth0, User } from '@auth0/auth0-react';
import { useUserSync, UserProfile, CollectorProfile } from '../hooks/user/useUserSync';

interface UserContextType {
  // Auth0 user data
  auth0User: User | undefined;
  isAuthenticated: boolean;
  isAuth0Loading: boolean;
  
  // Backend user profile data
  userProfile: UserProfile | null;
  collectorProfile: CollectorProfile | null;
  isFirstTimeUser: boolean;
  isUserLoading: boolean;
  userSyncError: string | null;
  
  // Actions
  syncUser: () => Promise<void>;
  refetchUserProfile: () => Promise<void>;
}

const UserContext = createContext<UserContextType | undefined>(undefined);

interface UserProviderProps {
  children: ReactNode;
}

export const UserProvider: React.FC<UserProviderProps> = ({ children }) => {
  const { user, isAuthenticated, isLoading: isAuth0Loading } = useAuth0();
  const { 
    userProfile, 
    isFirstTimeUser, 
    isLoading: isUserLoading, 
    error: userSyncError,
    syncUser,
    refetchUserProfile 
  } = useUserSync();

  const contextValue: UserContextType = {
    // Auth0 data
    auth0User: user,
    isAuthenticated,
    isAuth0Loading,
    
    // Backend user profile
    userProfile,
    collectorProfile: userProfile?.collectorProfile || null,
    isFirstTimeUser,
    isUserLoading,
    userSyncError,
    
    // Actions
    syncUser,
    refetchUserProfile
  };

  return (
    <UserContext.Provider value={contextValue}>
      {children}
    </UserContext.Provider>
  );
};

export const useUser = (): UserContextType => {
  const context = useContext(UserContext);
  if (context === undefined) {
    throw new Error('useUser must be used within a UserProvider');
  }
  return context;
};

// Convenience hooks for specific data
export const useUserProfile = () => {
  const { userProfile } = useUser();
  return userProfile;
};

export const useCollectorProfile = () => {
  const { collectorProfile } = useUser();
  return collectorProfile;
};

export const useIsCollector = () => {
  const { collectorProfile, userProfile } = useUser();
  return !!collectorProfile || !!userProfile;
};