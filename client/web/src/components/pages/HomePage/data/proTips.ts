export interface ProTip {
  title: string;
  body: string;
  ctaLabel: string;
  ctaRoute: string;
}

export const proTips: ProTip[] = [
  {
    title: 'Browse Sets with Binder View',
    body: 'Switch to binder view when exploring sets to see cards in their natural collector number order, just like your physical binder.',
    ctaLabel: 'Explore Sets',
    ctaRoute: '/sets',
  },
  {
    title: 'Filter by Rarity',
    body: 'Narrow your search results by rarity to quickly find mythics, rares, or those common playsets you need.',
    ctaLabel: 'Search Cards',
    ctaRoute: '/search',
  },
  {
    title: 'Track Set Completion',
    body: 'Mark cards as owned to automatically calculate your completion percentage for each set in your collection.',
    ctaLabel: 'View Your Collection',
    ctaRoute: '/collection',
  },
  {
    title: 'Plan Convention Signings',
    body: 'Build wishlists of cards you want signed and check which artists will be at upcoming conventions.',
    ctaLabel: 'Create Wishlist',
    ctaRoute: '/wishlist',
  },
  {
    title: 'Discover New Artists',
    body: 'Explore Magic artists and their complete card galleries to find new favorites or complete artist collections.',
    ctaLabel: 'Browse Artists',
    ctaRoute: '/artists',
  },
  {
    title: 'Build Your Wishlist',
    body: 'Add cards to your wishlist to track what you still need, whether for completing sets or getting signatures.',
    ctaLabel: 'Manage Wishlist',
    ctaRoute: '/wishlist',
  },
  {
    title: 'Organize by Set Groups',
    body: 'Use set groupings to organize your collection by blocks, years, or custom categories for easier tracking.',
    ctaLabel: 'Organize Collection',
    ctaRoute: '/collection/organize',
  },
];
