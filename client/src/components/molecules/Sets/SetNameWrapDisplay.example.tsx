/**
 * Usage Example for SetNameWrapDisplay
 *
 * This file demonstrates how to use the SetNameWrapDisplay component
 * with SetIconDisplay for wrapping text around set icons on mobile.
 */

import React from 'react';
import { SetNameWrapDisplay } from './SetNameWrapDisplay';
import { SetIconDisplay } from './SetIconDisplay';
import { getRarityColor } from '../../../theme';

// Example 1: Basic usage with SetIconDisplay
export const BasicExample: React.FC = () => {
  const setName = "Avatar: The Last Airbender Beginner Box Front Cards";
  const iconSize = 140;

  return (
    <SetNameWrapDisplay
      setName={setName}
      centerWidth={iconSize}
      centerHeight={iconSize}
      containerWidth={300} // Mobile screen width
      centerElement={
        <SetIconDisplay
          iconSvgUri="https://example.com/set-icon.svg"
          setName={setName}
          borderColor={getRarityColor('mythic')}
          size={iconSize}
        />
      }
    />
  );
};

// Example 2: Responsive usage (adapts to screen width)
export const ResponsiveExample: React.FC = () => {
  const setName = "Shadows over Innistrad Remastered";
  const iconSize = 120;

  return (
    <SetNameWrapDisplay
      setName={setName}
      centerWidth={iconSize}
      centerHeight={iconSize}
      // containerWidth not provided - will use window.innerWidth
      fontSize={12}
      centerElement={
        <SetIconDisplay
          iconSvgUri="https://example.com/set-icon.svg"
          setName={setName}
          borderColor={getRarityColor('rare')}
          size={iconSize}
        />
      }
    />
  );
};

// Example 3: Small/compact version
export const CompactExample: React.FC = () => {
  const setName = "Modern Horizons 3";
  const iconSize = 80;

  return (
    <SetNameWrapDisplay
      setName={setName}
      centerWidth={iconSize}
      centerHeight={iconSize}
      containerWidth={200}
      fontSize={10}
      fontWeight={500}
      centerElement={
        <SetIconDisplay
          iconSvgUri="https://example.com/set-icon.svg"
          setName={setName}
          borderColor={getRarityColor('uncommon')}
          size={iconSize}
        />
      }
    />
  );
};

// Example 4: With custom center element (not SetIconDisplay)
export const CustomCenterExample: React.FC = () => {
  const setName = "Foundations Jumpstart";

  return (
    <SetNameWrapDisplay
      setName={setName}
      centerWidth={100}
      centerHeight={100}
      containerWidth={250}
      centerElement={
        <div
          style={{
            width: 100,
            height: 100,
            borderRadius: '50%',
            backgroundColor: '#1976d2',
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'center',
            color: 'white',
            fontSize: '2rem',
            fontWeight: 'bold',
          }}
        >
          FJ
        </div>
      }
    />
  );
};

/**
 * Text Distribution Examples:
 *
 * Example 1: "Avatar: The Last Airbender Beginner Box Front Cards"
 * With containerWidth=300, fontSize=14:
 * - Top: "Avatar: The Last Airbender"
 * - Left: "Beginner Box"
 * - Right: "Front Cards"
 *
 * Example 2: "Modern Horizons 3"
 * With containerWidth=300, fontSize=14:
 * - Top: "Modern Horizons 3"
 * - Left: ""
 * - Right: ""
 *
 * Example 3: "The Lord of the Rings: Tales of Middle-earth Commander Decks"
 * With containerWidth=300, fontSize=12:
 * - Top: "The Lord of the Rings: Tales of Middle-earth"
 * - Left: "Commander"
 * - Right: "Decks"
 */
