import type React from 'react';
import {
  CollectionsBookmarkIcon,
  SearchIcon,
  BrushIcon,
  EventNoteIcon,
} from '../../../atoms';

export interface FeatureCardData {
  id: string;
  icon: React.ComponentType;
  title: string;
  description: string;
  ctaLabel: string;
  route: string;
  authRequired: boolean;
}

export const featureCards: FeatureCardData[] = [
  {
    id: 'browse-sets',
    icon: CollectionsBookmarkIcon,
    title: 'Browse Sets',
    description:
      'View every MTG set ever made. Track completion and view card checklists.',
    ctaLabel: 'Explore Sets',
    route: '/sets',
    authRequired: false,
  },
  {
    id: 'search-cards',
    icon: SearchIcon,
    title: 'Search Cards',
    description:
      'Find any card by name, type, color, or text. View all printings across sets.',
    ctaLabel: 'Search Now',
    route: '/search/cards',
    authRequired: false,
  },
  {
    id: 'discover-artists',
    icon: BrushIcon,
    title: 'Discover Artists',
    description:
      'Explore cards by your favorite artists. Perfect for collecting signatures.',
    ctaLabel: 'Search Artists',
    route: '/search/artists',
    authRequired: false,
  },
  {
    id: 'convention-signing',
    icon: EventNoteIcon,
    title: 'Convention Signing',
    description:
      'Plan which cards to bring for artist signings at conventions and events.',
    ctaLabel: 'Plan Signings',
    route: '/convention-signing',
    authRequired: true,
  },
];
