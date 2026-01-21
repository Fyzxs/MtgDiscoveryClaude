import React, { createContext, useContext } from 'react';

interface LinkParamsContextValue {
  additionalParams?: Record<string, string>;
}

const LinkParamsContext = createContext<LinkParamsContextValue>({});

interface LinkParamsProviderProps {
  additionalParams?: Record<string, string>;
  children: React.ReactNode;
}

/**
 * Provider for additional URL parameters to be included in navigation links.
 * Used by pages that need to add params like formats=paper to all outgoing links.
 */
export const LinkParamsProvider: React.FC<LinkParamsProviderProps> = ({
  additionalParams,
  children
}) => {
  return (
    <LinkParamsContext.Provider value={{ additionalParams }}>
      {children}
    </LinkParamsContext.Provider>
  );
};

/**
 * Hook to get additional link parameters from context.
 */
export const useLinkParams = (): Record<string, string> | undefined => {
  const context = useContext(LinkParamsContext);
  return context.additionalParams;
};
